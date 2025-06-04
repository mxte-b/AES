using Crypto.Utility;
using Crypto.Utility.Enums;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CipherMode = Crypto.Utility.Enums.CipherMode;

namespace Crypto.Components
{
    public class AESEngine
    {
        private CipherMode cipherMode;
        private KeyConstants keyConstants;

        public byte[] Encrypt(byte[] plaintext, byte[] key, byte[]? iv = null)
        {
            // Creating the state array, and padding the plaintext to be a multiple of 128 bits
            byte[] plaintextPadded = ApplyPadding(plaintext);
            List<State> states = GetStates(plaintextPadded);

            // Key Schedule
            Word[] subKeys = KeyExpander.Schedule(key, keyConstants);

            // Main encoding loop
            switch (cipherMode)
            {
                case CipherMode.ECB:

                    foreach (State state in states)
                    {
                        Cipher.Apply(state, subKeys, keyConstants);
                    }

                    break;
                case CipherMode.CBC:

                    State chainValue = iv == null ? Primitives.RandomIV() : new State(iv);

                    foreach (State state in states)
                    {
                        Cipher.ApplyCBC(state, subKeys, chainValue, keyConstants);
                        chainValue = state.Clone();
                    }

                    break;
                default:
                    throw new CryptographicException("Unknown cipher mode encountered");
            }

            return GetBytesFromStates(states);
        }

        // Applies PKCS#7 padding to the data
        private byte[] ApplyPadding(byte[] data)
        {
            byte[] padded;
            int remainderBytes = 16 - (data.Length % 16);

            int newLength = data.Length + remainderBytes;

            padded = new byte[data.Length + remainderBytes];
            Array.Copy(data, padded, data.Length);

            for (int i = 0; i < remainderBytes; i++)
            {
                padded[data.Length + i] = (byte)remainderBytes;
            }

            return padded;
        }

        // Removes PKCS#7 padding from the data
        private byte[] RemovePadding(byte[] data)
        {
            int pad = data[^1];

            if (pad > 16 || pad == 0)
            {
                throw new CryptographicException("Invalid padding value encountered.");
            }

            for (int i = data.Length - pad; i < data.Length; i++)
            {
                if (data[i] != pad)
                {
                    throw new CryptographicException("Padding values are not uniform.");
                }
            }

            byte[] raw = new byte[data.Length - pad];
            Array.Copy(data, raw, raw.Length);
            return raw;
        }

        // Formats the plaintext into a state array
        private List<State> GetStates(byte[] data)
        {
            if (data.Length % 16 != 0)
            {
                throw new ArgumentException("The length of the plaintext must be a multiple of 16");
            }

            List<State> states = new List<State>();
            for (int i = 0; i < data.Length / 16; i++)
            {
                int start = i * 16;
                int end = (i + 1) * 16;

                states.Add(new State(data[start..end]));
            }

            return states;
        }

        private byte[] GetBytesFromStates(List<State> states)
        {
            byte[] bytes = new byte[states.Count * 16];

            for (int i = 0; i < states.Count; i++)
            {
                Array.Copy(states[i].ToArray(), 0, bytes, i * 16, 16);
            }

            return bytes;
        }

        public AESEngine(CipherMode cipherMode, KeyConstants keyConstants)
        {
            this.cipherMode = cipherMode;
            this.keyConstants = keyConstants;
        }
    }
}
