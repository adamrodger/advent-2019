using System.Diagnostics;
using System.Linq;
using AdventOfCode.IntCode;
using AdventOfCode.Utilities;
using MoreLinq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 17
    /// </summary>
    public class Day17
    {
        public int Part1(string[] input)
        {
            var vm = new IntCodeEmulator(input);
            vm.Execute();

            int height = vm.StdOut.Count(c => c == 10) + 1;

            char[,] grid = new char[height, height];
            grid.ForEach((x, y, _) => grid[y, x] = ' ');

            int x = 0, y = 0;

            while (vm.StdOut.Any())
            {
                long value = vm.StdOut.Dequeue();

                if (value == '\n')
                {
                    x = 0;
                    y++;
                    continue;
                }

                grid[y, x] = (char)value;
                x++;
            }

            if (Debugger.IsAttached)
            {
                grid.Print();
            }

            int sum = 0;

            grid.ForEach((x, y, c) =>
            {
                if (c == '#' && grid.Adjacent4(x, y).All(cell => cell == '#'))
                {
                    sum += x * y;
                    grid[y, x] = 'O';
                }
            });

            return sum;
        }

        public int Part2(string[] input)
        {
            var vm = new IntCodeEmulator(input);
            vm.Program[0] = 2;

            const string overall = "B,A,B,A,C,B,A,C,B,C";
            const string A = "L,8,L,6,L,10,L,6";
            const string B = "R,6,L,6,L,10";
            const string C = "R,6,L,8,L,10,R,6";

            foreach (string command in new[] { overall, A, B ,C })
            {
                command.ForEach(o => vm.StdIn.Enqueue(o));
                vm.StdIn.Enqueue(10);
            }

            // don't show verbose output
            vm.StdIn.Enqueue('n');
            vm.StdIn.Enqueue(10);

            vm.Execute();

            if (Debugger.IsAttached)
            {
                vm.StdOut.Take(vm.StdOut.Count - 1).ForEach(l => Debug.Write((char)l));
                Debug.Flush();
            }

            return (int)vm.StdOut.Last();
        }
    }
}
