using Microsoft.Extensions.Options;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using PharmaBlockchainBackend.Domain.Blockchain;
using PharmaBlockchainBackend.Providers.Blockchain.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;


public class AuditRegistryHashReader : IBlockchainHashReader
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


    public AuditRegistryHashReader(IOptions<BlockchainOptions> options)
    {
        var opts = options.Value;
        if (string.IsNullOrWhiteSpace(opts.AbiPath))
            throw new InvalidOperationException("Blockchain:AbiPath is not configured");
        if (!File.Exists(opts.AbiPath))
            throw new FileNotFoundException("ABI file not found", opts.AbiPath);

        var chainId = 31337;
        var account = new Account(opts.PrivateKey, chainId);
        _web3 = new Web3(account, opts.RpcUrl);
        _contractAddress = ReadContractAddress(opts.ContractAddressFile);

        _abi = File.ReadAllText(opts.AbiPath);

        if (string.IsNullOrWhiteSpace(_abi))
            throw new InvalidOperationException("ABI file is empty");
    }

    public async Task<IReadOnlyCollection<byte[]>> GetHashesByTimestampAsync(long timestamp, CancellationToken ct)
    {
        var contract = _web3.Eth.GetContract(_abi, _contractAddress);
        var function = contract.GetFunction("getHashesByTimestamp");
        //1767823321
        //1767824738 
        //1767824738
        //1767825560
        //1767979828
        var result = await function.CallAsync<List<byte[]>>(
            (ulong)timestamp
        );

        return result;
    }
}
