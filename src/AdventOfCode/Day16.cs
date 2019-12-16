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

            char[] c = output.Select(o => o.ToString()[0]).ToArray();
            return int.Parse(new string(c));
        }

        public int Part2(string[] input)
        {
            int offset = int.Parse(new string(input[0].Take(7).ToArray()));

            int[] signal = input[0].Select(char.GetNumericValue).Select(c => (int)c).Repeat(10000).ToArray();
            int[] output = TransformPart2(signal, offset);

            char[] c = output.Select(o => o.ToString()[0]).ToArray();
            return int.Parse(new string(c));
        }

        /// <summary>
        /// Creates an infinite cycle of the base phase pattern multiplied by the output index and then used to calculate
        /// the value at the output index
        /// </summary>
        /// <param name="input">Input signal</param>
        /// <returns>Answer digits</returns>
        private static int[] TransformPart1(int[] input)
        {
            int[] basePattern = { 0, 1, 0, -1 };

            for (int phase = 0; phase < 100; phase++)
            {
                var output = new int[input.Length];

                for (int i = 0; i < input.Length; i++)
                {
                    // infinite repeating pattern, multiplied by i, offset one to the left and then by i because first i elements are all 0
                    IEnumerable<int> pattern = basePattern.Repeat()
                                                          .SelectMany(p => Enumerable.Repeat(p, i + 1))
                                                          .Skip(1 + i);

                    // no need to use first i iterations of input because they're all multiplied by 0
                    IEnumerable<int> zipped = input.Skip(i).Zip(pattern, (n, p) => n * p);

                    output[i] = zipped.Sum().Abs() % 10;
                }

                input = output;
            }

            return input.Take(8).ToArray();
        }

        /// <summary>
        /// Walks backwards through the array because I noticed that output[n] = (input[n] + input[n+1] + ...) mod 10
        /// but _only_ for the second half of the signal. Fortunately, the offset is in the second half.
        /// </summary>
        /// <param name="input">Input signal</param>
        /// <param name="offset">Offset to the 8-digit answer</param>
        /// <returns>Answer digits</returns>
        private static int[] TransformPart2(int[] input, int offset)
        {
            // no need to work on anything prior to the offset
            input = input.Skip(offset).ToArray();

            for (int phase = 0; phase < 100; phase++)
            {
                var output = new int[input.Length];
                int previousSum = 0;

                // output[i] = (input[i] + input[i+1] + input[i+2] + ...) % 10
                // go backwards through the array keeping a running some of digits to the 'right' of i
                for (int i = input.Length - 1; i >= 0; i--)
                {
                    int sum = input[i] + previousSum;
                    output[i] = sum % 10;

                    previousSum = sum;
                }

                input = output;
            }

            return input.Take(8).ToArray();
        }
    }
}
