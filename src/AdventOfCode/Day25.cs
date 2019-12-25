using System.Linq;
using System.Text;
using AdventOfCode.IntCode;
using AdventOfCode.Utilities;

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

            var instructions = new[]
            {
                "west",
                "south",
                "east",
                "take monolith",
                "south",
                "west",
                "west",
                "take astrolabe",
                "east",
                "east",
                "north",
                "west",
                "north",
                "west",
                "north",
                "take tambourine",
                "south",
                "west",
                "take dark matter",
                "west",
                "north"
            };

            foreach (char c in string.Join('\n', instructions))
            {
                vm.StdIn.Enqueue(c);
            }
            vm.StdIn.Enqueue('\n');

            vm.ExecuteUntilYield();

            var output = new StringBuilder();
            while (vm.StdOut.Any())
            {
               output.Append((char)vm.StdOut.Dequeue());
            }

            return output.ToString().Numbers<int>().Last();
        }
    }
}
