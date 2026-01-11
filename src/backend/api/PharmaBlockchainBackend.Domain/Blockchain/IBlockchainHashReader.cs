using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PharmaBlockchainBackend.Domain.Blockchain
{
    public interface IBlockchainHashReader
    {
        /// <summary>
        /// Writes one or more hashes to blockchain and returns block timestamp
        /// </summary>
        Task<IReadOnlyCollection<byte[]>> GetHashesByTimestampAsync(
            long timestamp,
            CancellationToken ct);
    }
}
