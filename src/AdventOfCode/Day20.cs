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
        private const int Outer = -1;
        private const int Inner = 1;

        public int Part1(string[] input)
        {
            throw new NotImplementedException("Doing part 2");
        }

        public int Part2(string[] input)
        {
            char[,] grid = input.ToGrid();

            // find all the teleporters and index each end of them
            Dictionary<(string, int), Point2D> teleporters = FindTeleporters(grid);

            // construct the graph
            Graph<Point3D> graph = BuildGraph(grid, teleporters);

            // shortest path between AA and ZZ
            List<(Point3D node, int distance)> path = graph.GetShortestPath(teleporters[("AA", Outer)], teleporters[("ZZ", Outer)]);

            Debug.Assert(path != null);

            return path.Count;
        }

        private static Dictionary<(string, int), Point2D> FindTeleporters(char[,] grid)
        {
            var teleporters = new Dictionary<(string, int), Point2D>();

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
                int delta = GetTransporterDelta(grid, start);

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

                teleporters[(id, delta)] = join;
            });

            return teleporters;
        }

        private static int GetTransporterDelta(char[,] grid, Point2D current)
        {
            return current.X < 2
                || current.Y < 2
                || current.X >= grid.GetLength(1) - 2
                || current.Y >= grid.GetLength(0) - 2
                       ? Outer
                       : Inner;
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

        private static Graph<Point3D> BuildGraph(char[,] grid, Dictionary<(string, int), Point2D> teleporters)
        {
            var graph = new Graph<Point3D>();

            var todo = new Queue<Point3D>();
            var visited = new HashSet<Point3D>();

            Point3D start = teleporters[("AA", Outer)];
            todo.Enqueue(start);

            while (todo.Any())
            {
                Point3D current = todo.Dequeue();

                foreach (Point3D next in current.Adjacent4())
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
                        int otherPortal = GetTransporterDelta(grid, next) * -1;

                        if (!teleporters.ContainsKey((id, otherPortal)))
                        {
                            continue;
                        }

                        // add the path to the other end of the teleporter and increase/decrease the layer
                        var teleport = teleporters[(id, otherPortal)];

                        var destination = new Point3D(teleport.X, teleport.Y, next.Z + otherPortal);

                        if (destination.Z > 0 || destination.Z < -150)
                        {
                            // in a loop, sack it off
                            continue;
                        }

                        // AA and ZZ are one-way so this would be default
                        if (teleport != default && !visited.Contains(destination))
                        {
                            graph.AddVertex(current, destination);
                            graph.AddVertex(destination, current);

                            todo.Enqueue(destination);
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
