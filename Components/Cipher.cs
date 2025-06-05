using Crypto.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Components
{
    public static class Cipher
    {
        /// <summary>
        /// Applies the AES cipher in EBC mode.
        /// </summary>
        public static void Apply(State state, Word[] subKeys, KeyConstants constants)
        {
            // https://nvlpubs.nist.gov/nistpubs/fips/nist.fips.197.pdf
            // Section 5.1 - Cipher
            // Figure 5.
            Primitives.AddRoundKey(state, subKeys[0..constants.Nb]);

            for (int round = 1; round < constants.Nr + 1; round++)
            {
                Primitives.SubBytes(state);
                Primitives.ShiftRows(state);
                if (round != constants.Nr)
                {
                    Primitives.MixColumns(state);
                }

                int start = round * constants.Nb;
                int end = (round + 1) * constants.Nb;
                Primitives.AddRoundKey(state, subKeys[start..end]);
            }
        }

        /// <summary>
        /// Applies the AES cipher in CBC mode using the provided chaining state.
        /// </summary>
        public static void ApplyCBC(State state, Word[] subKeys, State chain, KeyConstants constants)
        {
            // XOR-ing with the current CBC chain value
            Primitives.XOR(state, chain);

            Apply(state, subKeys, constants);
        }

        /// <summary>
        /// Applies the inverse AES cipher in EBC mode.
        /// </summary>
        public static void ApplyInverse(State state, Word[] subKeys, KeyConstants constants)
        {
            // https://nvlpubs.nist.gov/nistpubs/fips/nist.fips.197.pdf
            // Section 5.1 - Cipher
            // Figure 5.
            Primitives.AddRoundKey(state, subKeys[(constants.Nr * constants.Nb)..((constants.Nr + 1) * constants.Nb)]);

            for (int round = constants.Nr - 1; round >= 0; round--)
            {
                Primitives.InvShiftRows(state);
                Primitives.InvSubBytes(state);

                int start = round * constants.Nb;
                int end = (round + 1) * constants.Nb;
                Primitives.AddRoundKey(state, subKeys[start..end]);

                if (round > 0)
                {
                    Primitives.InvMixColumns(state);
                }
            }
        }

        /// <summary>
        /// Applies the inverse AES cipher in CBC mode using the provided chaining state.
        /// </summary>
        public static void ApplyInverseCBC(State state, Word[] subKeys, State chain, KeyConstants constants)
        {
            ApplyInverse(state, subKeys, constants);

            // XOR-ing with the current CBC chain value
            Primitives.XOR(state, chain);
        }
    }
}
