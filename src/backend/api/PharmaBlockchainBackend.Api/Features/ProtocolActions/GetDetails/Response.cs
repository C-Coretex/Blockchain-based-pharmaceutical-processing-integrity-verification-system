using PharmaBlockchainBackend.Domain.Enums;

namespace PharmaBlockchainBackend.Api.Features.ProtocolActions.GetDetails
{
    public record Response
    {
        public required Guid CmoId { get; init; }
        public required string CmoName { get; init; }
        public required ProtocolType ProtocolType { get; init; }
        public required Guid PalletCode { get; init; }
        public required int PossibleProtocolSteps { get; init; }
        public required ICollection<Package> Packages { get; init; }
    }

    public record Package
    {
        public required Guid PackageCode { get; init; }
        public required ICollection<StepResponse> Steps { get; init; }
    }

    public record StepResponse
    {
        public required int StepNumber { get; init; }
        public object? AdditionalData { get; init; }
    }
}
