using PharmaBlockchainBackend.Domain.Enums;

namespace PharmaBlockchainBackend.Infrastructure.Entities
{
    public class ProtocolStep
    {
        public Guid Id { get; init; } = Guid.CreateVersion7();


        public Guid CmoId { get; set; }
        public virtual Cmo Cmo { get; set; } = null!;

        public ProtocolType ProtocolType { get; set; }
        public int StepNumber { get; set; }

        public Guid PalletId { get; set; }
        public virtual Pallet Pallet { get; set; } = null!;

        public Guid PackageId { get; set; }
        public virtual Package Package { get; set; } = null!;

        public required byte[] Hash { get; set; }
        public DateTime Timestamp { get; set; }

        public object? AdditionalData { get; set; }
    }
}
