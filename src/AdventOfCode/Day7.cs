using System.Collections.Generic;
using System.Linq;
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
                    var stdout = new Queue<int>();

                    var emulator = new IntCodeEmulator(input, stdin, stdout);
                    emulator.Execute();

                    output = stdout.Dequeue();
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
            IEnumerable<IList<int>> permutations = Enumerable.Range(5, 5).ToArray().Permutations();
            //var permutations = new [] { new List<int> {  9, 8, 7, 6, 5 } };
            int max = -1;

            foreach (IList<int> permutation in permutations)
            {
                var waiting = new Queue<IntCodeEmulator>();

                foreach (int id in permutation)
                {
                    var vm = new IntCodeEmulator(input, new Queue<int>(), new Queue<int>());
                    vm.StdIn.Enqueue(id);
                    waiting.Enqueue(vm);
                }

                // initial input
                int output = 0;

                while (waiting.Any())
                {
                    var vm = waiting.Dequeue();
                    vm.StdIn.Enqueue(output);

                    // keep going until it needs input
                    while (!(vm.Halted || vm.WaitingForInput()))
                    {
                        vm.Step();
                    }

                    if (!vm.Halted)
                    {
                        waiting.Enqueue(vm);
                    }

                    output = vm.StdOut.Dequeue();
                }

                if (output > max)
                {
                    max = output;
                }
            }

            return max;
        }
    }
}
