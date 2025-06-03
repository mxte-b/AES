using AES.Components;
using AES.Utility.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AES.Utility
{
    public class ResourceDatabase
    {
        private static ResourceDatabase? _instance = null;
        private static readonly object _instanceLock = new object();

        public SBox SBox { get; private set; }
        public Word[] RoundConstants { get; private set; }
        public byte[,] MixColumnMatrix { get; private set; }

        private ResourceDatabase()
        {
            // Substitution box
            byte[,] sboxData = new byte[16, 16];
            int y = 0;
            foreach (string line in ResourceLoader.LoadEmbedded("AES.Resources.SBox.csv"))
            {
                string[] values = line.Split(',');

                for (int x = 0; x < 16; x++)
                {
                    sboxData[y, x] = Convert.ToByte(values[x]);
                }

                y++;
            }
            SBox = new SBox(sboxData);

            // Round constants
            RoundConstants = Primitives.GenerateRoundConstants();

            // MixColumns Matrix ( a(x) )
            MixColumnMatrix = new byte[4, 4]
            {
                { 0x02, 0x03, 0x01, 0x01 },
                { 0x01, 0x02, 0x03, 0x01 },
                { 0x01, 0x01, 0x02, 0x03 },
                { 0x03, 0x01, 0x01, 0x02 }
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
