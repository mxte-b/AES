using Crypto.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Components
{
    public static class KeyExpander
    {
        public static Word[] Schedule(byte[] key, KeyConstants constants)
        {
            int i = 0;
            Word[] result = new Word[constants.TotalWords];

            // Pasting the first Nk bytes into the result
            for (; i < constants.Nk; i++)
            {
                result[i] = new Word(key[4 * i], key[4 * i + 1], key[4 * i + 2], key[4 * i + 3]);
            }

            Word temp;

            // Performing the key expansion
            for (; i < constants.TotalWords; i++)
            {
                temp = result[i - 1];
                if (i % constants.Nk == 0)
                {
                    Word rcon = Primitives.RCon(i / constants.Nk);
                    temp = Primitives.SubWord(Primitives.RotWord(temp)) ^ rcon;
                }
                else if (constants.Nk > 6 && i % constants.Nk == 4)
                {
                    temp = Primitives.SubWord(temp);
                }
                result[i] = result[i - constants.Nk] ^ temp;
            }

            return result;
        }
    }
}
