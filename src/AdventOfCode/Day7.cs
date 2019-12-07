using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.IntCode;
using MoreLinq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 7
    /// </summary>
    public class Day7
    {
        public int Part1(string[] input)
        {
            IEnumerable<IList<int>> permutations = Enumerable.Range(0, 5).ToArray().Permutations();
            int max = -1;

            foreach (IList<int> permutation in permutations)
            {
                int output = 0;

                foreach (int thruster in permutation)
                {
                    var stdin = new Queue<int>(new[] {thruster, output});
                    var stdout = new StringBuilder();

                    var emulator = new IntCodeEmulator(input, stdin, stdout);
                    emulator.Execute();

                    output = int.Parse(stdout.ToString());
                }

                if (output > max)
                {
                    max = output;
                }
            }

            return max;
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
