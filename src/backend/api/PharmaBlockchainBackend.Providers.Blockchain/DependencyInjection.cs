using Microsoft.Extensions.DependencyInjection;
using PharmaBlockchainBackend.Domain.Blockchain;

namespace PharmaBlockchainBackend.Providers.Blockchain
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBlockchain(this IServiceCollection services)
        {
            services.AddScoped<IBlockchainHashWriter, AuditRegistryHashWriter>();
            services.AddScoped<IBlockchainHashReader, AuditRegistryHashReader>();
            return services;
        }
    }
}
