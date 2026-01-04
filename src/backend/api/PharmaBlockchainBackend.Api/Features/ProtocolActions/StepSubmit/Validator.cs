using PharmaBlockchainBackend.Domain.Enums;

namespace PharmaBlockchainBackend.Api.Features.ProtocolActions.StepSubmit
{
    public static class Validator
    {
        public static bool IsValid(Request request, out string? error)
        {
            error = request switch
            {
                { CmoId: var cmo } when cmo == Guid.Empty => "CmoId is required.",
                { ProtocolType: ProtocolType.None } => "ProtocolType cannot be None.",
                { StepNumber: < 0 } => "StepNumber must be greater or equal to 0.",
                { StepNumber: var step } when step > ProtocolTypeHelpers.ProtocolTypeMaxSteps[request.ProtocolType] => $"StepNumber cannot be greater than {ProtocolTypeHelpers.ProtocolTypeMaxSteps[request.ProtocolType]}.",
                { PackageCodes: not { Length: > 0 } }=> "At least one PackageCode must be provided.",
                { PackageCodes: var codes } when codes.Any(g => g == Guid.Empty) => "PackageCodes cannot contain empty GUIDs.",
                { PalletCode : var pallet } when pallet.HasValue && pallet == Guid.Empty => "PalletId cannot be an empty GUID.",
                _ => null
            };

            return error is null;
        }
    }
}
