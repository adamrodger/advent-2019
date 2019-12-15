using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using AdventOfCode.IntCode;
using AdventOfCode.Utilities;

namespace AdventOfCode
{
    public enum Tile { Wall = 0, Open = 1, Oxygen = 2 };

    public enum Move { North = 1, South = 2, West = 3, East = 4 };

    /// <summary>
    /// Solver for Day 15
    /// </summary>
    public class Day15
    {
        private static readonly IDictionary<Move, Point2D> Deltas = new Dictionary<Move, Point2D>(4)
        {
            [Move.North] = (0, -1), [Move.South] = (0, 1), [Move.West] = (-1, 0), [Move.East] = (1, 0)
        };

        private static readonly IDictionary<Move, Move> Unwinds = new Dictionary<Move, Move>(4)
        {
            [Move.North] = Move.South, [Move.South] = Move.North, [Move.West] = Move.East, [Move.East] = Move.West
        };

        public int Part1(string[] input)
        {
            (Graph<Point2D> graph, Point2D oxygenTank) = this.BuildGraph(input);
            List<(Point2D node, int distance)> shortest = graph.GetShortestPath((0, 0), oxygenTank);

            return shortest.Count;
        }

        public int Part2(string[] input)
        {
            (Graph<Point2D> graph, Point2D oxygenTank) = this.BuildGraph(input);

            var open = graph.Vertices.Keys;

            // brute force search from every open space back to the oxygen tank and pick the longest
            var lengths = open.ToDictionary(tile => tile, tile => graph.GetShortestPath(tile, oxygenTank)?.Count);
            int max = lengths.Values.Where(v => v.HasValue).Select(v => v.Value).Max();

            return max;
        }

        /// <summary>
        /// Build the graph of the given input IntCode program
        /// </summary>
        /// <param name="input">IntCode program</param>
        /// <returns>Constructed graph and oxygen tank location</returns>
        private (Graph<Point2D> graph, Point2D target) BuildGraph(string[] input)
        {
            var graph = new Graph<Point2D>(Graph<Point2D>.ManhattanDistanceHeuristic);
            var tiles = new Dictionary<Point2D, Tile>();

            Point2D start = (0, 0);
            tiles[start] = Tile.Open;

            var vm = new IntCodeEmulator(input);
            vm.ExecuteUntilYield(); // advance to first input

            // DFS to create graph vertices
            DiscoverGrid(vm, graph, tiles, start);

            PrintTiles(tiles);

            Point2D target = tiles.First(kvp => kvp.Value == Tile.Oxygen).Key;

            return (graph, target);
        }

        /// <summary>
        /// Recursive depth-first search, exploring the maze until we hit dead ends or run out of places to go
        /// </summary>
        /// <param name="vm">IntCode VM</param>
        /// <param name="graph">Maze graph</param>
        /// <param name="tiles">Previously visited tiles</param>
        /// <param name="current">Current position</param>
        private static void DiscoverGrid(IntCodeEmulator vm, Graph<Point2D> graph, IDictionary<Point2D, Tile> tiles, Point2D current)
        {
            // try and go in each direction, and unwind after each attempt
            foreach (Move move in Deltas.Keys)
            {
                Point2D next = NextMove(current, move);

                if (tiles.ContainsKey(next))
                {
                    // already visited
                    continue;
                }

                Tile result = MakeMove(vm, move);
                tiles[next] = result;

                PrintTiles(tiles);

                if (result != Tile.Wall)
                {
                    // add two-way vertex since we've not hit a wall
                    graph.AddVertex(current, next);
                    graph.AddVertex(next, current);

                    // DFS until we hit a wall
                    DiscoverGrid(vm, graph, tiles, next);
                }

                // compensatory unwind back to current position
                Move unwind = Unwinds[move];
                MakeMove(vm, unwind);
            }
        }

        /// <summary>
        /// Get the next position after the given move
        /// </summary>
        /// <param name="current">Current position</param>
        /// <param name="move">Move direction</param>
        /// <returns>Next position</returns>
        private static Point2D NextMove(Point2D current, Move move)
        {
            Point2D delta = Deltas[move];
            Point2D next = current + delta;
            return next;
        }

        /// <summary>
        /// Try to move from the current point and report the result
        /// </summary>
        /// <param name="vm">IntCode VM</param>
        /// <param name="move">Move direction</param>
        /// <returns>Next position's tile type</returns>
        private static Tile MakeMove(IntCodeEmulator vm, Move move)
        {
            vm.StdIn.Enqueue((int)move);
            vm.ExecuteUntilYield();

            Debug.Assert(!vm.Halted);
            Debug.Assert(vm.StdOut.Count == 1);

            Tile result = (Tile)(int)vm.StdOut.Dequeue();

            return result;
        }

        /// <summary>
        /// Print the current state of the tiles
        /// </summary>
        /// <param name="tiles">Tiles to print</param>
        private static void PrintTiles(IDictionary<Point2D, Tile> tiles)
        {
            char[,] grid = new char[42, 42];
            grid.ForEach((x, y, c) => grid[y, x] = ' ');

            foreach (var tile in tiles)
            {
                grid[tile.Key.Y + 21, tile.Key.X + 21] = tile.Value switch
                {
                    Tile.Wall   => '#',
                    Tile.Open   => '.',
                    Tile.Oxygen => 'X',
                    _           => throw new ArgumentOutOfRangeException()
                };
            }

            grid.Print();
        }
    }
}
