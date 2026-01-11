#!/bin/sh
set -e

echo "Starting Hardhat node..."
npx hardhat node --hostname 0.0.0.0 &
HARDHAT_PID=$!

echo "Waiting for Hardhat RPC..."
sleep 10

echo "Deploying contracts..."
npx hardhat run scripts/deploy.js --network localhost

echo "Hardhat running (pid=$HARDHAT_PID)"
wait $HARDHAT_PID
