using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace DBI_PE_Submit_Tool.Common
{
    class Encoder
    {
        public static string Encrypt(string plainText, string publicKey)
        {
            var bytes = Encoding.UTF8.GetBytes(plainText);

            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    var encryptedData = rsa.Encrypt(bytes, true);
                    var base64Encrypted = Convert.ToBase64String(encryptedData);
                    return base64Encrypted;
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }

        public static string Decrypt(string encryptedText, string privateKey)
        {
            // Base64 bytes.
            var bytes = Encoding.UTF8.GetBytes(encryptedText);

            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    //rsa.FromXmlString(privateKey);
                    rsa.ExportParameters(false);

                    var resultBytes = Convert.FromBase64String(encryptedText);
                    var decryptedBytes = rsa.Decrypt(resultBytes, true);
                    var decryptedData = Encoding.UTF8.GetString(decryptedBytes);
                    return decryptedData.ToString();
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }
    }
}
