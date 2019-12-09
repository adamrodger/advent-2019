using System.Linq;
using AdventOfCode.IntCode;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 5
    /// </summary>
    public class Day5
    {
        public int Part1(string[] input)
        {
            var vm = new IntCodeEmulator(input);
            vm.StdIn.Enqueue(1);
            vm.Execute();

            return (int)vm.StdOut.Last();
        }

        public int Part2(string[] input)
        {
            var vm = new IntCodeEmulator(input);
            vm.StdIn.Enqueue(5);
            vm.Execute();

            return (int)vm.StdOut.Last();
        }
    }
}
