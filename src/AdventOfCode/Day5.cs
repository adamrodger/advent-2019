using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var stdin = new Queue<int>(new[] { 1 });
            var stdout = new Queue<int>();

            var vm = new IntCodeEmulator(input, stdin, stdout);
            vm.Execute();

            return stdout.Last();
        }

        public int Part2(string[] input)
        {
            var stdin = new Queue<int>(new[] { 5 });
            var stdout = new Queue<int>();

            var vm = new IntCodeEmulator(input, stdin, stdout);
            vm.Execute();

            return stdout.Last();
        }
    }
}
