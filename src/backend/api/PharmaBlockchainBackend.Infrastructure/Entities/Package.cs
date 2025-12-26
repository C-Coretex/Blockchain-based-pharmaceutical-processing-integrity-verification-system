namespace PharmaBlockchainBackend.Infrastructure.Entities
{
    public class Package
    {
        public Guid Id { get; init; } = Guid.CreateVersion7();
        public Guid Code { get; init; }

        public Guid PalletId { get; init; }
        public virtual Pallet Pallet { get; set; } = null!;

        public virtual ICollection<ProtocolStep> ProtocolSteps { get; set; } = [];
    }
}
