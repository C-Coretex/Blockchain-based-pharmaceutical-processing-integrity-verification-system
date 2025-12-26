using Microsoft.EntityFrameworkCore;

namespace PharmaBlockchainBackend.Infrastructure
{
    public interface IRepository<TEntity> 
        where TEntity : class
    {
        public DbSet<TEntity> DbSet { get; }
        public DbContext DbContext { get; }
    }

    public class Repository<TEntity> : IRepository<TEntity> 
        where TEntity : class
    {
        public DbSet<TEntity> DbSet { get; }
        public DbContext DbContext { get; }

        public Repository(PharmaBlockchainBackendDbContext dbContext)
        {
            DbContext = dbContext;
            DbSet = dbContext.Set<TEntity>();
        }
    }
}
