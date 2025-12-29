using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PharmaBlockchainBackend.Infrastructure.Entities;

namespace PharmaBlockchainBackend.Infrastructure.Configuration
{
    internal class PackageConfiguration : IEntityTypeConfiguration<Package>
    {
        public void Configure(EntityTypeBuilder<Package> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasAlternateKey(x => x.Code);

            builder.HasOne(x => x.Pallet)
                   .WithMany(p => p.Packages)
                   .HasForeignKey(x => x.PalletId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
