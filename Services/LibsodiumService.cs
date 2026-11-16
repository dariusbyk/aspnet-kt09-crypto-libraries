using System.Text;
using Sodium;
using CryptoLibraries.Models;

namespace CryptoLibraries.Services
{
    public class LibsodiumService
    {
        public EncryptionResult Process(string text)
        {
            SodiumCore.Init();
            byte[] key = SecretBox.GenerateKey();
            byte[] nonce = SecretBox.GenerateNonce();

            byte[] encrypted = Encrypt(text, key, nonce);

            EncryptionResult result = new EncryptionResult();
            result.EncryptedText = Convert.ToBase64String(encrypted);
            result.DecryptedText = Decrypt(encrypted, key, nonce);
            return result;
        }

        public byte[] Encrypt(string text, byte[] key, byte[] nonce)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            return SecretBox.Create(data, nonce, key);
        }

        public string Decrypt(byte[] encrypted, byte[] key, byte[] nonce)
        {
            byte[] decrypted = SecretBox.Open(encrypted, nonce, key);
            return Encoding.UTF8.GetString(decrypted);
        }
    }
}
