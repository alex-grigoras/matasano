using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace sandbox2
{
    
    public class Program
    {



        public static void Main(string[] args)
        {
            var secret =
                Convert.FromBase64String(
                    "Um9sbGluJyBpbiBteSA1LjAKV2l0aCBteSByYWctdG9wIGRvd24gc28gbXkgaGFpciBjYW4gYmxvdwpUaGUgZ2lybGllcyBvbiBzdGFuZGJ5IHdhdmluZyBqdXN0IHRvIHNheSBoaQpEaWQgeW91IHN0b3A/IE5vLCBJIGp1c3QgZHJvdmUgYnkK");

            var encrSecret = RandomKeyAESECB.EncryptAESECB(secret);

            var i = 0;
            while (i*16 < secret.Length)
            {
                var block = secret.Skip(i*16).Take(16);
                var dec = Tools.DecryptFirstAESECBBlock(block.ToArray());
                Console.Write(Encoding.ASCII.GetString(dec));
                i ++;
            }

            Console.ReadLine();

        }
    }
}
