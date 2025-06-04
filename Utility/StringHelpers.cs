using Crypto.Utility.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Utility
{
    public static class StringHelpers
    {
        public static byte[] Decode(string str, StringEncoding encoding)
        {
            return encoding switch
            {
                StringEncoding.UTF8 => DecodeUTF8(str),
                StringEncoding.Base64 => DecodeBase64(str),
                _ => throw new ArgumentException("Unknown string encoding"),
            };
        }

        public static string Format(byte[] data, StringEncoding encoding)
        {
            return encoding switch
            {
                StringEncoding.UTF8 => FormatUTF8(data),
                StringEncoding.Base64 => FormatBase64(data),
                _ => throw new ArgumentException("Unknown string encoding"),
            };
        }

        // Returns an array of bytes from UTF-8 string
        private static byte[] DecodeUTF8(string payload)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            return encoder.GetBytes(payload);
        }

        // Returns the UTF-8 encoded string from an array of bytes
        private static string FormatUTF8(byte[] data)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            return encoder.GetString(data);
        }

        private static string FormatBase64(byte[] data) => Convert.ToBase64String(data);

        private static byte[] DecodeBase64(string text) => Convert.FromBase64String(text);
    }
}
