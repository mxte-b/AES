using AES.Components;
using AES.Utility.Enums;
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
                current = GF256.xtime(current);
            }

            return rcon;
        }
    }
}
