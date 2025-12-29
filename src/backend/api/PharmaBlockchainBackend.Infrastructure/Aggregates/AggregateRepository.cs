namespace PharmaBlockchainBackend.Infrastructure.Aggregates
{
    public interface IAggregateRepository<TAggregate>
        where TAggregate : class
    {
        public IQueryable<TAggregate> Query { get; }
    }
}
