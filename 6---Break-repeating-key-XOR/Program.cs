using System;
using System.Collections.Generic;
using System.Linq;

namespace sandbox2
{
    public class Program
    {
       public static void Main(string[] args)
       {
           var inputBytes = Convert.FromBase64String(Data.Input);

           //determine probable key length
           const int numberOfBestkeys = 5;
           const int maxKeySize = 40;
           const int blockCount = 4;

           var keyLengths = new FixedSizeQueue<int>(numberOfBestkeys);
           float minNormalizedHD = int.MaxValue;

           for (var keysize = 2; keysize <= maxKeySize; keysize++)
           {
               var blocks = new List<byte[]>();
               for (var i = 0; i < blockCount; i++)
               {
                   var bytes = inputBytes.Skip(i * keysize).Take(keysize).ToArray();
                  blocks.Add(bytes);
               }

               var crossProduct =
                   blocks.SelectMany((s, index) => blocks.Skip(index+1).Select(s1 => new Tuple<byte[], byte[]>(s, s1)));

               var hammingDistances = crossProduct.Select(t => Tools.GetHammingDistance(t.Item1, t.Item2));
               var distanceSum = hammingDistances.Aggregate((acc,d) => acc+d);
               var normalizedHD = (float)distanceSum / (keysize * blockCount);

               if (normalizedHD < minNormalizedHD)
               {
                   minNormalizedHD = normalizedHD;
                   keyLengths.Enqueue(keysize);
               }
           }

           foreach (var keyLength in keyLengths.BestEnumeration)
           {

               var substrings = new List<List<byte>>();
               for (var i = 0; i < keyLength; i++)
               {
                   substrings.Add(new List<byte>());
                   for (var j = i; j < inputBytes.Count(); j += keyLength)
                   {
                       substrings[i].Add(inputBytes[j]);
                   }
               }

               var decryptedSubstrings = new List<string>();
               for (var i = 0; i < keyLength; i++)
               {
                   var decr = Tools.GetBestSingleByteDecryption(substrings[i].ToArray(), new LetterProbabilityScorer());
                   decryptedSubstrings.Add(decr);
               }

               for (var i = 0; i < inputBytes.Count(); i++)
               {
                   var index = i % keyLength;
                   var index2 = i / keyLength;
                   Console.Write(decryptedSubstrings[index][index2]);
               }
               Console.WriteLine(keyLength+"-------------------------------------------");
               Console.ReadKey();
           }

           Console.ReadKey();
        }
    }
}
