using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace GarasTransport.Infrastructure.Security;

// Session-token crypto — byte-for-byte compatible with the NestJS backend:
//   key = sha256(secret) (32 bytes), iv = md5(secret) (16 bytes),
//   AES-256-CBC + PKCS7, base64 output. The secret comes from config, not
//   hardcoded. UserToken = encrypt(sessionId).
public class TokenCrypto
{
    private readonly byte[] _key;
    private readonly byte[] _iv;

    public TokenCrypto(IConfiguration config)
    {
        var secret = config["TokenSecret"]
                     ?? Environment.GetEnvironmentVariable("TOKEN_SECRET")
                     ?? "garas-dev-secret-change-me-please-32b";
        _key = SHA256.HashData(Encoding.UTF8.GetBytes(secret));       // 32 bytes
        _iv = MD5.HashData(Encoding.UTF8.GetBytes(secret));           // 16 bytes
    }

    public string Encrypt(string plain)
    {
        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        using var enc = aes.CreateEncryptor();
        var bytes = Encoding.UTF8.GetBytes(plain);
        var cipher = enc.TransformFinalBlock(bytes, 0, bytes.Length);
        return Convert.ToBase64String(cipher);
    }

    public string Decrypt(string token)
    {
        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        using var dec = aes.CreateDecryptor();
        var bytes = Convert.FromBase64String(token);
        var plain = dec.TransformFinalBlock(bytes, 0, bytes.Length);
        return Encoding.UTF8.GetString(plain);
    }

    /* ---- Tenant-bound session tokens (database-per-company) ----
       Session ids are per-database identity ints, so id alone is ambiguous
       across tenants (demo session #5 ≠ alpha session #5). The token therefore
       carries "sessionId|company" — opaque to clients, verified server-side. */

    /// <summary>Encrypt a session token bound to its company.</summary>
    public string EncryptSession(int sessionId, string companyName) =>
        Encrypt($"{sessionId}|{companyName.ToLowerInvariant()}");

    /// <summary>
    /// Decrypt + parse a session token. Returns false for malformed tokens.
    /// `company` is empty for legacy id-only tokens (issued before tenancy).
    /// </summary>
    public bool TryDecryptSession(string token, out int sessionId, out string company)
    {
        sessionId = 0;
        company = string.Empty;
        try
        {
            var plain = Decrypt(token);
            var sep = plain.IndexOf('|');
            var idPart = sep < 0 ? plain : plain[..sep];
            if (sep >= 0) company = plain[(sep + 1)..];
            return int.TryParse(idPart, out sessionId);
        }
        catch
        {
            return false;
        }
    }
}
