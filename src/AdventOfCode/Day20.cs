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

            // find all the portals and index each end of them
            Dictionary<(string, int), Point2D> portals = FindPortals(grid);

            // construct the graph
            Graph<Point3D> graph = BuildGraph(grid, portals);

            // shortest path between AA and ZZ
            List<(Point3D node, int distance)> path = graph.GetShortestPath(portals[("AA", Outer)], portals[("ZZ", Outer)]);

            Debug.Assert(path != null);

            return path.Count;
        }

        /// <summary>
        /// Find all the portals on the grid
        /// </summary>
        /// <param name="grid">Maze grid</param>
        /// <returns>Map of (ID, layer delta) to location for each portal</returns>
        private static Dictionary<(string, int), Point2D> FindPortals(char[,] grid)
        {
            var portals = new Dictionary<(string, int), Point2D>();

            grid.ForEach((x, y, c) =>
            {
                if (!char.IsLetter(c) || !char.IsUpper(c))
                {
                    // not a portal ID
                    return;
                }

                if (grid.Adjacent4(x, y).All(a => a != '.'))
                {
                    // wrong side of the ID, not connected to the maze
                    return;
                }

                Point2D start = (x, y);
                int delta = GetLayerDelta(grid, start);

                Point2D join = default;

                string id = GetPortalId(grid, start);

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

                portals[(id, delta)] = join;
            });

            return portals;
        }

        /// <summary>
        /// Get the delta for the portal at the current location
        /// </summary>
        /// <param name="grid">Maze grid</param>
        /// <param name="portal">Portal location</param>
        /// <returns>Layer delta for the portal</returns>
        private static int GetLayerDelta(char[,] grid, Point2D portal)
        {
            return portal.X < 2
                || portal.Y < 2
                || portal.X >= grid.GetLength(1) - 2
                || portal.Y >= grid.GetLength(0) - 2
                       ? Outer
                       : Inner;
        }

        /// <summary>
        /// Get the ID of the portal at the given location
        /// </summary>
        /// <param name="grid">Maze grid</param>
        /// <param name="portal">Portal location</param>
        /// <returns>Portal ID</returns>
        private static string GetPortalId(char[,] grid, Point2D portal)
        {
            foreach (Point2D other in portal.Adjacent4())
            {
                if (other.X < 0 || other.Y < 0 || other.X >= grid.GetLength(1) || other.Y >= grid.GetLength(0))
                {
                    // fell off the map
                    continue;
                }

                if (char.IsUpper(grid[other.Y, other.X]))
                {
                    // found the other part of the ID - this assumes there are no pairs! e.g. there isn't an XY and YX
                    char[] ordered = new[] { grid[portal.Y, portal.X], grid[other.Y, other.X] }.OrderBy(c => c).ToArray();
                    string id = new string(ordered);
                    return id;
                }
            }

            throw new InvalidOperationException($"Start is not a portal ID: {portal}");
        }

        /// <summary>
        /// Build the graph of the maze, taking into account the ability to jump through portals and layers
        /// </summary>
        /// <param name="grid">Maze grid</param>
        /// <param name="portals">Portals</param>
        /// <returns>Maze as a 3D graph of vertices</returns>
        private static Graph<Point3D> BuildGraph(char[,] grid, Dictionary<(string, int), Point2D> portals)
        {
            var graph = new Graph<Point3D>();
            var todo = new Queue<Point3D>();
            var visited = new HashSet<Point3D>();

            Point3D start = portals[("AA", Outer)];
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
                        string id = GetPortalId(grid, next);
                        int layerDelta = GetLayerDelta(grid, next) * -1;

                        if (!portals.ContainsKey((id, layerDelta)))
                        {
                            // portal has no other end (which AA and ZZ don't)
                            continue;
                        }

                        // add the path to the other end of the teleporter and increase/decrease the layer
                        var teleport = portals[(id, layerDelta)];

                        var destination = new Point3D(teleport.X, teleport.Y, next.Z + layerDelta);

                        if (destination.Z > 0 || destination.Z < -25)
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

                    graph.AddVertex(current, next);
                    graph.AddVertex(next, current);

                    todo.Enqueue(next);
                }
            }

            return graph;
        }
    }
}
