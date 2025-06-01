using AES.Utility.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AES.Utility
{
    public class KeyValidator
    {
        public static bool IsValid(string key, KeySize size)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            return encoding.GetByteCount(key) * 8 == (int)size;
        }
    }
}
