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
            IEnumerable<IList<int>> permutations = Enumerable.Range(0, 5).Permutations();
            int max = -1;

            foreach (IList<int> permutation in permutations)
            {
                int output = 0;

                foreach (int id in permutation)
                {
                    var emulator = new IntCodeEmulator(input);
                    emulator.StdIn.Enqueue(id);
                    emulator.StdIn.Enqueue(output);

                    emulator.Execute();

                    output = emulator.StdOut.Dequeue();
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
            IEnumerable<IList<int>> permutations = Enumerable.Range(5, 5).Permutations();
            int max = -1;

            foreach (IList<int> permutation in permutations)
            {
                var waiting = new Queue<IntCodeEmulator>();

                foreach (int id in permutation)
                {
                    var vm = new IntCodeEmulator(input);
                    vm.StdIn.Enqueue(id);
                    waiting.Enqueue(vm);
                }

                // initial input is always 0
                int output = 0;

                while (waiting.Any())
                {
                    var vm = waiting.Dequeue();
                    vm.StdIn.Enqueue(output);

                    // keep going until it needs input
                    while (!(vm.Halted || vm.WaitingForInput))
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
