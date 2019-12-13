using System;
using System.Linq;
using AdventOfCode.IntCode;
using AdventOfCode.Utilities;
using MoreLinq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 13
    /// </summary>
    public class Day13
    {
        public int Part1(string[] input)
        {
            var vm = new IntCodeEmulator(input);
            vm.Execute();

            return vm.StdOut.Batch(3).Count(b => b.ElementAt(2) == 2);
        }

        public int Part2(string[] input)
        {
            var vm = new IntCodeEmulator(input);
            vm.Program[0] = 2;

            var grid = new char[30, 50];
            int score = 0;

            while (!vm.Halted)
            {
                // keep going until it needs input
                vm.ExecuteUntilYield();

                // update the grid
                while (vm.StdOut.Count > 2)
                {
                    long x = vm.StdOut.Dequeue();
                    long y = vm.StdOut.Dequeue();
                    long value = vm.StdOut.Dequeue();

                    if (x == -1 && y == 0)
                    {
                        score = (int)value;
                        continue;
                    }

                    grid[y, x] = value switch
                    {
                        0 => ' ',
                        1 => '|',
                        2 => '#',
                        3 => '_',
                        4 => 'o',
                        _ => throw new ArgumentException()
                    };
                }

                // find the ball and move the paddle towards it
                Point2D ball = grid.First(c => c == 'o');
                Point2D paddle = grid.First(c => c == '_');

                long joystick = ball.X.CompareTo(paddle.X);

                vm.StdIn.Enqueue(joystick);
            }

            return score;
        }
    }
}
