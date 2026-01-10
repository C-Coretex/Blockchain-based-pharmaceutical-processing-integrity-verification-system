const { ethers } = require("ethers");

async function main() {
  const provider = new ethers.JsonRpcProvider("http://localhost:8545");

  const contract = "0xf39Fd6e51aad88F6F4ce6aB8827279cffFb92266";
  const timestamp = 1767979828;
  const slot = 0n;

  const base = ethers.keccak256(
    ethers.solidityPacked(
      ["uint256", "uint256"],
      [timestamp, slot]
    )
  );

  const lengthHex = await provider.getStorage(contract, base);
  const length = BigInt(lengthHex);

  console.log("Array length:", length.toString());

  if (length === 0n) return;

  let dataSlot = BigInt(ethers.keccak256(base));

  for (let i = 0n; i < length; i++) {
    const value = await provider.getStorage(
      contract,
      "0x" + (dataSlot + i).toString(16)
    );
    console.log(`hash[${i}]:`, value);
  }
}

main().catch(console.error);
