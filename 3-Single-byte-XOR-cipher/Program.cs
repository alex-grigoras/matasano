using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

partial class Program
{
    static void Main(String[] args)
    {
        var input = "1b37373331363f78151b7f2b783431333d78397828372d363c78373e783a393b3736";
        //the linq is from here http://stackoverflow.com/questions/321370/how-can-i-convert-a-hex-string-to-a-byte-array
        var bytes = Enumerable.Range(0, input.Length)
                     .Where(x => x % 2 == 0)
                     .Select(x => Convert.ToByte(input.Substring(x, 2), 16))
                     .ToArray();

        var bestMatches = new Queue<string>();
        const int capacity = 5; //we only care about top 5 best strings
        uint maxScore = 0;
        for (byte b = 0; b <= byte.MaxValue; b++)
        {
            var b1 = b;
            var decoded = bytes.Select(e => (byte)(e ^ b1)).ToArray();
            var str = Encoding.ASCII.GetString(decoded);
            var score = Score(str);
            if (score >= maxScore && score > 0)
            {
                maxScore = score;
                if (bestMatches.Count > capacity)
                {
                    bestMatches.Dequeue();
                }
                bestMatches.Enqueue(str);
            }
            if (b == byte.MaxValue)
            {
                break;
            }
        }

        foreach (var match in bestMatches)
        {
            Console.WriteLine(match);
        }
        Console.WriteLine("done");
        Console.ReadKey();
    }

    private static uint Score(string str)
    {
        uint score = 0;
        for (var i = 0; i < str.Length - 1; i++)
        {
            var digram = str.Substring(i, 2).ToUpper();
            if (Numbers.ContainsKey(digram[0]) && Numbers.ContainsKey(digram[1]))
            {
                score += DigramFreq[Numbers[digram[0]], Numbers[digram[1]]];
            }
        }
        return score;
    }
}
