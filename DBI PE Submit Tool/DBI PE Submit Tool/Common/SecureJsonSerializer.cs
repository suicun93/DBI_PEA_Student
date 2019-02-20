﻿using System;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.IO;

namespace DBI_PE_Submit_Tool.Common
{
    public class SecureJsonSerializer<T>
    where T : class
    {
        private readonly string filePath;

        /// <summary>
        /// Keep this inputkey very safe and prevent someone from decoding it some way!!
        /// </summary>
        private const string Inputkey = "560A18CD-6346-4CF0-A2E8-671F9B6B9EA9";

        private readonly ICryptoTransform encryptor;

        private readonly ICryptoTransform decryptor;

        // Password Salt
        private const string salt = "02341235XadfaDADFexA8932F7Dz3J3X";

        private static readonly byte[] saltByte = Encoding.ASCII.GetBytes(salt);

        /// <summary>
        /// Key to generate rmCrypto
        /// </summary>
        private static Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(Inputkey, (new SHA512Managed()).ComputeHash(saltByte));

        public SecureJsonSerializer(string filePath)
        {
            this.filePath = filePath;
            var rmCrypto = GetAlgorithm();
            this.encryptor = rmCrypto.CreateEncryptor();
            this.decryptor = rmCrypto.CreateDecryptor();
        }

        private static RijndaelManaged GetAlgorithm()
        {
            var algorithm = new RijndaelManaged();
            int bytesForKey = algorithm.KeySize / 8;
            int bytesForIV = algorithm.BlockSize / 8;
            algorithm.Key = key.GetBytes(bytesForKey);
            algorithm.IV = key.GetBytes(bytesForIV);
            algorithm.Padding = PaddingMode.Zeros;
            return algorithm;
        }
       
        public void Save(T obj)
        {

            using (var writer = new StreamWriter(new CryptoStream(File.Create(this.filePath), this.encryptor, CryptoStreamMode.Write)))
            {
                writer.Write(JsonConvert.SerializeObject(obj));
                writer.Close();
            }
        }

        public T Load()
        {
            using (var reader = new StreamReader(new CryptoStream(File.OpenRead(this.filePath), this.decryptor, CryptoStreamMode.Read)))
            {
                string json = reader.ReadToEnd();
                reader.Close();
                return JsonConvert.DeserializeObject<T>(json);
            }
        }
    }
}
