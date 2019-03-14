using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using EncryptData;
using System.IO;
using DBI_PE_Submit_Tool.Model;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using Newtonsoft.Json;

namespace DBI_PE_Submit_Tool.Common
{
    public class Coding<T>
    {
        private string Encryption(string strText)
        {
            var publicKey = "<RSAKeyValue>" +
                "<Modulus>" +
                "21wEnTU+mcD2w0Lfo1Gv4rtcSWsQJQTNa6gio05AOkV/Er9w3Y13Ddo5wGtjJ19402S71HUeN0vbKILLJdRSES5MHSdJPSVrOqdrll/vLXxDxWs/U0UT1c8u6k/Ogx9hTtZxYwoeYqdhDblof3E75d9n2F0Zvf6iTb4cI7j6fMs=" +
                "</Modulus>" +
                "<Exponent>AQAB</Exponent>" +
                "</RSAKeyValue>";

            var testData = Encoding.UTF8.GetBytes(strText);

            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    // client encrypting data with public key issued by server                    
                    rsa.FromXmlString(publicKey.ToString());

                    var encryptedData = rsa.Encrypt(testData, true);

                    var base64Encrypted = Convert.ToBase64String(encryptedData);

                    return base64Encrypted;
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }

        private string Decryption(string strText)
        {
            var privateKey = "<RSAKeyValue>" +
                "<Modulus>" +
                "21wEnTU+mcD2w0Lfo1Gv4rtcSWsQJQTNa6gio05AOkV/Er9w3Y13Ddo5wGtjJ19402S71HUeN0vbKILLJdRSES5MHSdJPSVrOqdrll/vLXxDxWs/U0UT1c8u6k/Ogx9hTtZxYwoeYqdhDblof3E75d9n2F0Zvf6iTb4cI7j6fMs=" +
                "</Modulus>" +
                "<Exponent>" +
                "AQAB" +
                "</Exponent>" +
                "<P>/aULPE6jd5IkwtWXmReyMUhmI/nfwfkQSyl7tsg2PKdpcxk4mpPZUdEQhHQLvE84w2DhTyYkPHCtq/mMKE3MHw==" +
                "</P>" +
                "<Q>3WV46X9Arg2l9cxb67KVlNVXyCqc/w+LWt/tbhLJvV2xCF/0rWKPsBJ9MC6cquaqNPxWWEav8RAVbmmGrJt51Q==</Q>" +
                "<DP>8TuZFgBMpBoQcGUoS2goB4st6aVq1FcG0hVgHhUI0GMAfYFNPmbDV3cY2IBt8Oj/uYJYhyhlaj5YTqmGTYbATQ==</DP>" +
                "<DQ>FIoVbZQgrAUYIHWVEYi/187zFd7eMct/Yi7kGBImJStMATrluDAspGkStCWe4zwDDmdam1XzfKnBUzz3AYxrAQ==</DQ>" +
                "<InverseQ>QPU3Tmt8nznSgYZ+5jUo9E0SfjiTu435ihANiHqqjasaUNvOHKumqzuBZ8NRtkUhS6dsOEb8A2ODvy7KswUxyA==</InverseQ>" +
                "<D>cgoRoAUpSVfHMdYXW9nA3dfX75dIamZnwPtFHq80ttagbIe4ToYYCcyUz5NElhiNQSESgS5uCgNWqWXt5PnPu4XmCXx6utco1UVH8HGLahzbAnSy6Cj3iUIQ7Gj+9gQ7PkC434HTtHazmxVgIR5l56ZjoQ8yGNCPZnsdYEmhJWk=</D>" +
                "</RSAKeyValue>";

            var testData = Encoding.UTF8.GetBytes(strText);

            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    var base64Encrypted = strText;

                    // server decrypting data with private key                    
                    rsa.FromXmlString(privateKey);

                    var resultBytes = Convert.FromBase64String(base64Encrypted);
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

        string filePath = "";

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="file"> Path to file we want to save</param>
        public Coding(string file)
        {
            filePath = file;
        }

        public void Save(T t)
        {
            // Convert object to json
            string json = JsonConvert.SerializeObject(t);
            // Spit json by 100
            string[] arrayOfJson = Split(json, 50).ToArray();
            // Encrypt all of it
            List<string> hihi = new List<string>();
            foreach (string item in arrayOfJson)
            {
                hihi.Add(Encryption(item));
            }
            // Write to file
            File.WriteAllLines(filePath, hihi.ToArray());
        }

        public T Load()
        {
            // Read from file by line
            string[] lines = File.ReadAllLines(filePath);
            // Decrypt all of line and append it to json
            string json = "";
            foreach (string item in lines)
            {
                json = json + Decryption(item);
            }
            // Deconvert json to object
            return JsonConvert.DeserializeObject<T>(json);
        }

        private IEnumerable<string> Split(string str, int maxChunkSize)
        {
            for (int i = 0; i < str.Length; i += maxChunkSize)
                yield return str.Substring(i, Math.Min(maxChunkSize, str.Length - i));
        }

        
    }
}
