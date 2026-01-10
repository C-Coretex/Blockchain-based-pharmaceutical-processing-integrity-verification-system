import { ethers } from "ethers";

const provider = new ethers.JsonRpcProvider("http://localhost:8545");

const latest = await provider.getBlockNumber();

for (let i = latest - 10; i <= latest; i++) {
  const block = await provider.getBlock(i);
  console.log(i, block.timestamp);
  console.log(i, block.hash);
}
