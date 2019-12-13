using System;
using System.Collections.Generic;
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
            grid.ForEach((x, y, _) => grid[y,x] = ' ');

            int score = 0;

            while (!vm.Halted)
            {
                // keep going until it needs input
                while (!(vm.Halted || vm.WaitingForInput))
                {
                    vm.Step();
                }

                // update the grid
                foreach (IEnumerable<long> output in vm.StdOut.Batch(3))
                {
                    var x = output.ElementAt(0);
                    var y = output.ElementAt(1);
                    var value = output.ElementAt(2);

                    if (x == -1 && y == 0)
                    {
                        score = (int)value;
                        continue;
                    }

                    char c;

                    switch (value)
                    {
                        case 0: c = ' '; break; // empty
                        case 1: c = '|'; break; // wall
                        case 2: c = '#'; break; // block
                        case 3: c = '_'; break; // paddle
                        case 4: c = 'o'; break; // ball
                        default: throw new ArgumentException();
                    }

                    grid[y, x] = c;
                }

                vm.StdOut.Clear();

                // print the grid
                //Console.Clear();
                //grid.Print();
                //Thread.Sleep(1);

                // check for input
                /*long joystick;
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.LeftArrow)
                {
                    joystick = -1;
                }
                else if (key.Key == ConsoleKey.RightArrow)
                {
                    joystick = 1;
                }
                else
                {
                    // any other key
                    joystick = 0;
                }*/

                long joystick = 0;
                
                // find the ball and move the paddle towards it
                var ball = grid.FindFirst(c => c == 'o');
                var paddle = grid.FindFirst(c => c == '_');

                if (ball.x > paddle.x)
                {
                    joystick = 1;
                }
                else if (ball.x < paddle.x)
                {
                    joystick = -1;
                }
                else
                {
                    joystick = 0;
                }

                vm.StdIn.Enqueue(joystick);
            }

            //score = (int)vm.StdOut.Last();
            Console.WriteLine($"Score: {score}");

            return score;
        }
    }
}
