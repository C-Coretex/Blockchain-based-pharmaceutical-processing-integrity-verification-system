namespace PharmaBlockchainBackend.Providers.Blockchain.Options
{
    public class BlockchainOptions
    {
        public string RpcUrl { get; set; } = null;
        public string PrivateKey { get; set; } = null;
        public string ContractAddress { get; set; } = null;
        public string AbiPath { get; set; } = null;
        public string ContractAddressFile { get; set; } = null!;
    }
}
