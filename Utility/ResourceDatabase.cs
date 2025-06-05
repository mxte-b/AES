using Crypto.Components;
using Crypto.Utility.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Utility
{
    public class ResourceDatabase
    {
        private static ResourceDatabase? _instance = null;
        private static readonly object _instanceLock = new object();

        public SBox SBox { get; }
        public SBox InvSBox { get; }
        public Word[] RoundConstants { get; }
        public byte[,] MixColumnsMatrix { get; }
        public byte[,] InvMixColumnsMatrix { get; }

        private ResourceDatabase()
        {
            // Substitution box
            byte[,] sboxData = new byte[16, 16];
            int y = 0;
            foreach (string line in ResourceLoader.LoadEmbedded("Crypto.Resources.SBox.csv"))
            {
                string[] values = line.Split(',');

                for (int x = 0; x < 16; x++)
                {
                    sboxData[y, x] = Convert.ToByte(values[x]);
                }

                y++;
            }
            SBox = new SBox(sboxData);

            // Inverse Substitution box
            byte[,] invSBoxData = new byte[16, 16];
            y = 0;
            foreach (string line in ResourceLoader.LoadEmbedded("Crypto.Resources.InvSBox.csv"))
            {
                string[] values = line.Split(',');

                for (int x = 0; x < 16; x++)
                {
                    invSBoxData[y, x] = Convert.ToByte(values[x]);
                }

                y++;
            }

            InvSBox = new SBox(invSBoxData);

            // Round constants
            RoundConstants = Primitives.GenerateRoundConstants();

            // MixColumns Matrix ( a(x) )
            MixColumnsMatrix = new byte[4, 4]
            {
                { 0x02, 0x03, 0x01, 0x01 },
                { 0x01, 0x02, 0x03, 0x01 },
                { 0x01, 0x01, 0x02, 0x03 },
                { 0x03, 0x01, 0x01, 0x02 }
            };

            // InvMixColumns Matrix ( a(x) )
            InvMixColumnsMatrix = new byte[4, 4]
            {
                { 0x0e, 0x0b, 0x0d, 0x09 },
                { 0x09, 0x0e, 0x0b, 0x0d },
                { 0x0d, 0x09, 0x0e, 0x0b },
                { 0x0b, 0x0d, 0x09, 0x0e }
            };
        }

        public static ResourceDatabase Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    _instance ??= new ResourceDatabase();
                    return _instance;
                }
            }
        }
    }
}
