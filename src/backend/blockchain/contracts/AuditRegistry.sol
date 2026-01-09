// SPDX-License-Identifier: MIT
pragma solidity ^0.8.20;

contract AuditRegistry {

    struct Record {
        bytes32 hash;
        uint256 timestamp;
    }

    mapping(uint256 => bytes32[]) private records;

    event HashesRecorded(bytes32[] hashes, uint256 timestamp);

    function recordHashes(bytes32[] calldata hashes)
        external
        returns (uint256 timestamp)
    {
        require(hashes.length > 0, "Empty batch");

        timestamp = block.timestamp;
        require(records[timestamp].length == 0, "Record already exists");

        records[timestamp] = hashes;

        emit HashesRecorded(hashes, timestamp);

        return timestamp;
    }

    function getHashesByTimestamp(uint256 timestamp)
        external
        view
        returns (bytes32[] memory)
    {
        bytes32[] memory result = records[timestamp];
        require(result.length > 0, "No record for this timestamp");
        return result;
    }
}
