using System.Collections.Generic;
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
            var inArgs = new Queue<int>(new[] { 1 });
            var output = new StringBuilder();

            var vm = new IntCodeEmulator(input, inArgs, output);
            vm.Execute();

            string result = output.ToString();

            return int.Parse(result);
        }

        public int Part2(string[] input)
        {
            var inArgs = new Queue<int>(new[] { 5 });
            var output = new StringBuilder();

            var vm = new IntCodeEmulator(input, inArgs, output);
            vm.Execute();

            string result = output.ToString();

            return int.Parse(result);
        }
    }
}
