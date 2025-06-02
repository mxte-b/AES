using AES.Utility.Enums;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AES.Components
{
    public class KeyConstants
    {

        // Source: https://nvlpubs.nist.gov/nistpubs/fips/nist.fips.197.pdf
        ///<summary>
        /// Number of columns (32-bit words) comprising the State.
        /// For this standard, Nb = 4. (Also see Sec. 6.3.)
        /// </summary>
        public int Nb { get; private set; } = 4;

        ///<summary>
        /// Number of 32-bit words comprising the Cipher Key. For this
        /// standard, Nk = 4, 6, or 8. (Also see Sec. 6.3.)
        ///</summary>
        public int Nk { get; set; }

        ///<summary>
        /// Number of rounds, which is a function of Nk and Nb (which is fixed).
        /// For this standard, Nr = 10, 12, or 14. (Also see Sec. 6.3.)
        ///</summary>
        public int Nr { get; set; }

        public int TotalWords
        {
            get
            {
                return (Nr + 1) * Nb;
            }
        }

        public KeyConstants(KeySize keySize)
        {
            switch (keySize)
            {
                case KeySize.Bits128:
                    Nk = 4;
                    Nr = 10;
                    break;
                case KeySize.Bits192:
                    Nk = 6;
                    Nr = 12;
                    break;
                case KeySize.Bits256:
                    Nk = 8;
                    Nr = 14;
                    break;
                default:
                    throw new ArgumentException("KeyConstants: Invalid key size");
            }
        }
    }
}
