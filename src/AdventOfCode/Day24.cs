using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public int Part2(string[] input, int iterations = 200)
        {
            char[,] grid = input.ToGrid();
            grid[2, 2] = '?';
            var space = new Dictionary<int, char[,]> {[0] = grid};

            for (int i = 0; i < iterations; i++)
            {
                Simulate(space, 0);
            }

            foreach (int level in space.Keys.OrderBy(k => k))
            {
                if (space[level][0, 0] == '\0')
                {
                    continue;
                }

                Debug.WriteLine($"Depth: {level}");
                space[level].Print();
            }

            return space.Values.Select(g => g.Search(c => c == '#').Count()).Sum();

            // 1981 -- too low
            // 2048 -- too low
            // 2001 -- too low, obviously! Passes the sample though
        }

        private static void Simulate(Dictionary<int, char[,]> space, int z)
        {
            if (!space.ContainsKey(z - 1))
            {
                space[z - 1] = new char[space[0].GetLength(0), space[0].GetLength(1)];
            }
            if (!space.ContainsKey(z + 1))
            {
                space[z + 1] = new char[space[0].GetLength(0), space[0].GetLength(1)];
            }

            var grid = space[z];

            char[,] newGrid = new char[grid.GetLength(0), grid.GetLength(1)];
            newGrid[2, 2] = '?';

            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    if (x == 2 && y == 2)
                    {
                        // centre square is now a portal
                        continue;
                    }

                    var adjacent = GetAdjacentRecursive(space, x, y, z).ToArray();
                    Debug.Assert(adjacent.Length == 4 || adjacent.Length == 8);
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

            var bugs = newGrid.Search(c => c == '#');

            if (bugs.Any())
            {
                if (z <= 0)
                {
                    // recurse outwards
                    Simulate(space, z - 1);
                }

                if (z >= 0)
                {
                    // recurse in
                    Simulate(space, z + 1);
                }
            }

            space[z] = newGrid;
        }

        private static IEnumerable<char> GetAdjacentRecursive(Dictionary<int, char[,]> space, int x, int y, int z)
        {
            if (x == 2 && y == 2)
            {
                // centre square is now a portal
                yield break;
            }

            // tile above
            int dy = y - 1;
            if (dy == 2 && x == 2)
            {
                // recurse in - bottom edge of inner grid
                yield return space[z + 1][4, 0];
                yield return space[z + 1][4, 1];
                yield return space[z + 1][4, 2];
                yield return space[z + 1][4, 3];
                yield return space[z + 1][4, 4];
            }
            else if (dy < 0)
            {
                // go out the top - inner top centre of outer grid
                yield return space[z - 1][1, 2];
            }
            else
            {
                // in the same grid
                yield return space[z][dy, x];
            }

            // tile to the left
            int dx = x - 1;
            if (y == 2 && dx == 2)
            {
                // recurse in - right edge of inner grid
                yield return space[z + 1][0, 4];
                yield return space[z + 1][1, 4];
                yield return space[z + 1][2, 4];
                yield return space[z + 1][3, 4];
                yield return space[z + 1][4, 4];
            }
            else if (dx < 0)
            {
                // go out the left - inner right centre of the outer grid
                yield return space[z - 1][2, 1];
            }
            else
            {
                // in the same grid
                yield return space[z][y, dx];
            }

            // tile to the right
            dx = x + 1;
            if (y == 2 && dx == 2)
            {
                // recurse in - left edge of inner grid
                yield return space[z + 1][0, 0];
                yield return space[z + 1][1, 0];
                yield return space[z + 1][2, 0];
                yield return space[z + 1][3, 0];
                yield return space[z + 1][4, 0];
            }
            else if (dx >= space[0].GetLength(1))
            {
                // go out the right - inner left centre of the outer grid
                yield return space[z - 1][2, 3];
            }
            else
            {
                // in the same grid
                yield return space[z][y, dx];
            }

            // tile to the bottom
            dy = y + 1;
            if (dy == 2 && x == 2)
            {
                // recurse in - top edge of inner grid
                yield return space[z + 1][0, 0];
                yield return space[z + 1][0, 1];
                yield return space[z + 1][0, 2];
                yield return space[z + 1][0, 3];
                yield return space[z + 1][0, 4];
            }
            else if (dy >= space[0].GetLength(0))
            {
                // go out the bottom - inner bottom centre of the outer grid
                yield return space[z - 1][3, 2];
            }
            else
            {
                // in the same grid
                yield return space[z][dy, x];
            }
        }
    }
}
