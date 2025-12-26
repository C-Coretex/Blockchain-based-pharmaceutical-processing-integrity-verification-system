namespace PharmaBlockchainBackend.Infrastructure.Entities
{
    public class Pallet
    {
        public Guid Id { get; init; } = Guid.CreateVersion7();
        public Guid Code { get; init; }

        public virtual ICollection<Package> Packages { get; set; } = [];
        public virtual ICollection<ProtocolStep> ProtocolSteps { get; set; } = [];
    }
}
