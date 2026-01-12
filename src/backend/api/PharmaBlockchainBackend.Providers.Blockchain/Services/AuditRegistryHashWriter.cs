using Microsoft.Extensions.Options;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using PharmaBlockchainBackend.Domain.Blockchain;
using PharmaBlockchainBackend.Providers.Blockchain.Options;
using PharmaBLockchainBackendApi.Blockchain.Functions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static PharmaBlockchainBackend.Providers.Blockchain.Dto.BlockchainDto;

public class AuditRegistryHashWriter : IBlockchainHashWriter
{
    private readonly Web3 _web3;
    private readonly string _contractAddress;
    private readonly string _abi;

    private static string ReadContractAddress(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"Contract address file not found: {path}");

        var address = File.ReadAllText(path).Trim();

        if (!address.StartsWith("0x") || address.Length != 42)
            throw new InvalidOperationException($"Invalid contract address: {address}");

        return address;
    }

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
        _contractAddress = ReadContractAddress(opts.ContractAddressFile); ;

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

        var hashes32 = hashes
           .Select(h =>
           {
               if (h == null || h.Length != 32)
                   throw new Exception($"Invalid hash length: {h?.Length}");
               return h;
           })
           .ToArray();

        object[] input = new object[]{hashes.Select(h => (object)h).ToArray()};

        var gas = new HexBigInteger(3_000_000);
        var contract = _web3.Eth.GetContract(_abi, _web3.TransactionManager.Account.Address);
        var function = contract.GetFunction("recordHashes");
        var contractS = _web3.TransactionManager.Account.Address;
        var handler = _web3.Eth.GetContractTransactionHandler<RecordHashesFunction>();
        
        var tx = new RecordHashesFunction
        {
            FromAddress = _web3.TransactionManager.Account.Address,
            Gas = 3_000_000,
            Hashes = hashes.ToList()
        };

        var receipt = await handler.SendRequestAndWaitForReceiptAsync(
            _contractAddress,
            tx); 

        var evt = receipt
        .DecodeAllEvents<HashesRecordedEventDTO>()
        .Single();

        var timestamp = evt.Event.Timestamp;

        return DateTimeOffset
            .FromUnixTimeSeconds((long)timestamp)
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
