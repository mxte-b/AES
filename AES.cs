using AES.Components;
using AES.Utility;
using AES.Utility.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AES
{
    public class AES
    {
        public CipherMode CipherMode { get; set; }
        public KeySize KeySize { get; set; }
        public KeyConstants Constants { get; set; }
        public string Encode(string plaintext, string key)
        {
            byte[] plaintextBytes = FormatUTF8(plaintext);
            byte[] keyBytes = FormatUTF8(key);

            if (!KeyValidator.IsValid(key, KeySize))
            {
                throw new ArgumentException($"The length of the key must exactly be {(int)KeySize} bits");
            }

            // Key Schedule
            // AES generates 128 bit keys for all versions
            Word[] subKeys = new Word[(Constants.Nr + 1) * Constants.Nb];

            return "";
        }

        public string Decode(string payload, string key, string iv)
        {
            return "";
        }

        private byte[] FormatUTF8(string payload)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            return encoder.GetBytes(payload);
        }

        public AES(CipherMode cipherMode, KeySize keySize)
        {
            CipherMode = cipherMode;
            KeySize = keySize;
            Constants = new KeyConstants(KeySize);
        }
    }
}
