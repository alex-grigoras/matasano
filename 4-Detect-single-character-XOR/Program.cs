using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

partial class Program
{

    public class TopResults<T>
    {
        readonly Queue<T> _queue = new Queue<T>();
        private readonly int _capacity;

        public TopResults(int capacity)
        {
            _capacity = capacity;
        }

        public void Add(T item)
        {
            if (_queue.Count >= _capacity)
            {
                _queue.Dequeue();
            }
            _queue.Enqueue(item);
        }

        public List<T> Results
        {
            get { return _queue.ToList(); }
        }
    }

    static byte[] GetBytes(string hexString)
    {
        //the linq is from here http://stackoverflow.com/questions/321370/how-can-i-convert-a-hex-string-to-a-byte-array
        var bytes = Enumerable.Range(0, hexString.Length)
                     .Where(x => x % 2 == 0)
                     .Select(x => Convert.ToByte(hexString.Substring(x, 2), 16))
                     .ToArray();

        return bytes;
    }

    static List<string> GetBestDecryptions(byte[] bytes, int capacity)
    {
        var bestMatches = new TopResults<string>(5);
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
                bestMatches.Add(str);
            }
            if (b == byte.MaxValue)
            {
                break;
            }
        }

        return bestMatches.Results;
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
    
    public static void Main(string[] args)
    {
        var strBytes = input.Split('\t').Select(GetBytes);
        var ordered = strBytes.OrderByDescending(s => s.Count() - s.Distinct().Count());
        var candidates = new List<string>();
        foreach (var bytes in ordered)
        {
            candidates.AddRange(GetBestDecryptions(bytes, 5));
        }

        var best = new TopResults<string>(5);
        uint maxScore = 0;
        foreach (var c in candidates)
        {
            var score = Score(c);
            if (score >= maxScore && score > 0)
            {
                maxScore = score;
                best.Add(c);
            }
        }

        foreach (var str in best.Results)
        {
            Console.WriteLine(str);
        }

        Console.ReadKey();
    }
}
