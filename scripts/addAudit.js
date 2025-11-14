import { ethers } from "hardhat";
import fetch from "node-fetch";

const contractAddress = "SOME_CONTRACT_ADDRESS_HERE"; // Replace with actual deployed contract address

const abi = [
  "function addAudit(string batchId, string hashData) public",
  "function assignRole(address user, uint8 role) public"
];

async function main() {
  const [owner, cmo] = await ethers.getSigners();
  const contract = new ethers.Contract(contractAddress, abi, owner);

  // Define CMO
  await contract.assignRole(cmo.address, 1); // Role.CMO

  // CMO adding record
  const cmoContract = contract.connect(cmo);
  const tx = await cmoContract.addAudit("batch001", "hash123abc");
  await tx.wait();

  // Save in PostgreSQL (TEST MODE)
  await fetch("http://localhost:4000/add-audit", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({
      batchId: "batch001",
      hashData: "hash123abc",
      cmoAddress: cmo.address
    })
  });

  console.log("Audit saved in blockchain and DB");
}

main().catch(console.error);
