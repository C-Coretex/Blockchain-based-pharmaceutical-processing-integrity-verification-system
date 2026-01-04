using Microsoft.EntityFrameworkCore;
using PharmaBlockchainBackend.Domain.Helpers;
using PharmaBlockchainBackend.Domain.Blockchain;
using PharmaBlockchainBackend.Infrastructure;
using PharmaBlockchainBackend.Infrastructure.Entities;

namespace PharmaBlockchainBackend.Api.Features.ProtocolActions.StepSubmit
{
    public class Handler(IRepository<ProtocolStep> protocolStepRepository, IRepository<Package> packageRepository, IRepository<Pallet> palletRepository, IRepository<Cmo> cmoRepository,IBlockchainHashWriter blockchainHashWriter)
    {
        public async Task Handle(Request request, CancellationToken ct)
        {
            var cmoExists = await cmoRepository.DbSet.AnyAsync(c => c.Id == request.CmoId, ct);
            if (!cmoExists)
                throw new InvalidOperationException($"CMO with Id {request.CmoId} does not exist.");

            request = request with { PackageCodes = [.. request.PackageCodes.Distinct()] };

            List<(Guid packageCode, byte[] hash)> packageHashes = [];
            foreach (var packageCode in request.PackageCodes)
            {
                var hash = HashHelpers.CalculateHash(request.ProtocolType, request.StepNumber, packageCode, request.AdditionalData);
                packageHashes.Add((packageCode, hash));
            }

            // Send data to blockchain
            var timestamp_temp = await blockchainHashWriter
                .RecordHashesAsync(
                    packageHashes.Select(h => h.hash).ToList(),
                    ct);

            DateTime timestamp = default;
            //TODO make better error handling here
            if (timestamp_temp.HasValue)
            {
                timestamp = (DateTime)timestamp_temp;
            }
            else
            {
                throw new InvalidOperationException("Failed to record hashes on the blockchain.");
            }

            //Save data to database
            //Add packages and pallets if they do not exist
            var existingPackages = await packageRepository.DbSet
                .Where(p => request.PackageCodes.Contains(p.Code))
                .Select(p => new { PalletCode = p.Pallet.Code, PackageCode = p.Code })
                .ToListAsync(ct);

            if(request.PalletCode is not null && existingPackages.Any(p => p.PalletCode != request.PalletCode))
                throw new InvalidOperationException("Some packages are associated with a different pallet than the one provided in the request.");

            var nonExistingPackages = request.PackageCodes
                .Except(existingPackages.Select(e => e.PackageCode))
                .ToList();

            if (nonExistingPackages.Count != 0)
            {
                if (request.PalletCode is null)
                    throw new InvalidOperationException("Some packages do not have an associated pallet, but no PalletCode was provided in the request.");

                var pallet = await palletRepository.DbSet.AsNoTracking().FirstOrDefaultAsync(p => p.Code == request.PalletCode.Value, ct);
                if (pallet is null)
                {
                    pallet ??= new Pallet() { Code = request.PalletCode.Value };
                    palletRepository.DbSet.Add(pallet);
                    await palletRepository.DbContext.SaveChangesAsync(ct);
                }

                foreach (var package in nonExistingPackages)
                    packageRepository.DbSet.Add(new Package { Code = package, PalletId = pallet.Id });

                await packageRepository.DbContext.SaveChangesAsync(ct);
            }

            //Save protocol step
            var packages = packageRepository.DbSet.Include(p => p.ProtocolSteps).AsNoTracking()
                .Where(p => request.PackageCodes.Contains(p.Code))
                .AsAsyncEnumerable();

            await foreach(var package in packages)
            {
                var protocolStep = package.ProtocolSteps.FirstOrDefault(ps => ps.ProtocolType == request.ProtocolType && ps.StepNumber == request.StepNumber);
                if (protocolStep is not null)
                {
                    protocolStep.Hash = packageHashes.First(h => h.packageCode == package.Code).hash;
                    protocolStep.Timestamp = timestamp;
                    protocolStep.AdditionalData = request.AdditionalData;

                    protocolStepRepository.DbSet.Update(protocolStep);
                    continue;
                }

                protocolStep = new ProtocolStep
                {
                    PackageId = package.Id,
                    CmoId = request.CmoId,
                    ProtocolType = request.ProtocolType,
                    StepNumber = request.StepNumber,
                    PalletId = package.PalletId,
                    Hash = packageHashes.First(h => h.packageCode == package.Code).hash,
                    Timestamp = timestamp,
                    AdditionalData = request.AdditionalData
                };
                protocolStepRepository.DbSet.Add(protocolStep);
            }

            await protocolStepRepository.DbContext.SaveChangesAsync(ct);
        }
    }
}
