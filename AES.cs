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
        public CipherMode CipherMode { get; }
        public KeySize KeySize { get; }
        public KeyConstants Constants { get; }

        public string Encrypt(byte[] plaintext, byte[] key, byte[]? iv = null)
        {
            AESEngine engine = new AESEngine(CipherMode, Constants);

            if (CipherMode == CipherMode.CBC)
            {
                iv ??= Primitives.RandomIVBytes();

                byte[] cipherBytes = engine.Encrypt(plaintext, key, iv);
                byte[] finalMessage = new byte[iv.Length + cipherBytes.Length];

                // Prepend the IV before the ciphertext
                PrependIV(finalMessage, cipherBytes, iv);

                return StringHelpers.Format(finalMessage, StringEncoding.Base64);
            }
            else
            {
                byte[] cipherBytes = engine.Encrypt(plaintext, key, null);

                return StringHelpers.Format(cipherBytes, StringEncoding.Base64);
            }
        }

        public string Encrypt(string plaintext, string key, string? iv = null, StringEncoding encoding = StringEncoding.UTF8)
        {
            byte[] plaintextBytes = StringHelpers.Decode(plaintext, encoding);
            byte[] keyBytes = StringHelpers.Decode(key, encoding);

            if (iv != null && iv.Length != 16)
            {
                throw new ArgumentException("IV must be 16 bytes long.");
            }

            byte[]? ivBytes = iv == null ? null : StringHelpers.Decode(iv, encoding);

            if (!KeyValidator.IsValid(key, KeySize, encoding))
            {
                throw new ArgumentException($"The length of the key must exactly be {(int)KeySize} bits");
            }

            return Encrypt(plaintextBytes, keyBytes, ivBytes);
        }
        public string Decrypt(byte[] ciphertext, byte[] key)
        {
            byte[]? iv = null;
            byte[] cipherBytes;
            
            if (CipherMode == CipherMode.CBC)
            {
                int ivBytes = Constants.Nb * 4;
                ExtractIVAndCipher(ciphertext, out iv, out cipherBytes);
            }
            else
            {
                cipherBytes = (byte[])ciphertext.Clone();
            }
            AESEngine engine = new AESEngine(CipherMode, Constants);
            byte[] plaintextBytes = engine.Decrypt(cipherBytes, key, iv);

            return StringHelpers.Format(plaintextBytes, StringEncoding.UTF8);
        }

        public string Decrypt(string ciphertext, string key, StringEncoding encoding = StringEncoding.Base64)
        {
            byte[] ciphertextBytes = StringHelpers.Decode(ciphertext, encoding);
            byte[] keyBytes = StringHelpers.Decode(key, encoding);

            if (!KeyValidator.IsValid(key, KeySize, encoding))
            {
                throw new ArgumentException($"The length of the key must exactly be {(int)KeySize} bits");
            }

            return Decrypt(ciphertextBytes, keyBytes);
        }

        public void PrependIV(byte[] dest, byte[] ciphertext, byte[] iv)
        {
            Buffer.BlockCopy(iv, 0, dest, 0, iv.Length);
            Buffer.BlockCopy(ciphertext, 0, dest, iv.Length, ciphertext.Length);
        }

        public void ExtractIVAndCipher(byte[] src, out byte[] iv, out byte[] cipher)
        {
            int ivBytes = Constants.Nb * 4;

            iv = new byte[ivBytes];
            cipher = new byte[src.Length - ivBytes];

            Buffer.BlockCopy(src, 0, iv, 0, ivBytes);
            Buffer.BlockCopy(src, ivBytes, cipher, 0, cipher.Length);
        }

        public AES(CipherMode cipherMode, KeySize keySize)
        {
            CipherMode = cipherMode;
            KeySize = keySize;
            Constants = new KeyConstants(KeySize);
        }
    }
}
