using System;
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
            var keys = new Dictionary<char, Point2D>();
            var doors = new Dictionary<char, Point2D>();
            Point2D start = (-1, -1);

            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    char c = input[y][x];

                    grid[y, x] = c;

                    if (c >= 'a' && c <= 'z')
                    {
                        keys.Add(c, (x, y));
                    }
                    else if (c >= 'A' && c <= 'Z')
                    {
                        doors.Add(c, (x, y));
                    }
                    else if (c == '@')
                    {
                        start = (x, y);
                    }
                }
            }

            int result = this.FindShortestPath(grid, start, string.Empty);
            return result;
        }

        public int Part2(string[] input)
        {
            throw new NotImplementedException();
        }

        private static Dictionary<(Point2D point, string foundKeys), int> Cache = new Dictionary<(Point2D point, string foundKeys), int>();

        private int FindShortestPath(char[,] grid, Point2D start, string foundKeys)
        {
            var cacheKey = (start, new string(foundKeys.OrderBy(c => c).ToArray()));

            if (Cache.ContainsKey(cacheKey))
            {
                return Cache[cacheKey];
            }

            var available = this.AvailableKeys(grid, start, foundKeys);
            int result = 0;

            if (available.Count > 0)
            {
                // not found all the keys yet
                var possibilities = new Dictionary<char, int>();

                // branch down all possible routes
                foreach (var key in available)
                {
                    possibilities[key.Key] = key.Value.distance + this.FindShortestPath(grid, key.Value.point, foundKeys + key.Key);
                }

                // pick the shortest path to the final key
                result = possibilities.Values.Min();
            }

            Cache[cacheKey] = result;
            return result;
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
    }
}
