// SPDX-License-Identifier: MIT
pragma solidity ^0.8.20;

/// @title AuditRegistry — блокчейн-аудит производственных условий медикаментов
/// @author ...
/// @notice Хранит хеши аудитов, добавленные CMO, с контролем доступа MAH
contract AuditRegistry {
    address public owner;

    enum Role { None, MAH, CMO, Auditor }

    struct AuditRecord {
        string batchId;        // Batch id
        string hashData;       // Hash
        address cmo;           // cmo
        uint256 timestamp;     // timestamp
    }

    mapping(address => Role) public roles;
    mapping(string => AuditRecord) public audits;

    event RoleAssigned(address indexed user, Role role);
    event AuditAdded(string batchId, string hashData, address indexed cmo, uint256 timestamp);
    event AuditVerified(string batchId, bool valid, address indexed auditor, uint256 timestamp);

    modifier onlyOwner() {
        require(msg.sender == owner, "Only owner (MAH) can call this");
        _;
    }

    modifier onlyCMO() {
        require(roles[msg.sender] == Role.CMO, "Only CMO can call this");
        _;
    }

    modifier onlyAuditor() {
        require(roles[msg.sender] == Role.Auditor, "Only Auditor can call this");
        _;
    }

    constructor() {
        owner = msg.sender;
        roles[msg.sender] = Role.MAH;
    }

    /// @notice Назначить роль пользователю
    function assignRole(address user, Role role) external onlyOwner {
        require(user != address(0), "Invalid address");
        roles[user] = role;
        emit RoleAssigned(user, role);
    }

    /// @notice CMO добавляет запись аудита
    function addAudit(string calldata batchId, string calldata hashData) external onlyCMO {
        require(bytes(audits[batchId].batchId).length == 0, "Batch already exists");
        audits[batchId] = AuditRecord({
            batchId: batchId,
            hashData: hashData,
            cmo: msg.sender,
            timestamp: block.timestamp
        });
        emit AuditAdded(batchId, hashData, msg.sender, block.timestamp);
    }

    /// @notice Аудитор сверяет хеш
    function verifyAuditHash(string calldata batchId, string calldata hashToCheck)
        external
        onlyAuditor
        returns (bool)
    {
        AuditRecord memory record = audits[batchId];
        require(bytes(record.batchId).length > 0, "Batch not found");

        bool valid = keccak256(abi.encodePacked(record.hashData)) ==
                     keccak256(abi.encodePacked(hashToCheck));

        emit AuditVerified(batchId, valid, msg.sender, block.timestamp);
        return valid;
    }

    /// @notice Get info about batch
    function getAudit(string calldata batchId) external view returns (AuditRecord memory) {
        return audits[batchId];
    }
}
