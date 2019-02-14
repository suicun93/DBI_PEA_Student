using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using EncryptData;

namespace DBI_PE_Submit_Tool.Common
{
    class Coding
    {
        //private static readonly string key = "12345678";

        public static string Encode(string json)
        {
            //var bytes = System.Text.Encoding.UTF8.GetBytes(json);
            //string encrypt = EncryptData.EncryptSupport.Encryption(bytes, key);
            //return encrypt;
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(json);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Decode(string code)
        {
            //var bytes = System.Text.Encoding.UTF8.GetBytes(code);

            //string decrypt = EncryptData.EncryptSupport.Decryption(bytes, key);

            //return decrypt;
            var base64EncodedBytes = System.Convert.FromBase64String(code);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
