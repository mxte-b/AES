using Crypto.Components;
using Crypto.Utility;
using Crypto.Utility.Enums;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CipherMode = Crypto.Utility.Enums.CipherMode;

namespace Crypto
{
    public class AES
    {
        public CipherMode CipherMode { get; set; }
        public KeySize KeySize { get; set; }
        public KeyConstants Constants { get; set; }
        public string Encrypt(string plaintext, string key, string? iv = null, StringEncoding encoding = StringEncoding.UTF8)
        {
            byte[] plaintextBytes = StringHelpers.Decode(plaintext, encoding);
            byte[] keyBytes = StringHelpers.Decode(key, encoding);
            byte[]? ivBytes = iv == null ? null : StringHelpers.Decode(iv, encoding);

            if (!KeyValidator.IsValid(key, KeySize, encoding))
            {
                throw new ArgumentException($"The length of the key must exactly be {(int)KeySize} bits");
            }

            AESEngine engine = new AESEngine(CipherMode, Constants);
            byte[] cipherBytes = engine.Encrypt(plaintextBytes, keyBytes, ivBytes);

            return StringHelpers.Format(cipherBytes, StringEncoding.Base64);
        }

        public string Encrypt(byte[] plaintext, byte[] key, byte[]? iv = null)
        {
            AESEngine engine = new AESEngine(CipherMode, Constants);
            byte[] cipherBytes = engine.Encrypt(plaintext, key, iv);

            return StringHelpers.Format(cipherBytes, StringEncoding.Base64);
        }

        public string Decrypt(string payload, string key, string iv)
        {
            return "";
        }

        public AES(CipherMode cipherMode, KeySize keySize)
        {
            CipherMode = cipherMode;
            KeySize = keySize;
            Constants = new KeyConstants(KeySize);
        }
    }
}
