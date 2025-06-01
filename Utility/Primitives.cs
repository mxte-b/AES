using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AES.Utility
{
    public static class Primitives
    {
        private static ResourceDatabase database = ResourceDatabase.Instance;
        public static byte[] RotWord(byte[] word)
        {
            return [ word[1], word[2], word[3], word[0] ];
        }

        public static byte[] SubWord(byte[] word)
        {
            return database.SBox.Substitute(word);
        }
    }
}
