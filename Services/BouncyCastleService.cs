using System.Text;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using CryptoLibraries.Models;

namespace CryptoLibraries.Services
{
    public class BouncyCastleService
    {
        public EncryptionResult Process(string text)
        {
            SecureRandom random = new SecureRandom();
            byte[] key = new byte[32];
            byte[] nonce = new byte[12];
            random.NextBytes(key);
            random.NextBytes(nonce);

            byte[] encrypted = Encrypt(text, key, nonce);

            EncryptionResult result = new EncryptionResult();
            result.EncryptedText = Convert.ToBase64String(encrypted);
            result.DecryptedText = Decrypt(encrypted, key, nonce);
            return result;
        }

        public byte[] Encrypt(string text, byte[] key, byte[] nonce)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            GcmBlockCipher cipher = new GcmBlockCipher(new AesEngine());
            cipher.Init(true, new AeadParameters(new KeyParameter(key), 128, nonce));

            byte[] encrypted = new byte[cipher.GetOutputSize(data.Length)];
            int length = cipher.ProcessBytes(data, 0, data.Length, encrypted, 0);
            length += cipher.DoFinal(encrypted, length);
            Array.Resize(ref encrypted, length);
            return encrypted;
        }

        public string Decrypt(byte[] encrypted, byte[] key, byte[] nonce)
        {
            GcmBlockCipher cipher = new GcmBlockCipher(new AesEngine());
            cipher.Init(false, new AeadParameters(new KeyParameter(key), 128, nonce));

            byte[] decrypted = new byte[cipher.GetOutputSize(encrypted.Length)];
            int length = cipher.ProcessBytes(encrypted, 0, encrypted.Length, decrypted, 0);
            length += cipher.DoFinal(decrypted, length);
            return Encoding.UTF8.GetString(decrypted, 0, length);
        }
    }
}
