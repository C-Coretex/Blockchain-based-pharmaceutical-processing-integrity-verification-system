using PharmaBlockchainBackend.Domain.Enums;

namespace PharmaBlockchainBackend.Api.Features.ProtocolActions.List
{
    public record Request
    {
        public int Top { get; init; } = 100;
        public int Skip { get; init; } = 0;

        public Guid? CmoId { get; init; }
        public ProtocolType? ProtocolType { get; init; }
        public Guid? PalletCode { get; init; }
        public Guid? PackageCode { get; init; }
    }
}
