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
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            string plain = "Almafácska HAhóóóó 💖🏄‍♂️🏖️🔥👌🤯☀️";
            string key = "Almafafinomalmaa"; // MUST BE 16/24/32 characters
            string iv = "Almafafinomalmaa";

            AES encoder = new AES(CipherMode.CBC, KeySize.Bits128);
            string cipherText = encoder.Encrypt(plain, key, iv);
            Console.WriteLine(cipherText);

            key =StringHelpers.Format(StringHelpers.Decode(key, StringEncoding.UTF8), StringEncoding.Base64);
            string plainText = encoder.Decrypt(cipherText, key);
            Console.WriteLine(plainText);
        }
    }
}
