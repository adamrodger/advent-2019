using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
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
            Point2D paddle = (0, 0);
            Point2D ball = (0, 0);

            while (!vm.Halted)
            {
                // keep going until it needs input
                vm.ExecuteUntilYield();

                // update the grid
                while (vm.StdOut.Count > 2)
                {
                    int x = (int)vm.StdOut.Dequeue();
                    int y = (int)vm.StdOut.Dequeue();
                    long value = vm.StdOut.Dequeue();

                    if (x == -1 && y == 0)
                    {
                        score = (int)value;
                    }
                    else if (value == 3)
                    {
                        paddle = (x, y);
                    }
                    else if (value == 4)
                    {
                        ball = (x, y);
                    }

                    if (Debugger.IsAttached)
                    {
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
                }

                if (Debugger.IsAttached)
                {
                    grid.Print();
                    Thread.Sleep(100);
                }

                // move the paddle towards the ball
                long joystick = ball.X.CompareTo(paddle.X);

                vm.StdIn.Enqueue(joystick);
            }

            return score;
        }
    }
}
