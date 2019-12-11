using System.Collections.Generic;
using System.Linq;
using AdventOfCode.IntCode;
using AdventOfCode.Utilities;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 11
    /// </summary>
    public class Day11
    {
        private const int Black = 0;
        private const int White = 1;

        public int Part1(string[] input)
        {
            Dictionary<(int x, int y), int> panels = RunPaintingRobot(input, Black);

            return panels.Keys.Count;
        }

        public string Part2(string[] input)
        {
            Dictionary<(int x, int y), int> panels = RunPaintingRobot(input, White);

            // map to a char grid so we can print it
            int maxX = panels.Keys.Max(k => k.x);
            int maxY = panels.Keys.Max(k => k.y) + 1;

            var grid = new char[maxY, maxX];

            // initialise to all black
            grid.ForEach((x, y, _) => grid[y, x] = ' ');

            // colour in the painted squares
            foreach (KeyValuePair<(int x, int y), int> pair in panels.Where(p => p.Value == White))
            {
                grid[pair.Key.y, pair.Key.x] = '█';
            }

            return grid.Print();
        }

        private static Dictionary<(int x, int y), int> RunPaintingRobot(IReadOnlyList<string> input, int startingColour)
        {
            var position = (0, 0);
            var direction = Bearing.North;

            var vm = new IntCodeEmulator(input);
            var panels = new Dictionary<(int x, int y), int>
            {
                [position] = startingColour
            };

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
                var turn = (TurnDirection)(int)vm.StdOut.Dequeue();
                direction = direction.Turn(turn);
                position = position.Move(direction);
            }

            return panels;
        }
    }
}
