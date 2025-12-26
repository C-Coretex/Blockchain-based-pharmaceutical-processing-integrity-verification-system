
import pkg from "hardhat";
const { ethers } = pkg;


async function main() {
  // get deployer account
  const [deployer] = await ethers.getSigners();
  console.log("Testing with account:", deployer.address);

  // Put here contract address 
  const CONTRACT_ADDRESS = "CONTRACT_ADRESS" //TODO maybe migrate to secrets;

  const Contract = await ethers.getContractAt(
    "AuditRegistry",
    CONTRACT_ADDRESS
  );

  // create test hash
  const hash = ethers.keccak256(
    ethers.toUtf8Bytes("BATCH_123_PHARMA")
  );

  console.log("Writing hash:", hash);

// save hash
  const tx = await Contract.recordHash(hash);
  const receipt = await tx.wait();

  const blockNumber = receipt.blockNumber;
  const block = await ethers.provider.getBlock(blockNumber);
  const timestamp = block.timestamp;

  console.log("Stored in block:", blockNumber);
  console.log("Timestamp:", timestamp);

  // Try to get hash by timestamp
  const storedHash = await Contract.getHashByTimestamp(timestamp);
  console.log("Read hash:", storedHash);
}

main().catch((error) => {
  console.error(error);
  process.exit(1);
});
