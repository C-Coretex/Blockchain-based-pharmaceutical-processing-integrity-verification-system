using Microsoft.Extensions.Options;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using PharmaBlockchainBackend.Domain.Blockchain;
using PharmaBlockchainBackend.Providers.Blockchain.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PharmaBLockchainBackendApi.Blockchain.Functions;

public class AuditRegistryHashWriter : IBlockchainHashWriter
{
    private readonly Web3 _web3;
    private readonly string _contractAddress;
    private readonly string _abi;

    public AuditRegistryHashWriter(IOptions<BlockchainOptions> options)
    {
        var opts = options.Value;
        if (string.IsNullOrWhiteSpace(opts.AbiPath))
            throw new InvalidOperationException("Blockchain:AbiPath is not configured");
        if (!File.Exists(opts.AbiPath))
            throw new FileNotFoundException("ABI file not found", opts.AbiPath);

        var chainId = 31337;
        var account = new Account(opts.PrivateKey, chainId);
        _web3 = new Web3(account, opts.RpcUrl);
        _contractAddress = _web3.TransactionManager.Account.Address;

        _abi = File.ReadAllText(opts.AbiPath);

        if (string.IsNullOrWhiteSpace(_abi))
            throw new InvalidOperationException("ABI file is empty");
    }

    public async Task<DateTime?> RecordHashesAsync(
        IReadOnlyCollection<byte[]> hashes,
        CancellationToken ct)
    {
        if (hashes.Count == 0)
            return null;
        // var abi = await GetAbiAsync(ct);

        foreach (var h in hashes)
        {
            if (h == null)
                throw new Exception("Hash is null");

            if (h.Length != 32)
                throw new Exception($"Invalid hash length: {h.Length}");
        }
        var hashes32 = hashes
           .Select(h =>
           {
               if (h == null || h.Length != 32)
                   throw new Exception($"Invalid hash length: {h?.Length}");
               return h;
           })
           .ToArray();



        object[] input = new object[]
    {
        hashes.Select(h => (object)h).ToArray()
    };
        var gas = new HexBigInteger(3_000_000);
        var contract = _web3.Eth.GetContract(_abi, _web3.TransactionManager.Account.Address);
        var function = contract.GetFunction("recordHashes");

        // bytes32[] — это byte[][]
        //var input = hashes.Select(h => h).ToArray();
        Console.WriteLine(_web3.TransactionManager.Account.Address);
        var contractS = _web3.TransactionManager.Account.Address;

        //var receipt = await function.SendTransactionAndWaitForReceiptAsync(
        //    from: _web3.TransactionManager.Account.Address,
        //    gas: new HexBigInteger(1_000_000),
        //    value: null,
        //    functionInput: input);

        var handler = _web3.Eth.GetContractTransactionHandler<RecordHashesFunction>();
        
        var tx = new RecordHashesFunction
        {
            FromAddress = _web3.TransactionManager.Account.Address,
            Gas = 3_000_000,
            Hashes = hashes.ToList()
        };

        var receipt = await handler.SendRequestAndWaitForReceiptAsync(
            contractS,
            tx
        ); 


        //-------READY -------
        Console.WriteLine($"TX block: {receipt.BlockNumber.Value}");
        var block = await _web3.Eth.Blocks
            .GetBlockWithTransactionsByNumber
            .SendRequestAsync(receipt.BlockNumber);

        Console.WriteLine($"Stored timestamp: {block.Timestamp.Value}");

        return DateTimeOffset
            .FromUnixTimeSeconds((long)block.Timestamp.Value)
            .UtcDateTime;
    }

    public async Task<IReadOnlyCollection<byte[]>> GetHashesByTimestampAsync(
    long timestamp,
    CancellationToken ct)
    {
        var contract = _web3.Eth.GetContract(_abi, _contractAddress);
        var function = contract.GetFunction("getHashesByTimestamp");

        var result = await function.CallAsync<List<byte[]>>(
            (ulong)timestamp
        );

        return result;
    }


    private async Task<long> RecordSingleAsync(byte[] hash)
    {
        var contract = _web3.Eth.GetContract(_abi, _contractAddress);
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
