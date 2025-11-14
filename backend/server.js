import http from 'http';
import { initDB, saveAudit } from './queries.js';

await initDB();

const server = http.createServer(async (req, res) => {
  if (req.method === 'POST' && req.url === '/add-audit') {
    let body = '';
    req.on('data', chunk => (body += chunk.toString()));
    req.on('end', async () => {
      const data = JSON.parse(body);
      await saveAudit(data.batchId, data.hashData, data.cmoAddress);
      res.writeHead(200, { 'Content-Type': 'application/json' });
      res.end(JSON.stringify({ status: 'ok' }));
    });
  } else {
    res.writeHead(404);
    res.end('Not found');
  }
});

server.listen(4000, () => console.log('🚀 Server running on http://localhost:4000'));
