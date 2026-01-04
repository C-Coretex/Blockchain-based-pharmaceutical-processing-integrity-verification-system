using PharmaBlockchainBackend.Domain.Enums;

namespace PharmaBlockchainBackend.Api.Features.ProtocolActions.GetDetails
{
    public static class Validator
    {
        public static bool IsValid(Request request, out string? error)
        {
            error = request switch
            {
                { ProtocolType: ProtocolType.None } => "ProtocolType cannot be None.",
                { PalletCode: var pallet } when pallet == Guid.Empty => "PalletId cannot be an empty GUID.",
                _ => null
            };

            return error is null;
        }
    }
}
