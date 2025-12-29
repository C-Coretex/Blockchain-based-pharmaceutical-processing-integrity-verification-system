using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using PharmaBlockchainBackend.Infrastructure.Entities;
using PharmaBlockchainBackend.Infrastructure.Aggregates;

namespace PharmaBlockchainBackend.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<PharmaBlockchainBackendDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<IRepository<Cmo>, Repository<Cmo>>();
            services.AddScoped<IRepository<Package>, Repository<Package>>();
            services.AddScoped<IRepository<Pallet>, Repository<Pallet>>();
            services.AddScoped<IRepository<ProtocolStep>, Repository<ProtocolStep>>();

            services.AddScoped<IAggregateRepository<ProtocolReportAggregate>, ProtocolReportAggregateRepository>();

            return services;
        }

        private static IAggregateRepository<TAggregate> GetAggregateDI<TAggregate> (this IServiceProvider serviceProvider, Func<PharmaBlockchainBackendDbContext, IAggregateRepository<TAggregate>> selector)
            where TAggregate : class
        {
            var dbContext = serviceProvider.GetRequiredService<PharmaBlockchainBackendDbContext>();
            return selector(dbContext);
        }
}
}
