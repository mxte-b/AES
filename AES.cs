using Crypto.Components;
using Crypto.Utility;
using Crypto.Utility.Enums;
using System;
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
        public string Encode(string plaintext, string key)
        {
            byte[] plaintextBytes = FormatUTF8(plaintext);
            byte[] keyBytes = FormatUTF8(key);

            if (!KeyValidator.IsValid(key, KeySize))
            {
                throw new ArgumentException($"The length of the key must exactly be {(int)KeySize} bits");
            }

            // Creating the state array, and padding the plaintext to be a multiple of 128 bits
            byte[] plaintextPadded = ApplyPadding(plaintextBytes);
            List<State> states = GetStates(plaintextPadded);

            Console.WriteLine(String.Join("\n\n", states));

            // Key Schedule
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
            byte[] padded;
            int remainderBytes = 16 - (data.Length % 16);

            int newLength = data.Length + remainderBytes;

            padded = new byte[data.Length + remainderBytes];
            Array.Copy(data, padded, data.Length);

            for (int i = 0; i < remainderBytes; i++)
            {
                padded[data.Length + i] = (byte)remainderBytes;
            }

            return padded;
        }

        // Removes PKCS#7 padding from the data
        private byte[] RemovePadding(byte[] data)
        {
            int pad = data[^1];

            if (pad > 16 || pad == 0)
            {
                throw new CryptographicException("Invalid padding value encountered.");
            }

            for (int i = data.Length - pad; i < data.Length; i++)
            {
                if (data[i] != pad)
                {
                    throw new CryptographicException("Padding values are not uniform.");
                }
            }
            
            byte[] raw = new byte[data.Length - pad];
            Array.Copy(data, raw, raw.Length);
            return raw;
        }

        // Formats the plaintext into a state array
        private List<State> GetStates(byte[] data)
        {
            if (data.Length % 16 != 0)
            {
                throw new ArgumentException("The length of the plaintext must be a multiple of 16");
            }

            List<State> states = new List<State>();
            for (int i = 0; i < data.Length / 16; i++)
            {
                int start = i * 16;
                int end = (i + 1) * 16;

                states.Add(new State(data[start..end]));
            }

            return states;
        }

        public AES(CipherMode cipherMode, KeySize keySize)
        {
            CipherMode = cipherMode;
            KeySize = keySize;
            Constants = new KeyConstants(KeySize);
        }
    }
}
