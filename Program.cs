using AES.Utility.Enums;
using QRay.Utility;

namespace AES
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string payload = "Haho";
            string key = "AlmafaAlmafaAlma"; // MUST BE 16/24/32 characters
            string cipherText = AES.Encode(payload, key, CipherMode.ECB, KeySize.Bits128);
        }
    }
}
