using System.Collections.Generic;
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
            int[] signal = input[0].Select(char.GetNumericValue).Select(c => (int)c).ToArray();
            int[] output = TransformPart1(signal);

            char[] c = output.Take(8).Select(o => o.ToString()[0]).ToArray();
            return int.Parse(new string(c));
        }

        public int Part2(string[] input)
        {
            int offset = int.Parse(new string(input[0].Take(7).ToArray()));

            int[] signal = input[0].Select(char.GetNumericValue).Select(c => (int)c).Repeat(10000).ToArray();
            int[] output = TransformPart2(signal, offset);

            char[] c = output.Skip(offset).Take(8).Select(o => o.ToString()[0]).ToArray();
            return int.Parse(new string(c));
        }

        /// <summary>
        /// Creates an infinite cycle of the base phase pattern multiplied by the output index and then used to calculate
        /// the value at the output index
        /// </summary>
        /// <param name="input">Input signal</param>
        /// <returns>Transformed output signal</returns>
        private static int[] TransformPart1(int[] input)
        {
            int[] basePattern = { 0, 1, 0, -1 };

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

            return input;
        }

        /// <summary>
        /// Walks backwards through the array because I noticed that output[n] = (input[n] + input[n+1] + ...) mod 10
        /// but _only_ for the second half of the signal. Fortunately, the offset is in the second half.
        /// </summary>
        /// <param name="input">Input signal</param>
        /// <param name="offset">Offset to the 8-digit answer</param>
        /// <returns>Transformed output signal</returns>
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

            return input;
        }
    }
}
