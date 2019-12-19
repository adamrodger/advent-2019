using System;
using System.Linq;
using AdventOfCode.IntCode;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 19
    /// </summary>
    public class Day19
    {
        public int Part1(string[] input)
        {
            int sum = 0;

            for (int y = 0; y < 50; y++)
            {
                for (int x = 0; x < 50; x++)
                {
                    var vm = new IntCodeEmulator(input);
                    vm.ExecuteUntilYield();

                    vm.StdIn.Enqueue(x);
                    vm.ExecuteUntilYield();

                    vm.StdIn.Enqueue(y);
                    vm.ExecuteUntilYield();

                    sum += (int)vm.StdOut.Dequeue();
                }
            }

            return sum;
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
