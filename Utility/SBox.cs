using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AES.Utility
{
    public class SBox
    {
        private byte[,] buffer;

        public byte Lookup(byte value)
        {
            return buffer[(value & 0xf0) >> 4, value & 0x0f];
        }

        public byte[] Substitute(byte[] word)
        {
            return word.Select(x => Lookup(x)).ToArray();
        }

        public SBox(byte[,] data)
        {
            buffer = data;
        }
    }
}
