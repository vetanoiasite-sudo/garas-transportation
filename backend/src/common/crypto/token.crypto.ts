import * as crypto from 'crypto';

// Session-token crypto — replicates the old scheme (UserToken = encrypt(sessionId)),
// but uses a configurable secret + AES-256-CBC (deterministic IV so tokens are
// stable, as in the old backend). The secret comes from env, not hardcoded.
const secret = process.env.TOKEN_SECRET ?? 'garas-dev-secret-change-me-please-32b';
const key = crypto.createHash('sha256').update(secret).digest(); // 32 bytes
const iv = crypto.createHash('md5').update(secret).digest(); // 16 bytes

export function encryptToken(plain: string): string {
  const cipher = crypto.createCipheriv('aes-256-cbc', key, iv);
  return cipher.update(plain, 'utf8', 'base64') + cipher.final('base64');
}

export function decryptToken(token: string): string {
  const decipher = crypto.createDecipheriv('aes-256-cbc', key, iv);
  return decipher.update(token, 'base64', 'utf8') + decipher.final('utf8');
}
