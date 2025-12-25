using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using PharmaBlockchainBackend.Infrastructure.Entities;

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

            return services;
        }
    }
}
