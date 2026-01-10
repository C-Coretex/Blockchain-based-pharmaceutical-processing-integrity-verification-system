using System.Collections.Generic;

namespace PharmaBlockchainBackend.Domain.Enums
{
    public enum ProtocolType
    {
        None = 0,
    }

    public static class ProtocolTypeHelpers
    {
        public static readonly IReadOnlyDictionary<ProtocolType, int> ProtocolTypeMaxSteps = new Dictionary<ProtocolType, int>
        {
            { ProtocolType.None, 3 },
        };
    }
}
