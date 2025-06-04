using Crypto.Utility.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Utility
{
    public class KeyValidator
    {
        public static bool IsValid(string key, KeySize size, StringEncoding encoding)
        {
            try
            {
                byte[] keyBytes = encoding switch
                {
                    StringEncoding.UTF8 => Encoding.UTF8.GetBytes(key),
                    StringEncoding.Base64 => Convert.FromBase64String(key),
                    _ => throw new ArgumentException("Invalid encoding encountered")
                };

                return keyBytes?.Length * 8 == (int)size;
            }
            catch
            {
                return false;
            }
        }
    }
}
