using System.Linq;
using AdventOfCode.IntCode;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 9
    /// </summary>
    public class Day9
    {
        public long Part1(string[] input)
        {
            var vm = new IntCodeEmulator(input);
            vm.StdIn.Enqueue(1);
            vm.Execute();

            return vm.StdOut.Last();
        }

        public long Part2(string[] input)
        {
            var vm = new IntCodeEmulator(input);
            vm.StdIn.Enqueue(2);
            vm.Execute();

            return vm.StdOut.Last();
        }
    }
}
