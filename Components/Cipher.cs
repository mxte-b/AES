using AES.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AES.Components
{
    public static class Cipher
    {
        public static void Apply(State state, Word[] subKeys, KeyConstants constants)
        {
            // https://nvlpubs.nist.gov/nistpubs/fips/nist.fips.197.pdf
            // Section 5.1 - Cipher
            // Figure 5.
            Primitives.AddRoundKey(state, subKeys[0..constants.Nb]);

            for (int round = 1; round < constants.Nr; round++)
            {
                Primitives.SubBytes(state);
                Primitives.ShiftRows(state);
                if (round != constants.Nr - 1)
                {
                    Primitives.MixColumns(state);
                }

                int start = round * constants.Nb;
                int end = (round + 1) * constants.Nb;
                Primitives.AddRoundKey(state, subKeys[start..end]);
            }
        }
    }
}
