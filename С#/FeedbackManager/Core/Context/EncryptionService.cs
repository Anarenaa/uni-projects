using System.Security.Cryptography;
using System.Text;

namespace Core.Context
{
    public class EncryptionService
    {
        private readonly byte[] _key;
        private readonly byte[] _iv = new byte[16]; // AES block size is 16 bytes

        public EncryptionService(string inputKey)
        {
            // Convert the string to a byte array. 
            // If there are not 32 bytes here, AES will give an error on the first attempt.
            // 1 symbol = 1 byte in UTF8
            _key = Encoding.UTF8.GetBytes(inputKey);
        }

        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText)) return plainText;

            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;

            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs)) sw.Write(plainText);

            return Convert.ToBase64String(ms.ToArray());
        }

        public string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText)) return cipherText;
            try
            {
                using var aes = Aes.Create();
                aes.Key = _key;
                aes.IV = _iv;

                using var ms = new MemoryStream(Convert.FromBase64String(cipherText));
                using var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
                using var sr = new StreamReader(cs);
                return sr.ReadToEnd();
            }
            catch { return "[Error: Key does not match]"; }
        }
    }
}
