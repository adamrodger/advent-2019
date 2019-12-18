using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
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

        public int Part1(string[] input)
        {
            var grid = input.ToGrid();
            var graph = new Graph<Point2D>(Graph<Point2D>.ManhattanDistanceHeuristic);

            /*int height = input.Length / 2;
            int width = input[0].Length / 2;
            Point2D start = (height, width);*/
            Point2D start = grid.First(c => c == '@');

            BuildGraph(graph, grid, start);

            int sum = 0;

            var keys = Enumerable.Range('a', 26)
                .Select(i => (char)i)
                .ToDictionary(k => k, k => grid.First(c => c == k));

            var doors = Enumerable.Range('A', 26)
                .Select(i => (char)i)
                .ToDictionary(d => d, d => grid.First(c => c == d));

            /*while (keys.Any())
            {
                // find the nearest key which isn't behind a door and do that one
                var key = keys.MinBy(k =>
                {
                    var shortest = graph.GetShortestPath(start, k.Value);

                    bool hitDoor = shortest.Any(s => doors.ContainsValue(s.node)); // check there's no door in the way
                    bool hitOtherKey = shortest.Any(s => s.node != start && s.node != k.Value && keys.ContainsValue(s.node));

                    return hitDoor || hitOtherKey ? int.MaxValue : shortest.Count;
                }).First();

                var path = graph.GetShortestPath(start, key.Value);
                sum += path.Count;

                // visited
                keys.Remove(key.Key);
                doors.Remove(key.Key.ToString().ToUpper()[0]);

                start = key.Value;
            }

            return sum;
            */

            int shortest = Branch(graph, keys, doors, start, new List<char>(), 0);
            return shortest;

            // 9126 -- wrong -- started in wrong place (41,41)
            // 9084 -- wrong -- started in right place (40,40), do keys a-z
            // 9658 -- wrong -- can't remember what I tried on this one
            // 3130 -- wrong -- always pick the closest other key
            // 6186 -- wrong -- pick the closest other key which isn't behind a door
            // missing the trick here - need to somehow branch and pick the 'overall' best path instead of min-maxing or max-minning
            // 5588 -- wrong -- make sure there's also not another key in the way otherwise that one is shorter
        }

        public int Part2(string[] input)
        {
            foreach (string line in input)
            {
                throw new NotImplementedException("Part 2 not implemented");
            }

            return 0;
        }

        private static void BuildGraph(Graph<Point2D> graph, char[,] grid, Point2D current)
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
                BuildGraph(graph, grid, next);
            }
        }

        private static int Branch(Graph<Point2D> graph, Dictionary<char, Point2D> keys, Dictionary<char, Point2D> doors, Point2D start, List<char> followed, int distance)
        {
            if (!keys.Any())
            {
                // finished
                return distance;
            }

            // find the keys you can get to (i.e. without hitting a door)
            KeyValuePair<char, Point2D>[] availableKeys = keys.Where(k => !followed.Contains(k.Key))
                                                              .Where(k =>
                                                              {
                                                                  var path = graph.GetShortestPath(start, k.Value);
                                                                  return path != null && !path.Any(p => doors.ContainsValue(p.node));
                                                              })
                                                              .ToArray();

            if (!availableKeys.Any())
            {
                Debug.WriteLine($"{Thread.CurrentThread.Name} - {distance} == {string.Join(" -> ", followed)}");
                return distance;
            }

            int shortest = int.MaxValue;

            // branch on each available key with a DFS
            foreach (KeyValuePair<char, Point2D> key in availableKeys.AsParallel())
            {
                // work out which keys/doors are yet to be opened - this is slow but we need to clone the dict minus this one key/door
                var remainingKeys = keys.Where(k => k.Key != key.Key).ToDictionary(k => k.Key, k => k.Value);
                var remainingDoors = doors.Where(k => k.Key != key.Key.ToUpper()).ToDictionary(k => k.Key, k => k.Value);
                var newFollowed = Enumerable.Append(followed, key.Key).ToList();

                // keep track of distance so far
                var path = graph.GetShortestPath(start, key.Value);
                var newDistance = distance + path.Count;

                //Debug.WriteLine($"{distance} + {path.Count} == {newDistance}: {string.Join(" -> ", newFollowed)}");

                // branch out - note the name of the flippin' problem! Many worlds!
                var branchlength = Branch(graph, remainingKeys, remainingDoors, key.Value, newFollowed, newDistance);

                // decide which branch was the shortest one
                if (branchlength < shortest)
                {
                    shortest = branchlength;
                }
            }

            return shortest;
        }
    }
}
