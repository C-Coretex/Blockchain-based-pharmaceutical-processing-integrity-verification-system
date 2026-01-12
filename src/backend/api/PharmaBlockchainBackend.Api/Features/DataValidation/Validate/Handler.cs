﻿using PharmaBlockchainBackend.Domain.Helpers;
using PharmaBlockchainBackend.Domain.Blockchain;

namespace PharmaBlockchainBackend.Api.Features.DataValidation.Validate
{
    public class Handler
    {
        private readonly IBlockchainHashReader _hashReader;

        public Handler(IBlockchainHashReader hashReader)
        {
            _hashReader = hashReader;
        }
        public async Task<Response> Handle(Request request, CancellationToken ct)
        {
            List<PackageValidationResult> invalidPackages = [];



            foreach (var package in request.Packages)
            {
                List<int> invalidSteps = [];
                foreach (var packageStep in package.Steps)
                {
                    var actualHash = HashHelpers.CalculateHash(request.ProtocolType, packageStep.StepNumber, package.PackageCode, packageStep.AdditionalData);

                    IReadOnlyCollection<byte[]> blockchainHashes;

                    long unixTimestamp = ((DateTimeOffset)packageStep.Timestamp).ToUnixTimeSeconds();
                    try
                    {
                        blockchainHashes = await _hashReader
                                .GetHashesByTimestampAsync(unixTimestamp, ct);


                        if (blockchainHashes == null || !blockchainHashes.Any())
                        {

                            invalidSteps.Add(packageStep.StepNumber);
                            continue;
                        }

                        foreach (var blockHash in blockchainHashes)
                        {
                            var expectedHash = blockHash;
                            var expectedTimestamp = packageStep.Timestamp; //TODO Guess we dont need that

                            if (packageStep.Timestamp != expectedTimestamp || !expectedHash.SequenceEqual(actualHash))
                                invalidSteps.Add(packageStep.StepNumber);
                        }
                    }
                    catch
                    {
                        // timestamp not found on blockchain
                        invalidSteps.Add(packageStep.StepNumber);
                        continue;
                    }


                }

                if (invalidSteps.Count > 0)
                    invalidPackages.Add(new()
                    {
                        PackageCode = package.PackageCode,
                        StepNumber = invalidSteps
                    });
            }

            return new Response
            {
                IsValid = invalidPackages.Count == 0,
                InvalidPackages = invalidPackages
            };
        }
    }
}
