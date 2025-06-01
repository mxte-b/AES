using AES.Utility;
using AES.Utility.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AES
{
    public static class AES
    {
        public static string Encode(string plaintext, string key, CipherMode mode, KeySize keySize)
        {
            byte[] plaintextBytes = FormatUTF8(plaintext);
            byte[] keyBytes = FormatUTF8(key);

            if (!KeyValidator.IsValid(key, keySize))
            {
                throw new ArgumentException($"The length of the key must exactly be {(int)keySize} bits");
            }

            // Key Schedule


            return "";
        }

        public static string Decode(string payload, string key, string iv, CipherMode mode, KeySize keySize)
        {
            return "";
        }

        private static byte[] FormatUTF8(string payload)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            return encoder.GetBytes(payload);
        }
    }
}
