using System;
using System.Collections.Generic;
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

                    int inSight = InSight(x, y, input).Count();

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

        /// <summary>
        /// See https://math.stackexchange.com/a/1269056 to get the angle between two lines. Something about abs and tan
        ///
        /// That page got me to Math.Atan2 which calculates the angle from 0,0 to a given point: https://docs.microsoft.com/en-us/dotnet/api/system.math.atan2
        /// </summary>
        /// <example>
        /// double destX = 1;
        /// double destY = 1;
        ///
        /// double radians = Math.Atan2(destY, destX);
        /// double degrees = radians * (180 / Math.PI);
        ///
        /// Console.WriteLine($"{degrees}"); // should be 45 degrees because (1,1) is directly north-east from (0,0)
        /// </example>
        public int Part2(string[] input, int startX = 20, int startY = 18) // start x/y calculated from part 1
        {
            int counter = 0;

            while (true)
            {
                // get everything currently in sight, order them by their angle to 'directly north' and destroy them in order
                (int x, int y)[] inSight = InSight(startX, startY, input).OrderBy(tuple => DegreesFromNorth(tuple.x, tuple.y, startX, startY)).ToArray();

                foreach ((int x, int y) in inSight)
                {
                    char[] chars = input[y].ToCharArray();
                    chars[x] = '.'; // destroyed
                    input[y] = new string(chars); // this is gonna be slow....

                    counter++;

                    if (counter == 200)
                    {
                        return (x * 100) + y;
                    }
                }
            }

            // guessed 1218, too high. I think it's because y and x are inverted, so my 'north' is probably pointing sideways instead of directly up
            // guessed 810, still too high. That means it's really near the top-left I think (i.e. it's lower than 8,10)
        }

        /// <summary>
        /// Find all the asteroids which are in sight of the given source co-ordinates
        /// </summary>
        /// <returns>Locations of all asteroids visible from the source co-ordinates</returns>
        private static IEnumerable<(int x, int y)> InSight(int srcX, int srcY, string[] input)
        {
            char[,] grid = input.ToGrid();

            // check every other asteroid on the grid to see if it's reachable from src x/y
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
                    (int x, int y) vector = GetSimplestVector(x, y, destX, destY);

                    // follow the vector and cross out everything after the first collision
                    while (x >= 0 && x < grid.GetLength(1) && y >= 0 && y < grid.GetLength(0))
                    {
                        if (!(x == srcX && y == srcY) && grid[y, x] == '#')
                        {
                            // found an asteroid on this vector
                            if (alreadyHit)
                            {
                                // obscured by a closer asteroid on the same vector, cross it out
                                grid[y, x] = 'X';
                            }

                            alreadyHit = true;
                        }

                        x += vector.x;
                        y += vector.y;
                    }
                }
            }

            // yield all the locations that contain asteroids and aren't obscured by closer ones on the same vector
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    if (grid[y, x] == '#' && !(x == srcX && y == srcY))
                    {
                        yield return (x, y);
                    }
                }
            }
        }

        /// <summary>
        /// Get the simplest vector between (x1,y1) and (x2,y2)
        /// </summary>
        /// <remarks>
        /// The vector is simplified using the greatest common divisor, e.g. the vector from (0,0) to (4,4) isn't (4,4), it's (1,1)
        /// </remarks>
        private static (int x, int y) GetSimplestVector(int x1, int y1, int x2, int y2)
        {
            int dx = x2 - x1;
            int dy = y2 - y1;

            int gcd = GCD(dx, dy);

            return (dx / gcd, dy / gcd);
        }

        /// <summary>
        /// Get the greater common divisor between a and b
        /// </summary>
        /// <remarks>
        /// Heavily based on http://csharphelper.com/blog/2014/08/calculate-the-greatest-common-divisor-gcd-and-least-common-multiple-lcm-of-two-integers-in-c/
        /// </remarks>
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

        /// <summary>
        /// Find the angle from north in degrees between (x1,y1) and (x2,y2)
        /// </summary>
        private static double DegreesFromNorth(int x1, int y1, int x2, int y2)
        {
            double radians = Math.Atan2(y2 - y1, x2 - x1);
            double degrees = radians * (180 / Math.PI);

            if (degrees < 0)
            {
                // goes negative for >180deg - e.g. 270deg is -90deg - so correct here
                degrees += 360;
            }

            // because x and y are flipped for the puzzle grid, rotate 90deg anticlockwise to make it from 'north'
            degrees -= 90;

            if (degrees < 0)
            {
                degrees += 360;
            }

            return degrees;
        }
    }
}
