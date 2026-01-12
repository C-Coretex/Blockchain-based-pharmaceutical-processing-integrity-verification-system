using Nethereum.ABI.FunctionEncoding.Attributes;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace PharmaBlockchainBackend.Providers.Blockchain.Dto
{
    internal class BlockchainDto
    {
        [Event("HashesRecorded")]
        public class HashesRecordedEventDTO : IEventDTO
        {
            [Parameter("uint256", "timestamp", 1, true)]
            public BigInteger Timestamp { get; set; }

            [Parameter("bytes32[]", "hashes", 2, false)]
            public List<byte[]>? Hashes { get; set; }
        }
    }
}
