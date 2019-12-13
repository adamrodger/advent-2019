using System;
using System.Linq;
using AdventOfCode.IntCode;
using MoreLinq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 13
    /// </summary>
    public class Day13
    {
        public int Part1(string[] input)
        {
            var vm = new IntCodeEmulator(input);
            vm.Execute();

            return vm.StdOut.Batch(3).Count(b => b.ElementAt(2) == 2);

            // 391 -- too high
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
