namespace PharmaBlockchainBackend.Providers.Blockchain.Options;

public class BlockchainOptions
{
    public string RpcUrl { get; set; } = null!;
    public string PrivateKey { get; set; } = null!;
    public string ContractAddress { get; set; } = null!;
}
