using PharmaBlockchainBackend.Domain.Enums;

namespace PharmaBlockchainBackend.Api.Features.ProtocolActions.GetDetails
{
    public record Request
    {
        public required ProtocolType ProtocolType { get; init; }
        public required Guid PalletCode { get; init; }
    }
}
