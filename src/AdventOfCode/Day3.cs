using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 3
    /// </summary>
    public class Day3
    {
        public int Part1(string[] input)
        {
            return ClosestIntersection(input, (_, __, i) => Math.Abs(i.x) + Math.Abs(i.y));
        }

        public int Part2(string[] input)
        {
            return ClosestIntersection(input, (firstPath, secondPath, i) => firstPath.IndexOf(i) + 1 + secondPath.IndexOf(i) + 1);
        }

        private static int ClosestIntersection(string[] input, Func<IList<(int x, int y)>, IList<(int x, int y)>, (int x, int y), int> distanceFactory)
        {
            var firstDirections = Parse(input[0]);
            var firstPath = BuildPath(firstDirections).ToList();

            var secondDirections = Parse(input[1]);
            var secondPath = BuildPath(secondDirections).ToList();

            ICollection<(int x, int y)> intersections = firstPath.Intersect(secondPath).ToArray();

            var distances = intersections.Select(i => distanceFactory(firstPath, secondPath, i));
            return distances.Min();
        }

        private static ICollection<(char direction, int distance)> Parse(string input)
        {
            return input.Split(',').Select(s => (s[0], int.Parse(s.Substring(1)))).ToArray();
        }

        private static IEnumerable<(int x, int y)> BuildPath(ICollection<(char direction, int distance)> steps)
        {
            (int x, int y) current = (0, 0);

            foreach ((char direction, int distance) in steps)
            {
                for (int i = 0; i < distance; i++)
                {
                    switch (direction)
                    {
                        case 'U':
                            current = (current.x, current.y + 1);
                            yield return current;
                            break;
                        case 'D':
                            current = (current.x, current.y - 1);
                            yield return current;
                            break;
                        case 'L':
                            current = (current.x - 1, current.y);
                            yield return current;
                            break;
                        case 'R':
                            current = (current.x + 1, current.y);
                            yield return current;
                            break;
                    }
                }
            }
        }
    }
}
