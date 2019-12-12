using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Utilities;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 12
    /// </summary>
    public class Day12
    {
        public int Part1(string[] input)
        {
            var moons = input.Select(i => i.Numbers<int>()).Select(n => new Moon(n[0], n[1], n[2])).ToList();

            for (int i = 0; i < 1000; i++)
            {
                moons.ForEach(m => m.UpdateVelocity(moons));
                moons.ForEach(m => m.Move());
            }

            return moons.Sum(m => m.TotalEnergy);

            // guessed 9920306 - too high (wasn't doing velocity and move independently)
            // guessed 494 -- too low (was adding potential/kinetic instead of multiplying)
        }

        public long Part2(string[] input)
        {
            // theory is - just imagine them in 2d space and all 4 orbiting at different speeds (like real planets)
            // and then you need work out when they'll all 'sync up' again. You can do that by multiplying the intervals
            // for each of the individual orbits to get the 'total' when they all sync up again. But, you've got 3 different dimensions
            // to work with, so you need to multiply the multiples of the intervals of the individual dimensions, or something :D

            var moons = input.Select(i => i.Numbers<int>()).Select(n => new Moon(n[0], n[1], n[2])).ToList();

            // erm this isn't nice.... it's the 4 positions and 4 velocities as a state of a single dimension
            var xStates = new HashSet<(int, int, int, int, int, int, int, int)>();
            var yStates = new HashSet<(int, int, int, int, int, int, int, int)>();
            var zStates = new HashSet<(int, int, int, int, int, int, int, int)>();

            while (true)
            {
                // there's got to be a nicer way to build this, but you need equality checking so tuples are nice
                bool unique = xStates.Add((moons[0].PositionX, moons[1].PositionX, moons[2].PositionX, moons[3].PositionX,
                                           moons[0].VelocityX, moons[1].VelocityX, moons[2].VelocityX, moons[3].VelocityX));

                if (!unique)
                {
                    break;
                }

                // keep going until you find a cycle
                moons.ForEach(m => m.UpdateVelocity(moons));
                moons.ForEach(m => m.Move());
            }

            // does it matter that we're not resetting state for each one? The interval would still be the same, but from a different starting point, right?
            while (true)
            {
                // there's got to be a nicer way to build this
                bool unique = yStates.Add((moons[0].PositionY, moons[1].PositionY, moons[2].PositionY, moons[3].PositionY,
                                           moons[0].VelocityY, moons[1].VelocityY, moons[2].VelocityY, moons[3].VelocityY));

                if (!unique)
                {
                    break;
                }

                // keep going until you find a cycle
                moons.ForEach(m => m.UpdateVelocity(moons));
                moons.ForEach(m => m.Move());
            }

            // lots of copy/paste...
            while (true)
            {
                // there's got to be a nicer way to build this
                bool unique = zStates.Add((moons[0].PositionZ, moons[1].PositionZ, moons[2].PositionZ, moons[3].PositionZ,
                                           moons[0].VelocityZ, moons[1].VelocityZ, moons[2].VelocityZ, moons[3].VelocityZ));

                if (!unique)
                {
                    break;
                }

                // keep going until you find a cycle
                moons.ForEach(m => m.UpdateVelocity(moons));
                moons.ForEach(m => m.Move());
            }

            // so now we've got the intervals from the hashset counts, find the lowest common multiple of all the intervals

            return LCM(xStates.Count, LCM(yStates.Count, zStates.Count));

            // guessed 470109376 -- too low -- with xStates.Count * yStates.Count * zStates.Count
        }

        /// <summary>
        /// Stolen from that same maths website as day 10
        /// </summary>
        private static long LCM(long a, long b)
        {
            return a * b / GCD(a, b);
        }

        /// <summary>
        /// Get the greater common divisor between a and b
        /// </summary>
        /// <remarks>
        /// Stolen from day 10 but converted to work on longs :D
        /// </remarks>
        private static long GCD(long a, long b)
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
                long remainder = a % b;
                if (remainder == 0) return b;
                a = b;
                b = remainder;
            }
        }
    }

    public class Moon
    {
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public int PositionZ { get; set; }

        public int VelocityX { get; set; }
        public int VelocityY { get; set; }
        public int VelocityZ { get; set; }

        public int TotalEnergy => (Math.Abs(PositionX) + Math.Abs(PositionY) + Math.Abs(PositionZ))
                                * (Math.Abs(VelocityX) + Math.Abs(VelocityY) + Math.Abs(VelocityZ));

        public Moon(int x, int y, int z)
        {
            this.PositionX = x;
            this.PositionY = y;
            this.PositionZ = z;
        }

        public void UpdateVelocity(ICollection<Moon> moons)
        {
            this.VelocityX += moons.Count(m => m.PositionX > this.PositionX) - moons.Count(m => m.PositionX < this.PositionX);
            this.VelocityY += moons.Count(m => m.PositionY > this.PositionY) - moons.Count(m => m.PositionY < this.PositionY);
            this.VelocityZ += moons.Count(m => m.PositionZ > this.PositionZ) - moons.Count(m => m.PositionZ < this.PositionZ);
        }

        public void Move()
        {
            this.PositionX += this.VelocityX;
            this.PositionY += this.VelocityY;
            this.PositionZ += this.VelocityZ;
        }
    }
}
