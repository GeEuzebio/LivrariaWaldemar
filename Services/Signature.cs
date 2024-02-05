using System.Security.Cryptography;
using System.Text;

namespace LibraryApp.Services
{
    public class Signature
    {
        public Signature() { }
        public static (string publicKey, string privateKey) GenerateKeyPair()
        {
            using (var rsa = new RSACryptoServiceProvider(1024))
            {
                string publicKey = rsa.ToXmlString(false);
                string privateKey = rsa.ToXmlString(true);
                return (publicKey, privateKey);
            }
        }

        public static string SignData(string data, string privateKey)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(privateKey);

                byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                byte[] signatureBytes = rsa.SignData(dataBytes, new SHA256CryptoServiceProvider());

                return Convert.ToBase64String(signatureBytes);
            }
        }

        public static bool VerifySignature(string data, string signature, string publicKey)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(publicKey);

                byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                byte[] signatureBytes = Convert.FromBase64String(signature);

                return rsa.VerifyData(dataBytes, new SHA256CryptoServiceProvider(), signatureBytes);
            }
        }
    }
}
