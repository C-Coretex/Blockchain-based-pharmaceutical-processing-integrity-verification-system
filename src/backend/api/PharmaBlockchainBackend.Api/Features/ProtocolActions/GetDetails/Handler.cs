using Microsoft.EntityFrameworkCore;
using PharmaBlockchainBackend.Domain.Enums;
using PharmaBlockchainBackend.Infrastructure;
using PharmaBlockchainBackend.Infrastructure.Entities;

namespace PharmaBlockchainBackend.Api.Features.ProtocolActions.GetDetails
{
    public class Handler(IRepository<ProtocolStep> protocolStepRepository)
    {
        public async Task<Response> Handle(Request request, CancellationToken ct)
        {
            var query = await protocolStepRepository.DbSet.Include(ps => ps.Cmo).Include(ps => ps.Package)
                .Where(ps => ps.ProtocolType == request.ProtocolType && ps.Pallet.Code == request.PalletCode)
                .ToListAsync(ct);

            var packages = query.GroupBy(ps => ps.Package.Code)
                .Select(g => new Package
                {
                    PackageCode = g.Key,
                    Steps = [.. g.Select(step => new StepResponse
                    {
                        StepNumber = step.StepNumber,
                        AdditionalData = step.AdditionalData
                    })]
                });

            return new Response()
            {
                CmoId = query.First().CmoId,
                CmoName = query.First().Cmo.Name,
                ProtocolType = request.ProtocolType,
                PalletCode = request.PalletCode,
                PossibleProtocolSteps = ProtocolTypeHelpers.ProtocolTypeMaxSteps[request.ProtocolType],
                Packages = [.. packages]
            };
        }
    }
}
