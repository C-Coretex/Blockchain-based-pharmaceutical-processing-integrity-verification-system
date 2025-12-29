using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PharmaBlockchainBackend.Infrastructure.Entities;

namespace PharmaBlockchainBackend.Infrastructure.Configuration
{
    internal class PalletConfiguration : IEntityTypeConfiguration<Pallet>
    {
        public void Configure(EntityTypeBuilder<Pallet> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasAlternateKey(x => x.Code);
        }
    }
}
