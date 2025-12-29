using PharmaBlockchainBackend.Domain.Enums;
using PharmaBlockchainBackend.Infrastructure.Entities;

namespace PharmaBlockchainBackend.Infrastructure.Aggregates
{
    public record ProtocolReportAggregate
    {
        public Guid LatestStepId { get; init; }

        public required Guid CmoId { get; init; }
        public required string CmoName { get; init; }

        public required ProtocolType ProtocolType { get; init; }
        public required int StepsSubmitted { get; init; }
        public required int PossibleProtocolSteps { get; init; }
        
        public required Guid PalletCode { get; init; }
        public required ICollection<Guid> PackageCodes { get; init; }

        public ProtocolReportAggregate PopulateInMemory()
            => this with { PossibleProtocolSteps = ProtocolTypeHelpers.ProtocolTypeMaxSteps[ProtocolType] };
    }

    public class ProtocolReportAggregateRepository : IAggregateRepository<ProtocolReportAggregate>
    {
        public IQueryable<ProtocolReportAggregate> Query { get; init; }

        public ProtocolReportAggregateRepository(IRepository<ProtocolStep> repository)
        {
            var query = repository.DbSet.AsQueryable();

            Query = query.GroupBy(x => new { x.ProtocolType, x.PalletId })
                .Select(g => new ProtocolReportAggregate
                {
                    LatestStepId = g.OrderByDescending(ps => ps.Id).First().Id,
                    CmoId = g.First().CmoId,
                    CmoName = g.First().Cmo.Name,
                    ProtocolType = g.Key.ProtocolType,
                    StepsSubmitted = g.Select(ps => ps.StepNumber).Distinct().Count(),
                    PossibleProtocolSteps = -1,
                    PalletCode = g.First().Pallet.Code,
                    PackageCodes = g.Select(ps => ps.Package.Code).Distinct().ToList()
                });
        }
    }
}
