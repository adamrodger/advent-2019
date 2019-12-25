using System;
using System.Diagnostics;
using System.Linq;
using AdventOfCode.IntCode;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 25
    /// </summary>
    public class Day25
    {
        public int Part1(string[] input)
        {
            var vm = new IntCodeEmulator(input);

            while (true)
            {
                vm.ExecuteUntilYield();

                while (vm.StdOut.Any())
                {
                    Debug.Write((char)vm.StdOut.Dequeue());
                }

                string line = Console.ReadLine();
                foreach (char c in line)
                {
                    vm.StdIn.Enqueue(c);
                }
            }
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
