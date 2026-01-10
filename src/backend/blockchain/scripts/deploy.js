const hre = require("hardhat");
const fs = require("fs");

async function main() {
  console.log("DEPLOY START");

  const AuditRegistry = await hre.ethers.getContractFactory("AuditRegistry");
  const contract = await AuditRegistry.deploy({
    gasLimit: 6_000_000
  });

  await contract.waitForDeployment();

  const address = contract.target;

  console.log("Contract deployed to:", address);

  

  fs.mkdirSync("/app/shared/blockchain", { recursive: true });

  fs.writeFileSync(
    "/app/shared/blockchain/AuditRegistry.abi.json",
    JSON.stringify(contract.interface.fragments, null, 2)
  );

  fs.writeFileSync(
    "/app/shared/blockchain/AuditRegistry.address",
    address
  );

  console.log("DEPLOY FINISHED");
}

main().catch((e) => {
  console.error(e);
  process.exit(1);
});
