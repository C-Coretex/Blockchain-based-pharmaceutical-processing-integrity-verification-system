using PharmaBlockchainBackend.Domain.Enums;
using PharmaBlockchainBackend.Infrastructure.Aggregates;

namespace PharmaBlockchainBackend.Api.Features.ProtocolActions.List
{
    public record Response
    {
        public required Guid CmoId { get; init; }
        public required ProtocolType ProtocolType { get; init; }
        public required int StepsSubmitted { get; init; }
        public required int PossibleProtocolSteps { get; init; }
        public required Guid PalletCode { get; init; }
        public required ICollection<Guid> PackageCodes { get; init; }

        public static Response Create(ProtocolReportAggregate aggregate)
        {
            return new Response
            {
                CmoId = aggregate.CmoId,
                ProtocolType = aggregate.ProtocolType,
                StepsSubmitted = aggregate.StepsSubmitted,
                PossibleProtocolSteps = aggregate.PossibleProtocolSteps,
                PalletCode = aggregate.PalletCode,
                PackageCodes = aggregate.PackageCodes
            };
        }
    }
}
