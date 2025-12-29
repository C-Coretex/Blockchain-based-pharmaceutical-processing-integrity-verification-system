using PharmaBlockchainBackend.Infrastructure.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace PharmaBlockchainBackend.Api.Features.ProtocolActions.List
{
    public class Handler(IAggregateRepository<ProtocolReportAggregate> protocolReportAggregate)
    {
        public async Task<IAsyncEnumerable<Response>> Handle(Request request)
        {
            var query = protocolReportAggregate.Query;

            if (request.CmoId.HasValue)
                query = query.Where(ps => ps.CmoId == request.CmoId.Value);
            if (request.ProtocolType.HasValue)
                query = query.Where(ps => ps.ProtocolType == request.ProtocolType.Value);
            if (request.PalletCode.HasValue)
                query = query.Where(ps => ps.PalletCode == request.PalletCode.Value);
            if (request.PackageCode.HasValue)
                query = query.Where(ps => ps.PackageCodes.Contains(request.PackageCode.Value));

            return query
                .OrderByDescending(p => p.LatestStepId)
                .Skip(request.Skip)
                .Take(request.Top)
                .AsAsyncEnumerable()
                .Select(ps => ps.PopulateInMemory())
                .Select(Response.Create);
        }
    }
}
