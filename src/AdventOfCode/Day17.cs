using System;
using System.Linq;
using AdventOfCode.IntCode;
using AdventOfCode.Utilities;

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

            int dim = (int)Math.Ceiling(Math.Sqrt(vm.StdOut.Count));

            char[,] grid = new char[dim,dim];
            int x = 0, y = 0;

            while(vm.StdOut.Any())
            {
                long value = vm.StdOut.Dequeue();

                if (value == 10)
                {
                    x = 0;
                    y++;
                    continue;
                }

                if (value == 35)
                {
                    grid[y, x] = '#';
                }
                else if (value == 46)
                {
                    grid[y, x] = '.';
                }

                x++;
            }

            grid.Print();

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

            // 1961 -- too high
        }

        public int Part2(string[] input)
        {
            var vm = new IntCodeEmulator(input);
            vm.Program[0] = 2;

            foreach (string line in input)
            {
                throw new NotImplementedException("Part 2 not implemented");
            }

            return 0;
        }
    }
}
