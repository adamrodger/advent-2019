using System;
using System.Linq;
using AdventOfCode.Utilities;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 10
    /// </summary>
    public class Day10
    {
        public int Part1(string[] input)
        {
            char[,] grid = input.ToGrid();
            int result = int.MinValue;

            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    if (grid[y, x] != '#') continue; // only calculate from src asteroids

                    int inSight = InSight(x, y, input);

                    if (inSight > result)
                    {
                        result = inSight;
                    }
                }
            }

            return result;

            // guessed 398 -- too high
            // guessed 288 -- too high, but says it's someone else's answer weirdly
        }

        public int Part2(string[] input)
        {
            foreach (string line in input)
            {
                throw new NotImplementedException("Part 2 not implemented");
            }

            return 0;
        }

        private static int InSight(int srcX, int srcY, string[] input)
        {
            char[,] grid = input.ToGrid();

            // check every other point on the grid to see if it's reachable from src x/y
            for (int destY = 0; destY < grid.GetLength(0); destY++)
            {
                for (int destX = 0; destX < grid.GetLength(1); destX++)
                {
                    if (grid[destY, destX] != '#' || srcX == destX && srcY == destY)
                    {
                        // target is either not an asteroid or it's the src asteroid
                        continue;
                    }

                    bool alreadyHit = false;
                    int x = srcX;
                    int y = srcY;
                    var vector = GetSimplestVector(x, y, destX, destY);

                    // follow the vector and cross out everything after the first collision
                    while (x >= 0 && x < grid.GetLength(1) && y >= 0 && y < grid.GetLength(0))
                    {
                        if (!(x == srcX && y == srcY) && grid[y, x] == '#')
                        {
                            // found an asteroid we can potentially see
                            if (alreadyHit)
                            {
                                // obscured by an earlier asteroid, cross it out
                                grid[y, x] = 'X';
                            }

                            alreadyHit = true;
                        }

                        x += vector.x;
                        y += vector.y;
                    }
                }
            }

            return grid.Where((_, __, c) => c == '#').Count() - 1;
        }

        /// <summary>
        /// Get a simplified vector between two points
        /// </summary>
        private static (int x, int y) GetSimplestVector(int x1, int y1, int x2, int y2)
        {
            int dx = x2 - x1;
            int dy = y2 - y1;

            // need to simplify it with greatest common divisor, e.g. the vector from (0,0) to (4,4) isn't (4,4), it's (1,1)
            int gcd = GCD(dx, dy);

            return (dx / gcd, dy / gcd);
        }

        /// <summary>
        /// Mostly stolen from http://csharphelper.com/blog/2014/08/calculate-the-greatest-common-divisor-gcd-and-least-common-multiple-lcm-of-two-integers-in-c/
        /// </summary>
        private static int GCD(int a, int b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);

            if (a == 0 || b == 0)
            {
                return Math.Max(a, b);
            }

            // Pull out remainders.
            while (true)
            {
                int remainder = a % b;
                if (remainder == 0) return b;
                a = b;
                b = remainder;
            }
        }
    }
}
