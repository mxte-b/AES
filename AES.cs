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

            // Creating the state array, and padding the plaintext to be a multiple of 128 bits
            ApplyPadding(plaintextBytes);


            // Key Schedule
            // AES generates 128 bit keys for all versions
            Word[] subKeys = KeyExpander.Schedule(keyBytes, Constants);

            // Main encoding loop
            

            return "";
        }

        public string Decode(string payload, string key, string iv)
        {
            return "";
        }

        // Returns an array of bytes from UTF-8 string
        private byte[] FormatUTF8(string payload)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            return encoder.GetBytes(payload);
        }

        // Returns the UTF-8 encoded string from an array of bytes
        private string DecodeUTF8(byte[] data)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            return encoder.GetString(data);
        }

        // Applies PKCS#7 padding to the data
        private byte[] ApplyPadding(byte[] data)
        {
            return data;
        }

        public AES(CipherMode cipherMode, KeySize keySize)
        {
            CipherMode = cipherMode;
            KeySize = keySize;
            Constants = new KeyConstants(KeySize);
        }
    }
}
