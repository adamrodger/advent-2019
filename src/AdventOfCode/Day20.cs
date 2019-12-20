using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AdventOfCode.Utilities;
using MoreLinq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 20
    /// </summary>
    public class Day20
    {
        public int Part1(string[] input)
        {
            char[,] grid = input.ToGrid();

            // find all the teleporters and index each end of them
            Dictionary<string, HashSet<Point2D>> teleporters = FindTeleporters(grid);

            // construct the graph
            Graph<Point2D> graph = BuildGraph(grid, teleporters);

            // shortest path between AA and ZZ
            List<(Point2D node, int distance)> path = graph.GetShortestPath(teleporters["AA"].First(), teleporters["ZZ"].First());

            return path.Count;
        }

        public int Part2(string[] input)
        {
            foreach (string line in input)
            {
                throw new NotImplementedException("Part 2 not implemented");
            }

            return 0;
        }

        private static Dictionary<string, HashSet<Point2D>> FindTeleporters(char[,] grid)
        {
            var teleporters = new Dictionary<string, HashSet<Point2D>>();

            grid.ForEach((x, y, c) =>
            {
                if (!char.IsLetter(c) || !char.IsUpper(c))
                {
                    // not a teleporter ID
                    return;
                }

                if (grid.Adjacent4(x, y).All(a => a != '.'))
                {
                    // wrong side of the ID, not connected to the maze
                    return;
                }

                Point2D start = (x, y);
                Point2D join = default;

                string id = GetTeleporterId(grid, start);

                // find the point connected to the maze
                foreach (Point2D other in start.Adjacent4())
                {
                    if (grid[other.Y, other.X] == '.')
                    {
                        join = other;
                        break;
                    }
                }

                Debug.Assert(join != default);

                teleporters.GetOrCreate(id).Add(join);
            });

            return teleporters;
        }

        private static string GetTeleporterId(char[,] grid, Point2D start)
        {
            foreach (Point2D other in start.Adjacent4())
            {
                if (other.X < 0 || other.Y < 0 || other.X >= grid.GetLength(1) || other.Y >= grid.GetLength(0))
                {
                    // fell off the map
                    continue;
                }

                if (char.IsUpper(grid[other.Y, other.X]))
                {
                    // found the other part of the ID
                    char[] ordered = new[] { grid[start.Y, start.X], grid[other.Y, other.X] }.OrderBy(c => c).ToArray();
                    string id = new string(ordered);
                    return id;
                }
            }

            throw new InvalidOperationException($"Start is not a teleporter ID: {start}");
        }

        private static Graph<Point2D> BuildGraph(char[,] grid, Dictionary<string, HashSet<Point2D>> teleporters)
        {
            var graph = new Graph<Point2D>();

            Queue<Point2D> todo = new Queue<Point2D>();
            HashSet<Point2D> visited = new HashSet<Point2D>();

            Point2D start = teleporters["AA"].First();
            todo.Enqueue(start);

            while (todo.Any())
            {
                var current = todo.Dequeue();

                foreach (Point2D next in current.Adjacent4())
                {
                    if (visited.Contains(next))
                    {
                        continue;
                    }

                    visited.Add(current);

                    char c = grid[next.Y, next.X];

                    if (c == '#' || c == ' ')
                    {
                        continue;
                    }

                    if (char.IsUpper(c))
                    {
                        string id = GetTeleporterId(grid, next);

                        // add the path to the other end of the teleporter
                        var teleport = teleporters[id].FirstOrDefault(t => t != current);

                        // AA and ZZ are one-way so this would be default
                        if (teleport != default && !visited.Contains(teleport))
                        {
                            graph.AddVertex(current, teleport);
                            graph.AddVertex(teleport, current);

                            todo.Enqueue(teleport);
                        }

                        // don't walk outwards, no point
                        continue;
                    }

                    Debug.Assert(c == '.');

                    graph.AddVertex(current, next);
                    graph.AddVertex(next, current);

                    todo.Enqueue(next);
                }
            }

            return graph;
        }
    }
}
