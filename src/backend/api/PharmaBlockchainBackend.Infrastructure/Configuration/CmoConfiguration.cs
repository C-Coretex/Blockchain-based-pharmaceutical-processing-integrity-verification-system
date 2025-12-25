using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PharmaBlockchainBackend.Infrastructure.Entities;

namespace PharmaBlockchainBackend.Infrastructure.Configuration
{
    internal class CmoConfiguration : IEntityTypeConfiguration<Cmo>
    {
        public void Configure(EntityTypeBuilder<Cmo> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(250);

            builder.HasData(
                new Cmo
                {
                    Id = Guid.Parse("1d4dba2c-da8e-4518-8bbc-83cd9052e65b"),
                    Name = "MedCore Manufacturing"
                },
                new Cmo
                {
                    Id = Guid.Parse("d525062a-0c4b-4e1a-a5fe-6e1f567530f3"),
                    Name = "HelixPharm Services"
                }
            );
        }
    }
}
