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

        public int Part2(string[] input)
        {
            foreach (string line in input)
            {
                throw new NotImplementedException("Part 2 not implemented");
            }

            return 0;
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
