using Crypto.Components;
using Crypto.Utility.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Utility
{
    public static class Primitives
    {
        private static ResourceDatabase database = ResourceDatabase.Instance;

        public static byte[] RandomIVBytes()
        {
            byte[] buffer = new byte[16];
            using var r = RandomNumberGenerator.Create();
            r.GetBytes(buffer);
            return buffer;
        }

        // --- Word Primitives ---
        public static Word RotWord(Word word)
        {
            return new Word(word[1], word[2], word[3], word[0]);
        }

        public static Word SubWord(Word word)
        {
            return database.SBox.Substitute(word);
        }

        public static Word RCon(int round)
        {
            return database.RoundConstants[round - 1];
        }

        public static Word[] GenerateRoundConstants()
        {
            Word[] rcon = new Word[10];
            byte current = 0x01;

            for (int i = 1; i < 10 + 1; i++)
            {
                rcon[i - 1] = new Word(current, 0, 0, 0);
                current = GF256.XTime(current);
            }

            return rcon;
        }
        // --- END Word Primitives ---

        // --- State Primitives ---

        public static State RandomIV()
        {
            return new State(RandomIVBytes());
        }

        public static void AddRoundKey(State state, Word[] roundKey)
        {
            if (roundKey.Length != 4)
            {
                throw new ArgumentException("The round key must be exactly 4 words long");
            }

            for (int i = 0; i < 4; i++)
            {
                state.SetColumn(i, state.GetColumn(i) ^ roundKey[i]);
            }
        }

        /// <summary>
        /// Performs byte-wise XOR operation on "a", using the bytes from "b"
        /// </summary>
        public static void XOR(State a, State b)
        {
            for (int i = 0; i < 4; i++)
            {
                a.SetColumn(i, a.GetColumn(i) ^ b.GetColumn(i));
            }
        }

        public static void SubBytes(State state)
        {
            for (int i = 0; i < 4; i++)
            {
                Word sub = database.SBox.Substitute(state.GetColumn(i));
                state.SetColumn(i, sub);
            }
        }

        public static void ShiftRows(State state)
        {
            byte[,] temp = state.GetBuffer();

            for (int row = 0; row < 4; row++)
            {
                int shift = row;
                for (int col = 0; col < 4; col++)
                {
                    state[row, col] = temp[row, (col + shift) % 4];
                }
            }
        }

        public static void MixColumns(State state)
        {
            for (int col = 0; col < 4; col++)
            {
                state.SetColumn(col, GF256.MultiplyMatrix(database.MixColumnsMatrix, state.GetColumn(col)));
            }
        }

        public static void InvSubBytes(State state)
        {
            for (int i = 0; i < 4; i++)
            {
                Word sub = database.InvSBox.Substitute(state.GetColumn(i));
                state.SetColumn(i, sub);
            }
        }

        public static void InvShiftRows(State state)
        {
            byte[,] temp = state.GetBuffer();

            for (int row = 0; row < 4; row++)
            {
                int shift = 4 - row;
                for (int col = 0; col < 4; col++)
                {
                    state[row, col] = temp[row, (col + shift) % 4];
                }
            }
        }

        public static void InvMixColumns(State state)
        {
            for (int col = 0; col < 4; col++)
            {
                state.SetColumn(col, GF256.MultiplyMatrix(database.InvMixColumnsMatrix, state.GetColumn(col)));
            }
        }
        // --- END State Primitives ---
    }
}
