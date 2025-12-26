namespace PharmaBlockchainBackend.Infrastructure.Entities
{
    public class Cmo
    {
        public Guid Id { get; init; } = Guid.CreateVersion7();
        public required string Name { get; set; }

        public virtual ICollection<ProtocolStep> ProtocolSteps { get; set; } = [];
    }
}
