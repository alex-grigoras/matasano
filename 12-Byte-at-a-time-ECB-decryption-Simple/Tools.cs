using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace sandbox2
{
    public class ByteArrayComparer : IEqualityComparer<byte[]>
    {
        public bool Equals(byte[] x, byte[] y)
        {
            for (var i = 0; i < x.Length; i++)
            {
                if (x[i] != y[i])
                {
                    return false;
                }
            }
            return true;
        }

        //this is from StackOverflow http://stackoverflow.com/questions/3329574/removing-duplicate-bytes-from-a-collection
        public int GetHashCode(byte[] obj)
        {
            int result = 13 * obj.Length;
            for (int i = 0; i < obj.Length; i++)
            {
                result = (17 * result) + obj[i];
            }
            return result;
        }
    }

    public class RandomKeyAESECB
    {
        private static string _key = "0123456789123456";

        public static byte[] Encrypt(byte[] inputBytes)
        {
            var aes = Aes.Create();
            aes.Key = Encoding.ASCII.GetBytes(_key);
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            aes.BlockSize = 128;

            var encryptor = aes.CreateEncryptor();

            var ms = new MemoryStream();
            var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            cs.Write(inputBytes, 0, inputBytes.Length);
            cs.Close();
            ms.Close();

            return ms.ToArray();
        }
    }

    public class Tools
    {
        public static List<byte[]> GetBlocks(int blocksFromInput, byte[] input)
        {
            var prefix = new List<byte>();
            for (var i = 0; i < 16 - 1 - blocksFromInput; i++)
            {
                prefix.Add(41);
            }
            prefix.AddRange(input.Take(blocksFromInput));

            var blocks = new List<byte[]>();
            for (int i = 0; i <= byte.MaxValue; i++)
            {
                var block = prefix.Concat(new [] {(byte)i}).ToArray();
                blocks.Add(block);
            }
            return blocks;
        }

        public static byte[] DecryptFirstAESECBBlock(byte[] inputBytes)
        {
            var dict = new Dictionary<byte[], byte>(new ByteArrayComparer());
            var decrypted = new List<byte>();
            for (var i = 1; i <= Math.Min(16, inputBytes.Count()); i++)
            {
                dict.Clear();
                var blocks = GetBlocks(i-1, inputBytes);
                foreach (var block in blocks)
                {
                    var encryptedBlock = RandomKeyAESECB.Encrypt(block);
                    dict[encryptedBlock] = block.Last();
                }

                foreach (var block in blocks.Select(b => b.Take(b.Length-i)))
                {
                    var toEncrypt = block.Concat(inputBytes.Take(i)).ToArray();
                    var encryptedBlock = RandomKeyAESECB.Encrypt(toEncrypt);
                    if (dict.ContainsKey(encryptedBlock))
                    {
                        decrypted.Add(dict[encryptedBlock]);
                        break;
                    }
                }
            }
            return decrypted.ToArray();
        }
    }
}
