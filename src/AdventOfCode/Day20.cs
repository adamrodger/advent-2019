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
        private const int OuterDelta = 1;
        private const int InnerDelta = -1;

        public int Part1(string[] input)
        {
            return GetShortestPath(input, false);
        }

        public int Part2(string[] input)
        {
            return GetShortestPath(input, true);
        }

        /// <summary>
        /// Get the shortest path from AA to ZZ
        /// </summary>
        /// <param name="input">Puzzle input</param>
        /// <param name="layered">Whether the maze is layered or not</param>
        /// <returns>Shortest path</returns>
        private static int GetShortestPath(string[] input, bool layered)
        {
            char[,] grid = input.ToGrid();

            // find all the portals and join each end of them
            Dictionary<(string id, bool outer), Point2D> portals = FindPortals(grid);
            Dictionary<Point2D, Point3D> connected = ConnectPortals(portals, layered);

            // construct the graph
            Point3D start = portals[("AA", true)];
            Point3D end = portals[("ZZ", true)];

            Graph<Point3D> graph = BuildGraph(grid, connected, start, end);

            // shortest path between AA and ZZ
            List<(Point3D node, int distance)> path = graph.GetShortestPath(start, end);

            return path.Count;
        }

        /// <summary>
        /// Find all the portals on the grid
        /// </summary>
        /// <param name="grid">Maze grid</param>
        /// <returns>Map of (ID, outer) to location for each portal</returns>
        private static Dictionary<(string id, bool outer), Point2D> FindPortals(char[,] grid)
        {
            var portalIndex = new Dictionary<(string id, bool outer), Point2D>();

            grid.ForEach((x, y, c) =>
            {
                if (!char.IsUpper(c))
                {
                    return;
                }

                if (x + 1 > grid.GetLength(1) - 1 || y + 1 > grid.GetLength(0) - 1)
                {
                    // too close to the right or bottom sides to look for the other part of the ID
                    return;
                }

                // check if this is the first letter in a portal ID - i.e. there's another letter to the right or below
                Point2D right = (x + 1, y);
                Point2D below = (x, y + 1);

                bool isOuter = x < 2
                            || y < 2
                            || x >= grid.GetLength(1) - 2
                            || y >= grid.GetLength(0) - 2;

                if (char.IsUpper(grid[right.Y, right.X]))
                {
                    // left-to-right ID, so maze tile is either on the left or the right
                    string id = new string(new [] { c, grid[right.Y, right.X] });

                    Point2D mazeLeft = (x - 1, y);
                    Point2D mazeRight = (x + 2, y);

                    portalIndex[(id, isOuter)] = mazeLeft.X > 0 && grid[mazeLeft.Y, mazeLeft.X] == '.'
                        ? mazeLeft
                        : mazeRight;
                }
                else if (char.IsUpper(grid[below.Y, below.X]))
                {
                    // top-to-bottom ID, so maze tile is either above or below
                    string id = new string(new[] { c, grid[below.Y, below.X] });

                    Point2D mazeAbove = (x, y - 1);
                    Point2D mazeBelow = (x, y + 2);

                    portalIndex[(id, isOuter)] = mazeAbove.Y > 0 && grid[mazeAbove.Y, mazeAbove.X] == '.'
                        ? mazeAbove
                        : mazeBelow;
                }
            });

            return portalIndex;
        }

        /// <summary>
        /// Connect inner and outer portals together
        /// </summary>
        /// <param name="portals">Portal lookup</param>
        /// <param name="layered">Whether the map is layered (part 2) or not (part 1)</param>
        /// <returns>Portal jump points</returns>
        private static Dictionary<Point2D, Point3D> ConnectPortals(Dictionary<(string id, bool outer), Point2D> portals, bool layered)
        {
            var connected = new Dictionary<Point2D, Point3D>(portals.Count);

            foreach ((string id, _) in portals.Keys.Where(k => k.outer))
            {
                if (id == "AA" || id == "ZZ")
                {
                    // these have no warp point
                    continue;
                }

                Point2D outer = portals[(id, true)];
                Point2D inner = portals[(id, false)];

                connected[outer] = new Point3D(inner.X, inner.Y, layered ? InnerDelta : 0);
                connected[inner] = new Point3D(outer.X, outer.Y, layered ? OuterDelta : 0);
            }

            return connected;
        }

        /// <summary>
        /// Build the graph of the maze, taking into account the ability to jump through portals and layers
        /// </summary>
        /// <param name="grid">Maze grid</param>
        /// <param name="portals">Portals</param>
        /// <param name="start">Start point</param>
        /// <returns>Maze as a 3D graph of vertices</returns>
        private static Graph<Point3D> BuildGraph(char[,] grid, Dictionary<Point2D, Point3D> portals, Point3D start, Point3D end)
        {
            int recursionLimit = Math.Max(10, portals.Count / 2);

            var graph = new Graph<Point3D>();
            var todo = new Queue<Point3D>();
            var visited = new HashSet<Point3D>();

            todo.Enqueue(start);

            while (todo.Any())
            {
                Point3D current = todo.Dequeue();

                if (current == end)
                {
                    // reached destination
                    break;
                }

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
                        if (!portals.ContainsKey(current))
                        {
                            // portal has no other end (which AA and ZZ don't)
                            continue;
                        }

                        // update destination to the other end of the portal (which may involve changing layer in part 2)
                        var portal = portals[current];
                        destination = new Point3D(portal.X, portal.Y, portal.Z + current.Z);

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
