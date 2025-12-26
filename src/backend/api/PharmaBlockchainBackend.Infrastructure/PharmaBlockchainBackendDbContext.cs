using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using PharmaBlockchainBackend.Infrastructure.Entities;

namespace PharmaBlockchainBackend.Infrastructure
{
    public class PharmaBlockchainBackendDbContext(DbContextOptions<PharmaBlockchainBackendDbContext> options) : DbContext(options)
    {
        #region DbSets

        internal DbSet<Cmo> Cmo { get; set; }
        internal DbSet<Pallet> Pallet { get; set; }
        internal DbSet<Package> Package { get; set; }
        internal DbSet<ProtocolStep> ProtocolStep { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }

        public static void ApplyMigrations(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PharmaBlockchainBackendDbContext>().UseNpgsql(connectionString);
            var db = new PharmaBlockchainBackendDbContext(optionsBuilder.Options).Database;
            var pendingMigrations = db.GetPendingMigrations();

            if (pendingMigrations.Any())
                db.Migrate();
        }
    }

    public class MigrationsDbContextFactory : IDesignTimeDbContextFactory<PharmaBlockchainBackendDbContext>
    {
        public PharmaBlockchainBackendDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PharmaBlockchainBackendDbContext>().UseNpgsql();
            return new PharmaBlockchainBackendDbContext(optionsBuilder.Options);
        }
    }

}
