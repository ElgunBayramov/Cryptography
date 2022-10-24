using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace CryptoExample
{
    internal class Program
    {
        static void Main(string[] args)
        {


            //---------------HASHING ALGORITM------------------
            string text = "1";
            var buffer = Encoding.UTF8.GetBytes(text);
            var provider = MD5.Create();
            //var provider = SHA1.Create();
            byte[] hashBuffer = provider.ComputeHash(buffer);

            StringBuilder builder = new StringBuilder();
            foreach (var item in hashBuffer)
            {
                builder.Append($"{item:X2}");
            }
            Console.WriteLine(text);
            Console.WriteLine(builder.ToString());
            //--------------SYMMETRIC ALGORITM----------------
            string data = "1";
            string key = "test-data-academy";

            while (Console.ReadKey().Key == ConsoleKey.Enter)
            {
                Console.WriteLine("--------------------------------------");
                Console.Write("data:");
                data = Console.ReadLine();
                string chiperText = Encrypt(data,key);
                Console.WriteLine(chiperText);
                Console.WriteLine(Decrypt(chiperText, key));
                Console.WriteLine("*********************");
            }

        }
        //ENCRYPT
        public static string Encrypt(string value, string key)   //123
        {
            try
            {
                using (var provider = new TripleDESCryptoServiceProvider())
                using (var md5 = new MD5CryptoServiceProvider())
                {
                    var keyBuffer = md5.ComputeHash(Encoding.UTF8.GetBytes($"#{key}!2022"));
                    var ivBuffer = md5.ComputeHash(Encoding.UTF8.GetBytes($"2022@{key}$"));

                    var transform = provider.CreateEncryptor(keyBuffer, ivBuffer);

                    using (var ms = new MemoryStream())
                    using (var cs = new CryptoStream(ms, transform, CryptoStreamMode.Write))
                    {
                        var valueBuffer = Encoding.UTF8.GetBytes(value);

                        cs.Write(valueBuffer, 0, valueBuffer.Length);
                        cs.FlushFinalBlock();

                        ms.Position = 0;
                        var result = new byte[ms.Length];

                        ms.Read(result, 0, result.Length);

                        return Convert.ToBase64String(result);
                    }
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        //DECRYPT
            public static string Decrypt(string value, string key)   //123
            {
                try
                {
                    using (var provider = new TripleDESCryptoServiceProvider())
                    using (var md5 = new MD5CryptoServiceProvider())
                    {
                        var keyBuffer = md5.ComputeHash(Encoding.UTF8.GetBytes($"#{key}!2022"));
                        var ivBuffer = md5.ComputeHash(Encoding.UTF8.GetBytes($"2022@{key}$"));

                        var transform = provider.CreateDecryptor(keyBuffer, ivBuffer);

                        using (var ms = new MemoryStream())
                        using (var cs = new CryptoStream(ms, transform, CryptoStreamMode.Write))
                        {
                            var valueBuffer = Convert.FromBase64String(value);

                            cs.Write(valueBuffer, 0, valueBuffer.Length);
                            cs.FlushFinalBlock();

                            ms.Position = 0;
                            var result = new byte[ms.Length];

                            ms.Read(result, 0, result.Length);

                            return Encoding.UTF8.GetString(result);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return "";
                }
            }

    }
}
