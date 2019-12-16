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
            int[] nums = input[0].Select(char.GetNumericValue).Select(c => (int)c).ToArray();
            int[] output = Transform(nums, 0);

            char[] c = output.Take(8).Select(o => o.ToString()[0]).ToArray();
            return int.Parse(new string(c));
        }

        public int Part2(string[] input)
        {
            int offset = int.Parse(new string(input[0].Take(7).ToArray()));

            int[] nums = input[0].Select(char.GetNumericValue).Select(c => (int)c).Repeat(10000).ToArray();
            int[] output = TransformPart2(nums, offset);

            char[] c = output.Skip(offset).Take(8).Select(o => o.ToString()[0]).ToArray();
            return int.Parse(new string(c));
        }

        private static int[] TransformPart2(int[] input, int offset)
        {
            for (int phase = 0; phase < 100; phase++)
            {
                var output = new int[input.Length];
                int previousSum = 0;

                for (int i = input.Length - 1; i >= offset; i--)
                {
                    int sum = input[i] + previousSum;
                    output[i] = sum.Abs() % 10;

                    previousSum = sum;
                }

                input = output;
            }

            // turn nums back into a string
            return input;
        }
    }
}
