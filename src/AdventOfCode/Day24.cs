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
            var previous = new HashSet<string> {grid.Print()};

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

                // check for duplicate state
                if (!previous.Add(state))
                {
                    int sum = 0;

                    for (int y = 0; y < newGrid.GetLength(0); y++)
                    {
                        for (int x = 0; x < newGrid.GetLength(1); x++)
                        {
                            if (newGrid[y, x] != '#')
                            {
                                continue;
                            }

                            int i = (y * newGrid.GetLength(1)) + x;
                            sum += 0x01 << i; // 2^i
                        }
                    }

                    return sum;
                }

                grid = newGrid;
            }
        }

        public int Part2(string[] input, int iterations = 200)
        {
            char[,] grid = input.ToGrid();
            grid[2, 2] = '?';

            // initialise
            var space = new Dictionary<int, char[,]>();
            for (int z = (iterations / 2) * -1; z < iterations / 2 + 1; z++)
            {
                space[z] = new char[5, 5];
            }

            space[0] = grid;

            for (int i = 0; i < iterations; i++)
            {
                space = Simulate(space, iterations);
            }

            return space.Values.Select(g => g.Search(c => c == '#').Count()).Sum();
        }

        /// <summary>
        /// Simulate a single evolution round
        /// </summary>
        /// <param name="space">Current state of the space</param>
        /// <param name="iterations">Number of iterations in the overall solution</param>
        /// <returns>New state of the space</returns>
        private static Dictionary<int, char[,]> Simulate(Dictionary<int, char[,]> space, int iterations)
        {
            var newSpace = new Dictionary<int, char[,]>(space.Count);

            // loop from -z to +z populating new state from old state per layer
            for (int z = (iterations / 2) * -1; z < iterations / 2 + 1; z++)
            {
                newSpace[z] = new char[5, 5];
                newSpace[z][2, 2] = '?';

                for (int y = 0; y < 5; y++)
                {
                    for (int x = 0; x < 5; x++)
                    {
                        if (x == 2 && y == 2)
                        {
                            // centre square is a portal to the inner dimension
                            continue;
                        }

                        var adjacent = GetAdjacentRecursive(space, x, y, z).ToArray();
                        int count = adjacent.Count(c => c == '#');

                        if (space[z][y, x] == '#')
                        {
                            newSpace[z][y, x] = count == 1 ? '#' : '.';
                        }
                        else
                        {
                            newSpace[z][y, x] = count == 1 || count == 2 ? '#' : '.';
                        }
                    }
                }
            }

            return newSpace;
        }

        /// <summary>
        /// Very (!) literal way of getting the adjacent cells to a given x/y/z triplet. Didn't bother to tighten this up - it's day 24 ffs!
        /// </summary>
        /// <remarks>'Inner' cells recurse into another z dimension, 'outer' cells recurse out to a higher one</remarks>
        /// <param name="space">Current state of the space</param>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="z">Z</param>
        /// <returns>The values of all adjacent cells</returns>
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
                if (space.ContainsKey(z + 1))
                {
                    // recurse in - bottom edge of inner grid
                    yield return space[z + 1][4, 0];
                    yield return space[z + 1][4, 1];
                    yield return space[z + 1][4, 2];
                    yield return space[z + 1][4, 3];
                    yield return space[z + 1][4, 4];
                }
            }
            else if (dy < 0)
            {
                if (space.ContainsKey(z - 1))
                {
                    // go out the top - inner top centre of outer grid
                    yield return space[z - 1][1, 2];
                }
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
                if (space.ContainsKey(z + 1))
                {
                    // recurse in - right edge of inner grid
                    yield return space[z + 1][0, 4];
                    yield return space[z + 1][1, 4];
                    yield return space[z + 1][2, 4];
                    yield return space[z + 1][3, 4];
                    yield return space[z + 1][4, 4];
                }
            }
            else if (dx < 0)
            {
                if (space.ContainsKey(z - 1))
                {
                    // go out the left - inner right centre of the outer grid
                    yield return space[z - 1][2, 1];
                }
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
                if (space.ContainsKey(z + 1))
                {
                    // recurse in - left edge of inner grid
                    yield return space[z + 1][0, 0];
                    yield return space[z + 1][1, 0];
                    yield return space[z + 1][2, 0];
                    yield return space[z + 1][3, 0];
                    yield return space[z + 1][4, 0];
                }
            }
            else if (dx >= space[0].GetLength(1))
            {
                if (space.ContainsKey(z - 1))
                {
                    // go out the right - inner left centre of the outer grid
                    yield return space[z - 1][2, 3];
                }
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
                if (space.ContainsKey(z + 1))
                {
                    // recurse in - top edge of inner grid
                    yield return space[z + 1][0, 0];
                    yield return space[z + 1][0, 1];
                    yield return space[z + 1][0, 2];
                    yield return space[z + 1][0, 3];
                    yield return space[z + 1][0, 4];
                }
            }
            else if (dy >= space[0].GetLength(0))
            {
                if (space.ContainsKey(z - 1))
                {
                    // go out the bottom - inner bottom centre of the outer grid
                    yield return space[z - 1][3, 2];
                }
            }
            else
            {
                // in the same grid
                yield return space[z][dy, x];
            }
        }
    }
}
