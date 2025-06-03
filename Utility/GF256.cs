using AES.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AES.Utility
{
    public static class GF256
    {
        public static byte xtime(byte polynomial)
        {
            byte shifted = (byte)(polynomial << 1);
            bool mostSignificantBitSet = (polynomial & 0x80) != 0;
            return (byte)( mostSignificantBitSet ? shifted ^ 0x1B : shifted);
        }

        public static byte Multiply(byte left, byte right)
        {
            byte result = 0x00;

            // https://nvlpubs.nist.gov/nistpubs/fips/nist.fips.197.pdf
            // Section 4.2.1 - Multiplication by x in GF(2^8)
            // Multiplication by higher powers of x can be implemented by repeated application of xtime().
            // By adding intermediate results, multiplication by any constant can be implemented. 
            for (int i = 0; i < 8; i++)
            {
                // For every bit in the multiplier, if that bit is set,
                // we calculate its value using xtime(), and finally XOR
                // with the result.
                if ((right & (1 << i)) != 0)
                {
                    byte multiple = left;
                    for (int j = 0; j < i; j++)
                    {
                        multiple = xtime(multiple);
                    }
                    result ^= multiple;
                }
            }

            return result;
        }

        // Matrix multiplication by a vector in GF(2^8)
        public static Word MultiplyMatrix(byte[,] matrix, Word word)
        {
            Word result = new Word();

            for (int i = 0; i < word.Length; i++)
            {
                byte sum = 0x00;

                for (int j = 0; j < word.Length; j++)
                {
                    sum ^= Multiply(matrix[i, j], word[j]);
                }

                result[i] = sum;
            }

            return result;
        }
    }
}
