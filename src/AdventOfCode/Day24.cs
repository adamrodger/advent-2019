using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Utilities;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 24
    /// </summary>
    public class Day24
    {
        public int Part1(string[] input)
        {
            char[,] grid = input.ToGrid();
            HashSet<string> previous = new HashSet<string>();
            previous.Add(grid.Print());

            while (true)
            {
                char[,] newGrid = new char[grid.GetLength(0), grid.GetLength(1)];

                for (int y = 0; y < grid.GetLength(0); y++)
                {
                    for (int x = 0; x < grid.GetLength(1); x++)
                    {
                        var adjacent = grid.Adjacent4(x, y);
                        int count = adjacent.Count(c => c == '#');

                        if (grid[y, x] == '#')
                        {
                            newGrid[y, x] = count == 1 ? '#' : '.';
                        }
                        else
                        {
                            newGrid[y, x] = count == 1 || count == 2 ? '#' : '.';
                        }
                    }
                }

                string state = newGrid.Print();

                if (!previous.Add(state))
                {
                    int sum = 0;

                    // duplicate state
                    for (int y = 0; y < newGrid.GetLength(0); y++)
                    {
                        for (int x = 0; x < newGrid.GetLength(1); x++)
                        {
                            if (newGrid[y, x] != '#')
                            {
                                continue;
                            }

                            int i = (y * newGrid.GetLength(1)) + x;
                            sum += (int)Math.Pow(2, i);
                        }
                    }

                    return sum;
                }

                grid = newGrid;
            }

            // 24495935 -- too low
            // 32435853 -- too high
        }

        public int Part2(string[] input)
        {
            foreach (string line in input)
            {
                throw new NotImplementedException("Part 2 not implemented");
            }

            return 0;
        }
    }
}
