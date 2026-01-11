using System.Collections.Generic;

namespace PharmaBlockchainBackend.Domain.Enums
{
    public enum ProtocolType
    {
        None = 0,
        Doxazosin = 1,
        Irbesartan = 2
    }

    public static class ProtocolTypeHelpers
    {
        public static readonly IReadOnlyDictionary<ProtocolType, int> ProtocolTypeMaxSteps = new Dictionary<ProtocolType, int>
        {
            { ProtocolType.None, 0 },
            { ProtocolType.Doxazosin, 4 },
            { ProtocolType.Irbesartan, 4 },
        };
    }
}
