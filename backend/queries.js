import { pool } from './db.js';

export async function initDB() {
  await pool.query(`
    CREATE TABLE IF NOT EXISTS audits (
      id SERIAL PRIMARY KEY,
      batch_id VARCHAR(100) UNIQUE NOT NULL,
      hash_data TEXT NOT NULL,
      cmo_address TEXT NOT NULL,
      created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
    );
  `);
  console.log("✅ PostgreSQL table ready");
}

export async function saveAudit(batchId, hashData, cmoAddress) {
  await pool.query(
    'INSERT INTO audits (batch_id, hash_data, cmo_address) VALUES ($1, $2, $3) ON CONFLICT DO NOTHING',
    [batchId, hashData, cmoAddress]
  );
  console.log(`💾 Audit saved: ${batchId}`);
}
