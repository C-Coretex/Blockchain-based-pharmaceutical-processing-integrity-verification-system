namespace PharmaBlockchainBackend.Domain.Blockchain;

public interface IBlockchainHashWriter
{
    /// <summary>
    /// Writes one or more hashes to blockchain and returns block timestamp
    /// </summary>
    Task<DateTime?> RecordHashesAsync(
        IReadOnlyCollection<byte[]> hashes,
        CancellationToken ct);
}
