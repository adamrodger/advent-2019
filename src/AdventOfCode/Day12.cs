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

            bool xFound = false, yFound = false, zFound = false;

            while (!(xFound && yFound && zFound))
            {
                // there's got to be a nicer way to track the state, but you need equality checking so tuples work well
                xFound = xFound || !xStates.Add((moons[0].Position.X, moons[1].Position.X, moons[2].Position.X, moons[3].Position.X,
                                                 moons[0].Velocity.X, moons[1].Velocity.X, moons[2].Velocity.X, moons[3].Velocity.X));

                yFound = yFound || !yStates.Add((moons[0].Position.Y, moons[1].Position.Y, moons[2].Position.Y, moons[3].Position.Y,
                                                 moons[0].Velocity.Y, moons[1].Velocity.Y, moons[2].Velocity.Y, moons[3].Velocity.Y));

                zFound = zFound || !zStates.Add((moons[0].Position.Z, moons[1].Position.Z, moons[2].Position.Z, moons[3].Position.Z,
                                                 moons[0].Velocity.Z, moons[1].Velocity.Z, moons[2].Velocity.Z, moons[3].Velocity.Z));

                // keep going until you find all the cycles
                moons.ForEach(m => m.UpdateVelocity(moons));
                moons.ForEach(m => m.Move());
            }

            // so now we've got the intervals from the hashset counts, find the lowest common multiple of all the intervals

            long xyLCM = LCM(xStates.Count, yStates.Count);
            long xyzLCM = LCM(xyLCM, zStates.Count);

            return xyzLCM;

            // guessed 470109376 -- too low -- with xStates.Count * yStates.Count * zStates.Count
        }

        /// <summary>
        /// Calculates the smallest number which can be divided by both a and b
        /// </summary>
        /// <remarks>
        /// Stolen from that same maths website as day 10
        /// </remarks>
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
        public Point3D Position { get; }

        public Point3D Velocity { get; }

        public int TotalEnergy => (Math.Abs(this.Position.X) + Math.Abs(this.Position.Y) + Math.Abs(this.Position.Z))
                                * (Math.Abs(this.Velocity.X) + Math.Abs(this.Velocity.Y) + Math.Abs(this.Velocity.Z));

        public Moon(int x, int y, int z)
        {
            this.Position = new Point3D { X = x, Y = y, Z = z };
            this.Velocity = new Point3D();
        }

        public void UpdateVelocity(ICollection<Moon> moons)
        {
            this.Velocity.X += moons.Count(m => m.Position.X > this.Position.X) - moons.Count(m => m.Position.X < this.Position.X);
            this.Velocity.Y += moons.Count(m => m.Position.Y > this.Position.Y) - moons.Count(m => m.Position.Y < this.Position.Y);
            this.Velocity.Z += moons.Count(m => m.Position.Z > this.Position.Z) - moons.Count(m => m.Position.Z < this.Position.Z);
        }

        public void Move()
        {
            this.Position.X += this.Velocity.X;
            this.Position.Y += this.Velocity.Y;
            this.Position.Z += this.Velocity.Z;
        }
    }
}
