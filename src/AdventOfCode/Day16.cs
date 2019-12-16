using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            string line = input[0];
            int[] basePattern = { 0, 1, 0, -1 };

            for (int x = 0; x < 100; x++)
            {
                var output = new StringBuilder(line.Length);

                for (int i = 0; i < line.Length; i++)
                {
                    IEnumerable<int> pattern = basePattern.Repeat()
                                                          .SelectMany(p => Enumerable.Repeat(p, i + 1))
                                                          .Skip(1);

                    var zipped = line.Zip(pattern, (c1, i1) => int.Parse(new string(new[] { c1 })) * i1);

                    int a = zipped.Sum();
                    int b = Math.Abs(a);
                    int c = b % 10;

                    output.Append(c.ToString());
                }

                line = output.ToString();
            }

            return int.Parse(new string(line.Take(8).ToArray()));

            // guessed 59772698 -- too low
        }

        public int Part2(string[] input)
        {
            foreach (string line in input)
            {
                throw new NotImplementedException("Part 2 not implemented");
            }

            return 0;
        }
    }
}
