using System;
using System.Linq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 8
    /// </summary>
    public class Day8
    {
        public int Part1(string[] input)
        {
            string image = input[0];
            int minZeroes = int.MaxValue;
            int result = int.MinValue;

            for (int i = 0; i < image.Length; i += 25 * 6)
            {
                string layer = new string(image.Skip(i).Take(25 * 6).ToArray());

                int zeroes = layer.Count(l => l == '0');

                if (zeroes < minZeroes)
                {
                    minZeroes = zeroes;
                    result = layer.Count(l => l == '1') * layer.Count(l => l == '2');
                }
            }

            return result;
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
