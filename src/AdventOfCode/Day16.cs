using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AdventOfCode.Utilities;
using MoreLinq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 16
    /// </summary>
    public class Day16
    {
        public int Part1(string[] input)
        {
            string line = Transform(input[0]);

            return int.Parse(line);
        }

        public int Part2(string[] input)
        {
            string line = new string(input[0].Repeat(10000).ToArray());
            int offset = int.Parse(new string(line.Take(7).ToArray()));

            Debug.Assert(offset < line.Length); // make sure there's no need to repeat

            // 6,500,000 char string * 100 iterations == ouch. can't brute-force that really
            line = Transform(line, offset);

            return int.Parse(line);

            // guessed 56,422,847 with a complete hack of just working on skip(offset).take(8) -- too high :D
        }

        private static string Transform(string parse, int offset = 0)
        {
            int[] basePattern = { 0, 1, 0, -1 };
            int[] input = parse.Select(char.GetNumericValue).Select(c => (int)c).ToArray();

            for (int phase = 0; phase < 100; phase++)
            {
                var output = new int[input.Length];

                for (int i = 0; i < input.Length; i++)
                {
                    // got to be a mathsy way of calculating this instead of effectively reproducing all the time. This bit is slow
                    IEnumerable<int> pattern = basePattern.Repeat()
                                                          .SelectMany(p => Enumerable.Repeat(p, i + 1))
                                                          .Skip(1);

                    IEnumerable<int> zipped = input.Zip(pattern, (n, p) => n * p);

                    output[i] = zipped.Sum().Abs() % 10;
                }

                input = output;
            }

            // turn nums back into a string
            return string.Join(string.Empty, input.Skip(offset).Take(8).Select(n => n.ToString()));
        }
    }
}
