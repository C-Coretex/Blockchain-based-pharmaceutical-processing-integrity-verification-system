using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using PharmaBlockchainBackend.Domain.Blockchain;
using PharmaBlockchainBackend.Providers.Blockchain.Options;
using Microsoft.Extensions.Options;

public class AuditRegistryHashWriter : IBlockchainHashWriter
{
    private readonly Web3 _web3;
    private readonly string _contractAddress;

    //TEST ABI FOR TESTS. 
    // WILL BE REPLACED WITH REAL ABI FROM FILE AFTER DOCKER COMPILE
    public static class AuditRegistryAbi
    {
        public const string Json = """
    [
      {
        "inputs":[{"internalType":"bytes32","name":"hash","type":"bytes32"}],
        "name":"recordHash",
        "outputs":[],
        "stateMutability":"nonpayable",
        "type":"function"
      },
      {
        "inputs":[{"internalType":"uint256","name":"timestamp","type":"uint256"}],
        "name":"getHashByTimestamp",
        "outputs":[{"internalType":"bytes32","name":"","type":"bytes32"}],
        "stateMutability":"view",
        "type":"function"
      }
    ]
    """;
    }


    public AuditRegistryHashWriter(IOptions<BlockchainOptions> options)
    {
        var opts = options.Value;

        var account = new Account(opts.PrivateKey);
        _web3 = new Web3(account, opts.RpcUrl);
        _contractAddress = opts.ContractAddress;
    }

    public async Task<DateTime?> RecordHashesAsync(IReadOnlyCollection<byte[]> hashes,CancellationToken ct)
    {
        // Пока simplest variant: один hash → одна транзакция
        // Потом можно заменить на batch-контракт

        long? lastTimestamp = null;

        foreach (var hash in hashes)
        {
            var receipt = await RecordSingleAsync(hash);
            lastTimestamp = receipt;
        }

        if (!lastTimestamp.HasValue)
            return null;

        return DateTimeOffset
         .FromUnixTimeSeconds(lastTimestamp.Value)
         .UtcDateTime;
    }

    private async Task<long> RecordSingleAsync(byte[] hash)
    {
        var contract = _web3.Eth.GetContract(AuditRegistryAbi.Json, _contractAddress);
        var function = contract.GetFunction("recordHash");

        var receipt = await function.SendTransactionAndWaitForReceiptAsync(
            from: _web3.TransactionManager.Account.Address,
            gas: null,
            value: null,
            functionInput: hash);

        var block = await _web3.Eth.Blocks
            .GetBlockWithTransactionsByNumber
            .SendRequestAsync(receipt.BlockNumber);

        return (long)block.Timestamp.Value;
    }
}
