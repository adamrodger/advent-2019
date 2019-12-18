using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Utilities;

namespace AdventOfCode
{
    public class Day18Attempt2
    {
        public enum Move { North = 1, South = 2, West = 3, East = 4 };

        private static readonly IDictionary<Move, Point2D> Deltas = new Dictionary<Move, Point2D>(4)
        {
            [Move.North] = (0, -1),
            [Move.South] = (0, 1),
            [Move.West] = (-1, 0),
            [Move.East] = (1, 0)
        };

        public int Part1(string[] input)
        {
            char[,] grid = new char[input.Length, input[0].Length];

            List<Point2D> robots = new List<Point2D>();

            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    char c = input[y][x];

                    grid[y, x] = c;

                    if (c == '@')
                    {
                        robots.Add((x, y));
                    }
                }
            }

            int result = this.FindShortestPath(grid, robots, string.Empty);
            return result;
        }

        public int Part2(string[] input)
        {
            // convert the map to allow 4 robots - so hacky :D
            input[39] = "#.#.............#...............#......@#@........#.........#.................M.#";
            input[40] = "#################################################################################";
            input[41] = "#.#...#.......#........................@#@....#.........#.........#..d#...#.....#";

            return this.Part1(input);
        }

        private static Dictionary<(int robotsHash, string foundKeys), int> Cache = new Dictionary<(int robotsHash, string foundKeys), int>();

        private int FindShortestPath(char[,] grid, List<Point2D> robots, string foundKeys)
        {
            // how can we generate a cache key for multiple bots?!
            var cacheKey = (robots.GetCombinedHashCode(), new string(foundKeys.OrderBy(c => c).ToArray()));

            if (Cache.ContainsKey(cacheKey))
            {
                return Cache[cacheKey];
            }

            Dictionary<char, (Point2D point, int distance, Point2D robot)> available = this.AvailableKeys(grid, robots, foundKeys);

            int result = 0;

            if (available.Count > 0)
            {
                // not found all the keys yet
                var possibilities = new Dictionary<char, int>();

                // branch down all possible routes
                foreach (KeyValuePair<char, (Point2D point, int distance, Point2D robot)> key in available)
                {
                    // got to track which robot moved and where they moved to
                    var newRobots = robots.Except(new [] { key.Value.robot }).Append(key.Value.point).ToList();
                    possibilities[key.Key] = key.Value.distance + this.FindShortestPath(grid, newRobots, foundKeys + key.Key);
                }

                // pick the shortest path to the final key
                result = possibilities.Values.Min();
            }

            Cache[cacheKey] = result;
            return result;
        }

        private Dictionary<char, (Point2D point, int distance, Point2D robot)> AvailableKeys(char[,] grid, List<Point2D> robots, string foundKeys)
        {
            // collect all visible keys from all available robots
            var keys = new Dictionary<char, (Point2D point, int distance, Point2D robot)>();

            foreach (Point2D robot in robots)
            {
                var robotKeys = this.AvailableKeys(grid, robot, foundKeys);

                foreach (var robotKey in robotKeys)
                {
                    keys[robotKey.Key] = (robotKey.Value.point, robotKey.Value.distance, robot);
                }
            }

            return keys;
        }

        private Dictionary<char, (Point2D point, int distance)> AvailableKeys(char[,] grid, Point2D start, string foundKeys)
        {
            Dictionary<Point2D, int> visited = new Dictionary<Point2D, int> { [start] = 0 };
            Dictionary<char, (Point2D point, int distance)> availableKeys = new Dictionary<char, (Point2D point, int distance)>();

            Queue<Point2D> todo = new Queue<Point2D>();
            todo.Enqueue(start);

            while (todo.Any())
            {
                Point2D current = todo.Dequeue();

                foreach (Move move in Deltas.Keys)
                {
                    Point2D delta = Deltas[move];
                    Point2D next = current + delta;

                    // make sure we've not fallen off the map
                    if (next.X < 0 || next.X >= grid.GetLength(1) || next.Y < 0 || next.Y >= grid.GetLength(0))
                    {
                        continue;
                    }

                    // make sure we've not already been there
                    if (visited.ContainsKey(next))
                    {
                        continue;
                    }

                    char c = grid[next.Y, next.X];

                    // make sure we've not hit a wall
                    if (c == '#')
                    {
                        continue;
                    }

                    int distance = visited[current] + 1;
                    visited[next] = distance;

                    // stop if we hit a locked door for which we don't have the key
                    if (c >= 'A' && c <= 'Z' && !foundKeys.ToUpper().Contains(c))
                    {
                        continue;
                    }

                    if (c >= 'a' && c <= 'z' && !foundKeys.Contains(c))
                    {
                        // found a key!!
                        availableKeys[c] = (next, distance);
                    }
                    else
                    {
                        // keep searching...
                        todo.Enqueue(next);
                    }
                }
            }

            return availableKeys;
        }

        /*public class Robots : IEquatable<Robots>, IEnumerable<Point2D>
        {
            public Point2D A { get; }
            public Point2D B { get; }
            public Point2D C { get; }
            public Point2D D { get; }

            public Robots(Point2D a, Point2D b, Point2D c, Point2D d)
            {
                this.A = a;
                this.B = b;
                this.C = c;
                this.D = d;
            }

            /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
            /// <param name="other">An object to compare with this object.</param>
            /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
            public bool Equals(Robots other)
            {
                if (ReferenceEquals(null, other))
                {
                    return false;
                }

                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                return this.A.Equals(other.A)
                    && this.B.Equals(other.B)
                    && this.C.Equals(other.C)
                    && this.D.Equals(other.D);
            }

            /// <summary>Returns an enumerator that iterates through the collection.</summary>
            /// <returns>An enumerator that can be used to iterate through the collection.</returns>
            public IEnumerator<Point2D> GetEnumerator()
            {
                if (A != default) yield return A;
                if (B != default) yield return B;
                if (C != default) yield return C;
                if (D != default) yield return D;
            }

            /// <summary>Determines whether the specified object is equal to the current object.</summary>
            /// <param name="obj">The object to compare with the current object.</param>
            /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                {
                    return false;
                }

                if (ReferenceEquals(this, obj))
                {
                    return true;
                }

                if (obj.GetType() != this.GetType())
                {
                    return false;
                }

                return Equals((Robots)obj);
            }

            /// <summary>Serves as the default hash function.</summary>
            /// <returns>A hash code for the current object.</returns>
            public override int GetHashCode()
            {
                unchecked
                {
                    int hashCode = this.A.GetHashCode();
                    hashCode = (hashCode * 397) ^ this.B.GetHashCode();
                    hashCode = (hashCode * 397) ^ this.C.GetHashCode();
                    hashCode = (hashCode * 397) ^ this.D.GetHashCode();
                    return hashCode;
                }
            }

            /// <summary>Returns an enumerator that iterates through a collection.</summary>
            /// <returns>An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.</returns>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }*/
    }

    public static class RobotExtensions
    {
        public static int GetCombinedHashCode(this ICollection<Point2D> robots)
        {
            return robots.Aggregate(robots.First().GetHashCode(), (hashcode, robot) => (hashcode * 397) ^ robot.GetHashCode());
        }
    }
}
