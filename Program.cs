using AES.Utility;
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

            AES encoder = new AES(CipherMode.CBC, KeySize.Bits128);
            string cipherText = encoder.Encode(payload, key);
        }
    }
}
