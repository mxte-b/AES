using Crypto.Utility;
using Crypto.Utility.Enums;
using QRay.Utility;
using System.Text;

namespace Crypto
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string plain = "Almafácskaasdasdasdasdadasdasd";
            string key = "AlmafafinomalmaaAlmafafinomalmaa"; // MUST BE 16/24/32 characters
            string iv = "Almafafinomalmaa";

            AES encoder = new AES(CipherMode.CBC, KeySize.Bits256);
            string cipherText = encoder.Encrypt(plain, key, iv);
            Console.WriteLine(cipherText);
        }
    }
}
