// SPDX-License-Identifier: MIT
pragma solidity ^0.8.20;
contract AuditRegistry {
    address public owner;

    enum Role { None, MAH, CMO, Auditor }

    struct AuditRecord {
        string batchId;        // Batch id
        string hashData;       // Hash
        address cmo;           // cmo
        uint256 timestamp;     // timestamp
    }
    }
