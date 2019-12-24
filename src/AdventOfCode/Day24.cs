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
            char[,] grid = input.ToGrid();
            var space = new Dictionary<int, char[,]> {[0] = grid};

            for (int i = 0; i < 200; i++)
            {
                Simulate(space, 0);
            }

            return space.Values.Select(g => g.Search(c => c == '#').Count()).Sum();

            // 1981 -- too low
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

            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    if (x == 3 && y == 3)
                    {
                        // centre square is now a portal
                        continue;
                    }

                    var adjacent = GetAdjacentRecursive(space, x, y, z);
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
            if (x == 3 && y == 3)
            {
                // centre square is now a portal
                yield break;
            }

            var grid = space[z];

            foreach (char c in grid.Adjacent4(x, y))
            {
                // simply within the same level
                yield return c;
            }

            if (x == 0 || y == 0 || x == grid.GetLength(1) - 1 || y == grid.GetLength(0) - 1)
            {
                // on the edge, recurse outwards
                grid = space[z - 1];

                if (x == 0)
                {   
                    // go out left
                    yield return grid[2, 1];
                }
                else if (y == 0)
                {
                    // go out the top
                    yield return grid[1, 2];
                }
                else if (x == grid.GetLength(1) - 1)
                {
                    // go out right
                    yield return grid[2, 3];
                }
                else if (y == grid.GetLength(0) - 1)
                {
                    // go out the bottom
                    yield return grid[3, 2];
                }
            }
            else
            {
                // otherwise, touches the centre, recurse inwards (adjacent with entire side of inner grid)
                grid = space[z + 1];

                if (x == 2 && y == 1)
                {
                    // top edge
                    yield return grid[0, 0];
                    yield return grid[0, 1];
                    yield return grid[0, 2];
                    yield return grid[0, 3];
                    yield return grid[0, 4];
                }
                else if (x == 2 && y == 3)
                {
                    // bottom edge
                    yield return grid[4, 0];
                    yield return grid[4, 1];
                    yield return grid[4, 2];
                    yield return grid[4, 3];
                    yield return grid[4, 4];
                }
                else if (x == 1 && y == 2)
                {
                    // left edge
                    yield return grid[0, 0];
                    yield return grid[1, 0];
                    yield return grid[2, 0];
                    yield return grid[3, 0];
                    yield return grid[4, 0];
                }
                else if (x == 3 && y == 2)
                {
                    // right edge
                    yield return grid[0, 4];
                    yield return grid[1, 4];
                    yield return grid[2, 4];
                    yield return grid[3, 4];
                    yield return grid[4, 4];
                }
            }
        }
    }
}
