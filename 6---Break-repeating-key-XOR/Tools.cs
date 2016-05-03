using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sandbox2
{
    public class Tools
    {
        public static byte[] GetBytes(string hexString)
        {
            //the linq is from here http://stackoverflow.com/questions/321370/how-can-i-convert-a-hex-string-to-a-byte-array
            var bytes = Enumerable.Range(0, hexString.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hexString.Substring(x, 2), 16))
                .ToArray();

            return bytes;
        }

        public static string GetBestSingleByteDecryption(byte[] bytes, IStringScorer scorer)
        {
            return GetSingleByteDecryptions(bytes, 1, scorer).Best;
        }

        public static List<string> GetBestSingleByteDecryptions(byte[] bytes, int resultCount, IStringScorer scorer)
        {
            return GetSingleByteDecryptions(bytes, resultCount, scorer).Results;
        }
            
        private static FixedSizeQueue<string> GetSingleByteDecryptions(byte[] bytes, int resultCount, IStringScorer scorer)
        {
            var bestMatches = new FixedSizeQueue<string>(resultCount);
            uint maxScore = 0;
            for (byte b = 0; b <= byte.MaxValue; b++)
            {
                var b1 = b;
                var decoded = bytes.Select(e => (byte)(e ^ b1)).ToArray();
                var str = Encoding.ASCII.GetString(decoded);
                var score = scorer.Score(str);
                if (score >= maxScore && score > 0)
                {
                    maxScore = score;
                    bestMatches.Enqueue(str);
                }
                if (b == byte.MaxValue)
                {
                    break;
                }
            }

            return bestMatches;
        }

        public static int GetHammingDistance(byte[] ba, byte[] bb)
        {
            var numberOfBits = 0;
            for(var i=0; i<ba.Count(); i++)
            {
                var a = ba[i];
                var b = bb[i];

                var differingBits = (byte) (a ^ b);
                while (differingBits != 0)
                {
                    if ((differingBits & 1) == 1)
                    {
                        numberOfBits++;
                    }
                    differingBits >>= 1;
                }
            }
            return numberOfBits;
        }
    }
}
