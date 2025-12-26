// SPDX-License-Identifier: MIT
pragma solidity ^0.8.20;

contract AuditRegistry {

    struct Record {
        bytes32 hash;
        uint256 timestamp;
    }

    // timestamp => hash
    mapping(uint256 => bytes32) private records;

    event HashRecorded(bytes32 hash, uint256 timestamp);

    /**
     * @notice Store a hash in the blockchain and return block timestamp
     * @param dataHash Hash of off-chain data
     * @return timestamp Block timestamp when hash was recorded
     */
    function recordHash(bytes32 dataHash) external returns (uint256 timestamp) {
        timestamp = block.timestamp;

        require(records[timestamp] == bytes32(0), "Record already exists");

        records[timestamp] = dataHash;

        emit HashRecorded(dataHash, timestamp);

        return timestamp;
    }

    /**
     * @notice Get stored hash by timestamp
     * @param timestamp Block timestamp
     * @return dataHash Hash stored at that timestamp
     */
    function getHashByTimestamp(uint256 timestamp)
        external
        view
        returns (bytes32 dataHash)
    {
        dataHash = records[timestamp];
        require(dataHash != bytes32(0), "No record for this timestamp");
        return dataHash;
    }
}
