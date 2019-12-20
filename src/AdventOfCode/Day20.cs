using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Utilities;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 20
    /// </summary>
    public class Day20
    {
        private const int Outer = 1;
        private const int Inner = -1;

        public int Part1(string[] input)
        {
            char[,] grid = input.ToGrid();

            // find all the portals and index each end of them
            Dictionary<Point2D, Point3D> portals = FindPortals(grid, false);

            // construct the graph
            Point3D start = portals[("AA", 0)];
            Point3D end = portals[("ZZ", 0)];

            Graph<Point3D> graph = BuildGraph(grid, portals, start);

            // shortest path between AA and ZZ
            List<(Point3D node, int distance)> path = graph.GetShortestPath(start, end);

            return path.Count;
        }

        public int Part2(string[] input)
        {
            char[,] grid = input.ToGrid();

            // find all the portals and index each end of them
            Dictionary<(string, int), Point2D> portals = FindPortals(grid, true);

            // construct the graph
            Point3D start = portals[("AA", Outer)];
            Point3D end = portals[("ZZ", Outer)];

            Graph<Point3D> graph = BuildGraph(grid, portals, start);

            // shortest path between AA and ZZ
            List<(Point3D node, int distance)> path = graph.GetShortestPath(start, end);

            return path.Count;
        }

        /// <summary>
        /// Find all the portals on the grid
        /// </summary>
        /// <param name="grid">Maze grid</param>
        /// <param name="part2">Is part 2 running?</param>
        /// <returns>Map of (ID, layer delta) to location for each portal</returns>
        private static Dictionary<Point2D, Point3D> FindPortals(char[,] grid, bool part2)
        {
            var portals = new Dictionary<Point2D, Point3D>();

            grid.ForEach((x, y, c) =>
            {
                if (!char.IsUpper(c))
                {
                    return;
                }

                // check if this is the first letter in a portal ID - i.e. there's another letter to the right or below
                char right = grid[y, x + 1];
                char below = grid[y + 1, x];

                bool isOuter = x < 2
                            || y < 2
                            || x >= grid.GetLength(1) - 2
                            || y >= grid.GetLength(0) - 2;

                int delta = part2
                                ? isOuter ? Outer : Inner
                                : 0;

                if (char.IsUpper(right))
                {
                    // left-to-right ID, so maze tile is on the right
                    string id = new string(new [] { c, right });
                    portals[id] = (x + 2, y, delta);
                }
                else if (char.IsUpper(below))
                {
                    // top-to-bottom ID, so maze is below
                    string id = new string(new [] { c, below});
                    portals[id] = (x, y + 2, delta);
                }
            });

            return portals;
        }

        /// <summary>
        /// Build the graph of the maze, taking into account the ability to jump through portals and layers
        /// </summary>
        /// <param name="grid">Maze grid</param>
        /// <param name="portals">Portals</param>
        /// <param name="start">Start point</param>
        /// <returns>Maze as a 3D graph of vertices</returns>
        private static Graph<Point3D> BuildGraph(char[,] grid, Dictionary<Point2D, Point3D> portals, Point3D start)
        {
            int recursionLimit = Math.Max(10, (int)(portals.Count * 0.5));

            var graph = new Graph<Point3D>();
            var todo = new Queue<Point3D>();
            var visited = new HashSet<Point3D>();

            todo.Enqueue(start);

            while (todo.Any())
            {
                Point3D current = todo.Dequeue();

                foreach (Point3D next in current.Adjacent4())
                {
                    Point3D destination = next;

                    if (visited.Contains(destination))
                    {
                        continue;
                    }

                    visited.Add(current);

                    char c = grid[destination.Y, destination.X];

                    if (c == '#' || c == ' ')
                    {
                        continue;
                    }

                    if (char.IsUpper(c))
                    {
                        if (!portals.ContainsKey(destination))
                        {
                            // portal has no other end (which AA and ZZ don't)
                            continue;
                        }

                        // warp over to the other end of the portal and increase/decrease the layer if necessary
                        var portal = portals[destination];
                        destination = new Point3D(portal.X, portal.Y, portal.Z + destination.Z);

                        if (visited.Contains(destination) || destination.Z < 0 || destination.Z > recursionLimit)
                        {
                            // visited already or in a loop, sack it off
                            continue;
                        }
                    }

                    graph.AddVertex(current, destination);
                    graph.AddVertex(destination, current);

                    todo.Enqueue(destination);
                }
            }

            return graph;
        }
    }
}
