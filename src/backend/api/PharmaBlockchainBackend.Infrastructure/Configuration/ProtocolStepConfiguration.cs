using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PharmaBlockchainBackend.Infrastructure.Entities;

namespace PharmaBlockchainBackend.Infrastructure.Configuration
{
    internal class ProtocolStepConfiguration : IEntityTypeConfiguration<ProtocolStep>
    {
        public void Configure(EntityTypeBuilder<ProtocolStep> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ProtocolType).IsRequired();
            builder.Property(x => x.StepNumber).IsRequired();
            builder.Property(x => x.Hash).IsRequired();

            builder.Property(x => x.AdditionalData).HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => System.Text.Json.JsonSerializer.Deserialize<object>(v, (System.Text.Json.JsonSerializerOptions?)null));

            builder.HasOne(x => x.Cmo)
                   .WithMany(p => p.ProtocolSteps)
                   .HasForeignKey(x => x.CmoId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Pallet)
                   .WithMany(p => p.ProtocolSteps)
                   .HasForeignKey(x => x.PalletId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Package)
                   .WithMany(p => p.ProtocolSteps)
                   .HasForeignKey(x => x.PackageId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
