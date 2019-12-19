using AdventOfCode.IntCode;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 19
    /// </summary>
    public class Day19
    {
        public int Part1(string[] input)
        {
            int sum = 0;

            for (int y = 0; y < 50; y++)
            {
                for (int x = 0; x < 50; x++)
                {
                    if (IsInRange(input, x, y))
                    {
                        sum++;
                    }
                }
            }

            return sum;
        }

        public int Part2(string[] input)
        {
            // start a good way down
            int x = 0;
            int y = 100;

            while (true)
            {
                // walk x across until you hit the edge of the beam
                while (!IsInRange(input, x, y))
                {
                    x++;
                }

                // check if it's big enough for a 100x100 square
                if (IsInRange(input, x + 99, y - 99))
                {
                    return (x * 10000) + (y - 99);
                }

                // walk y down
                y++;
            }
        }

        private static bool IsInRange(string[] input, int x, int y)
        {
            var vm = new IntCodeEmulator(input);

            vm.StdIn.Enqueue(x);
            vm.StdIn.Enqueue(y);

            vm.Execute();

            return vm.StdOut.Dequeue() == 1;
        }
    }
}
