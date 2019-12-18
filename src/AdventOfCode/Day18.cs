﻿using System;
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
                        keys.Add(c, (x,y));
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
            BuildGraph(graph, grid, start);

            // calculate the path from each key to each other key, and also from origin
            var points = Enumerable.Append(keys.Values, start).ToList();
            var distances = points.Cartesian(points, (p1, p2) => (start: p1, end: p2))
                                  .ToDictionary(k => k, pair => graph.GetShortestPath(pair.start, pair.end));

            int shortest = Branch(distances, keys, doors, start, string.Empty, 0);
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
        private static int Branch(Dictionary<(Point2D start, Point2D end), List<(Point2D node, int distance)>> paths,
                                  Dictionary<char, Point2D> keys,
                                  Dictionary<char, Point2D> doors,
                                  Point2D start,
                                  string followed,
                                  int distance)
        {
            // find the keys you can get to (i.e. without hitting a locked door)
            KeyValuePair<char, Point2D>[] availableKeys = keys.Where(k => !followed.Contains(k.Key))
                                                              .Where(k =>
                                                              {
                                                                  var path = paths[(start, k.Value)];
                                                                  return path != null && !path.Any(p => doors.ContainsValue(p.node)); // not behind a locked door
                                                              })
                                                              .ToArray();

            if (!availableKeys.Any())
            {
                Debug.WriteLine($"{followed} == {distance}");
                return distance;
            }

            int shortest = int.MaxValue;

            // branch on each available key with a DFS
            foreach (KeyValuePair<char, Point2D> key in availableKeys)
            {
                // work out which keys/doors are yet to be opened - this is slow but we need to clone the dict minus this one key/door
                //var remainingKeys = keys.Where(k => k.Key != key.Key).ToDictionary(k => k.Key, k => k.Value);
                var remainingDoors = doors.Where(k => k.Key != key.Key.ToUpper()).ToDictionary(k => k.Key, k => k.Value);
                var newFollowed = followed + key.Key;

                // keep track of distance so far
                var path = paths[(start, key.Value)];
                var newDistance = distance + path.Count;

                // branch out - note the name of the flippin' problem! Many worlds!
                int branchLength = Branch(paths, keys, remainingDoors, key.Value, newFollowed, newDistance);

                // decide which branch was the shortest one
                if (branchLength < shortest)
                {
                    shortest = branchLength;
                }
            }

            return shortest;
        }
    }
}