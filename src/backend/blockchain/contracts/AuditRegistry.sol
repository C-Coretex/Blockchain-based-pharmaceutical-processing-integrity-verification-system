// SPDX-License-Identifier: MIT
pragma solidity ^0.8.20;

contract AuditRegistry {

    // timestamp => hashes
    mapping(uint256 => bytes32[]) private records;

    event HashesRecorded(
        uint256 indexed timestamp,
        bytes32[] hashes
    );

    function recordHashes(bytes32[] calldata hashes)
        external
        returns (uint256 timestamp)
    {
        require(hashes.length > 0, "Empty batch");

        timestamp = block.timestamp;

        for (uint256 i = 0; i < hashes.length; i++) {
            records[timestamp].push(hashes[i]);
        }

        emit HashesRecorded(timestamp, hashes);

        return timestamp;
    }

    function getHashesByTimestamp(uint256 timestamp)
        external
        view
        returns (bytes32[] memory)
    {
        return records[timestamp];
    }
}
