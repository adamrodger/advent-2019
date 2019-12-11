using System;
using System.Collections.Generic;
using AdventOfCode.IntCode;
using AdventOfCode.Utilities;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 11
    /// </summary>
    public class Day11
    {
        public enum Direction { North, South, East, West };

        public enum Turn { Left = 0, Right = 1 };

        public int Part1(string[] input)
        {
            var vm = new IntCodeEmulator(input);
            var panels = new Dictionary<(int x, int y), int>();

            var position = (0, 0);
            var direction = Direction.North;

            while (!vm.Halted)
            {
                vm.StdIn.Enqueue(panels.GetOrCreate(position));

                while (!(vm.Halted || vm.WaitingForInput))
                {
                    vm.Step();
                }

                // paint
                int paint = (int)vm.StdOut.Dequeue();
                panels[position] = paint;

                // turn and move
                Turn turn = (Turn)(int)vm.StdOut.Dequeue();

                (direction, position) = TurnAndMove(turn, direction, position);
            }

            return panels.Keys.Count;
        }

        public string Part2(string[] input)
        {
            var vm = new IntCodeEmulator(input);
            var panels = new Dictionary<(int x, int y), int>();

            var position = (0, 0);
            var direction = Direction.North;

            // start on white
            panels[position] = 1;

            while (!vm.Halted)
            {
                vm.StdIn.Enqueue(panels.GetOrCreate(position));

                while (!(vm.Halted || vm.WaitingForInput))
                {
                    vm.Step();
                }

                // paint
                int paint = (int)vm.StdOut.Dequeue();
                panels[position] = paint;

                // turn and move
                Turn turn = (Turn)(int)vm.StdOut.Dequeue();

                (direction, position) = TurnAndMove(turn, direction, position);
            }

            // map to a char grid
            char[,] grid = new char[30,50];
            foreach (KeyValuePair<(int x, int y), int> pair in panels)
            {
                grid[pair.Key.y, pair.Key.x] = pair.Value == 1 ? '#' : ' ';
            }

            return grid.Print();
        }

        private static (Direction, (int x, int y)) TurnAndMove(Turn turn, Direction direction, (int x, int y) position)
        {
            Direction newDirection;

            switch (direction)
            {
                case Direction.North:
                    newDirection = turn == Turn.Left ? Direction.West : Direction.East;
                    break;
                case Direction.South:
                    newDirection = turn == Turn.Left ? Direction.East : Direction.West;
                    break;
                case Direction.East:
                    newDirection = turn == Turn.Left ? Direction.North : Direction.South;
                    break;
                case Direction.West:
                    newDirection = turn == Turn.Left ? Direction.South : Direction.North;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            (int x, int y) newPosition;

            switch (newDirection)
            {
                case Direction.North:
                    newPosition = (position.x, position.y - 1);
                    break;
                case Direction.South:
                    newPosition = (position.x, position.y + 1);
                    break;
                case Direction.East:
                    newPosition = (position.x + 1, position.y);
                    break;
                case Direction.West:
                    newPosition = (position.x - 1, position.y);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return (newDirection, newPosition);
        }
    }
}
