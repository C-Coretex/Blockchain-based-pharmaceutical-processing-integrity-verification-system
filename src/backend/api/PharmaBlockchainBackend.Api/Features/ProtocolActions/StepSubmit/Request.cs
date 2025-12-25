using PharmaBlockchainBackend.Domain.Enums;

namespace PharmaBlockchainBackend.Api.Features.ProtocolActions.StepSubmit
{
    public record Request
    {
        public required Guid CmoId { get; init; }
        public required ProtocolType ProtocolType { get; init; }
        public required int StepNumber { get; init; }
        public required Guid[] PackageCodes { get; init; }

        public object? AdditionalData { get; init; }
    }
}
