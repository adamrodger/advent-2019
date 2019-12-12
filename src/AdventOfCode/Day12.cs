using System;
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
                // calculate new velocity
                foreach (Moon moon in moons)
                {
                    moon.VelocityX += moons.Count(m => m.PositionX > moon.PositionX) - moons.Count(m => m.PositionX < moon.PositionX);
                    moon.VelocityY += moons.Count(m => m.PositionY > moon.PositionY) - moons.Count(m => m.PositionY < moon.PositionY);
                    moon.VelocityZ += moons.Count(m => m.PositionZ > moon.PositionZ) - moons.Count(m => m.PositionZ < moon.PositionZ);
                }

                // move all moons
                foreach (Moon moon in moons)
                {
                    moon.PositionX += moon.VelocityX;
                    moon.PositionY += moon.VelocityY;
                    moon.PositionZ += moon.VelocityZ;
                }
            }

            return moons.Sum(m => m.TotalEnergy);

            // guessed 9920306 - too high
            // guessed 494 -- too low
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

        public int TotalEnergy => (Math.Abs(PositionX)
                                 + Math.Abs(PositionY)
                                 + Math.Abs(PositionZ))
                                * (Math.Abs(VelocityX)
                                 + Math.Abs(VelocityY)
                                 + Math.Abs(VelocityZ));

        public Moon(int x, int y, int z)
        {
            this.PositionX = x;
            this.PositionY = y;
            this.PositionZ = z;
        }
    }
}
