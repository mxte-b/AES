using Crypto.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Utility
{
    public class SBox
    {
        private byte[,] buffer;

        public byte Lookup(byte value)
        {
            return buffer[(value & 0xf0) >> 4, value & 0x0f];
        }

        public Word Substitute(Word word)
        {
            return new Word(word.GetBuffer().Select(Lookup).ToArray());
        }

        public SBox(byte[,] data)
        {
            buffer = data;
        }
    }
}
