using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AdventOfCode.Utilities;
using MoreLinq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 18
    /// </summary>
    public class Day18
    {
        public enum Move { North = 1, South = 2, West = 3, East = 4 };

        private static readonly IDictionary<Move, Point2D> Deltas = new Dictionary<Move, Point2D>(4)
        {
            [Move.North] = (0, -1),
            [Move.South] = (0, 1),
            [Move.West] = (-1, 0),
            [Move.East] = (1, 0)
        };

        private static readonly Dictionary<(Point2D key, string collected), int> Cache = new Dictionary<(Point2D key, string collected), int>(100000);

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

            var graph = new Graph<Point2D>(Graph<Point2D>.ManhattanDistanceHeuristic);
            DiscoverMaze(graph, grid, start);

            var paths = Enumerable.Append(keys.Values, start)
                                  .ToDictionary(k => k, k => GetKeyTargets(graph, k, keys, doors));

            int shortest = CollectKeys(paths, start, string.Empty);

            return shortest;
        }

        public int Part2(string[] input)
        {
            foreach (string line in input)
            {
                throw new NotImplementedException("Part 2 not implemented");
            }

            return 0;
        }

        /// <summary>
        /// Discover the graph of the maze
        /// </summary>
        /// <param name="graph">Graph to populate</param>
        /// <param name="grid">Character grid</param>
        /// <param name="current">Current location</param>
        private static void DiscoverMaze(Graph<Point2D> graph, char[,] grid, Point2D current)
        {
            // try and go in each direction, and unwind after successful move attempt
            foreach (Move move in Deltas.Keys)
            {
                Point2D delta = Deltas[move];
                Point2D next = current + delta;

                if (graph.Vertices.ContainsKey(next))
                {
                    // visited
                    continue;
                }

                if (grid[next.Y, next.X] == '#')
                {
                    // wall
                    continue;
                }

                // add two-way vertex since we've not hit a wall
                graph.AddVertex(current, next);
                graph.AddVertex(next, current);

                // DFS
                DiscoverMaze(graph, grid, next);
            }
        }

        /// <summary>
        /// Get the shortest paths from a given location to all keys, noting which keys are required to take each path
        /// </summary>
        /// <param name="graph">Maze graph</param>
        /// <param name="start">Start location</param>
        /// <param name="keys">Key locations</param>
        /// <param name="doors">Door locations</param>
        /// <returns>Key to target keys lookup</returns>
        private static List<KeyTarget> GetKeyTargets(Graph<Point2D> graph, Point2D start, Dictionary<char, Point2D> keys, Dictionary<char, Point2D> doors)
        {
            var paths = new List<KeyTarget>(keys.Count);

            foreach (var key in keys)
            {
                var path = graph.GetShortestPath(start, key.Value).Select(p => p.node).ToHashSet();

                char[] requiredDoors = doors.Where(d => path.Contains(d.Value))
                                            .Select(d => d.Key.ToLower())
                                            .OrderBy(c => c)
                                            .ToArray();

                paths.Add(new KeyTarget(key.Key, key.Value, path.Count, new string(requiredDoors)));
            }

            return paths;
        }

        /// <summary>
        /// Collect remaining keys from the given start location
        /// </summary>
        /// <param name="paths">Lookup of key to other keys</param>
        /// <param name="start">Start location</param>
        /// <param name="haveKeys">Keys collected so far</param>
        /// <returns>Shortest path to collect all remaining keys</returns>
        private static int CollectKeys(IReadOnlyDictionary<Point2D, List<KeyTarget>> paths, Point2D start, string haveKeys)
        {
            var cacheKey = (start, new string(haveKeys.OrderBy(c => c).ToArray()));

            if (Cache.ContainsKey(cacheKey))
            {
                // already gone from this key whilst holding the current set of keys
                return Cache[cacheKey];
            }

            // don't visit already-collected keys or blocked paths
            var remainingKeys = paths[start].Where(p => !haveKeys.Contains(p.Id));
            var availableKeys = remainingKeys.Where(p => !p.RequiredKeys.Except(haveKeys).Any())
                                             .OrderBy(k => k.Distance)
                                             .ToArray();

            int result = 0;

            if (availableKeys.Any())
            {
                var possibilities = new Dictionary<char, int>(availableKeys.Length);

                foreach (var key in availableKeys)
                {
                    // branch out using DFS - note the name of the flippin' problem! Many worlds!
                    possibilities[key.Id] = key.Distance + CollectKeys(paths, key.Location, haveKeys + key.Id);
                }

                result = possibilities.Values.Min();
            }

            Debug.WriteLine($"{result}\t\t{start}\t\t{haveKeys}");

            Cache[cacheKey] = result;
            return result;
        }

        public class KeyTarget
        {
            public char Id { get; }
            public Point2D Location { get; }
            public int Distance { get; }
            public string RequiredKeys { get; }

            public KeyTarget(char id, Point2D location, int distance, string requiredKeys)
            {
                this.Id = id;
                this.Location = location;
                this.Distance = distance;
                this.RequiredKeys = requiredKeys;
            }

            public override string ToString()
            {
                return $"Id: {this.Id}, Location: {this.Location}, Distance: {this.Distance}, RequiredKeys: {this.RequiredKeys}";
            }
        }
    }
}
