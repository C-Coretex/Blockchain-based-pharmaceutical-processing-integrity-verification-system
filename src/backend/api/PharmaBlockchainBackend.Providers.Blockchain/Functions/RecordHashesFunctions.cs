using System.Collections.Generic;
using Nethereum.Contracts;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace PharmaBLockchainBackendApi.Blockchain.Functions
{
    [Function("recordHashes", "uint256")]
    public class RecordHashesFunction : FunctionMessage
    {
        [Parameter("bytes32[]", "hashes", 1)]
        public List<byte[]> Hashes { get; set; } = new List<byte[]> ();
    }
}
