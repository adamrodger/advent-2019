using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        private string[] input;
        private const int Offset = 21;

        public int Part1(string[] input)
        {
            this.input = input;

            var graph = new Graph<Point2D>(Graph<Point2D>.ManhattanDistanceHeuristic);
            var tiles = new Dictionary<Point2D, Tile>();

            // do a flood search, setting valid indices on the graph, use a new intcode VM each time with the chain
            DiscoverGrid(graph, tiles, new[] { (Move.North, new Point2D(Offset, Offset - 1)) });
            DiscoverGrid(graph, tiles, new[] { (Move.South, new Point2D(Offset, Offset + 1)) });
            DiscoverGrid(graph, tiles, new[] { (Move.West, new Point2D(Offset - 1, Offset)) });
            DiscoverGrid(graph, tiles, new[] { (Move.East, new Point2D(Offset + 1, Offset)) });

            if (tiles[(Offset, Offset - 1)] != Tile.Wall)
            {
                graph.AddVertex((Offset,Offset), (Offset, Offset - 1));
            }
            if (tiles[(Offset, Offset + 1)] != Tile.Wall)
            {
                graph.AddVertex((Offset, Offset), (Offset, Offset + 1));
            }
            if (tiles[(Offset - 1, Offset)] != Tile.Wall)
            {
                graph.AddVertex((Offset, Offset), (Offset - 1, Offset));
            }
            if (tiles[(Offset + 1, Offset)] != Tile.Wall)
            {
                graph.AddVertex((Offset, Offset), (Offset + 1, Offset));
            }

            char[,] grid = new char[Offset * 2, Offset * 2];
            grid.ForEach((x, y, c) => grid[y, x] = ' ');

            foreach (var tile in tiles)
            {
                grid[tile.Key.Y, tile.Key.X] = tile.Value == Tile.Wall ? '#'
                                             : tile.Value == Tile.Open ? '.'
                                             : 'X';
            }

            grid.Print();

            Point2D target = tiles.First(kvp => kvp.Value == Tile.Oxygen).Key;
            List<(Point2D node, int distance)> shortest = graph.GetShortestPath((Offset, Offset), target);

            return shortest.Count;
        }

        public void DiscoverGrid(Graph<Point2D> graph, Dictionary<Point2D, Tile> tiles, IList<(Move move, Point2D position)> path)
        {
            var current = path.Last();

            if (tiles.ContainsKey(current.position))
            {
                // already visited
                return;
            }

            // follow the path
            var vm = new IntCodeEmulator(this.input);

            foreach ((Move move, _) in path)
            {
                vm.StdIn.Enqueue((int)move);
            }

            while (vm.StdIn.Any())
            {
                vm.ExecuteUntilYield();
            }

            Debug.Assert(!vm.Halted);
            Debug.Assert(vm.StdOut.Any());

            // see what the result is
            Tile result = (Tile)(int)vm.StdOut.Last();
            tiles[current.position] = result;

            if (result == Tile.Wall)
            {
                return;
            }


            if (path.Count > 1)
            {
                // add valid vertex since we've not hit a wall
                var previous = path.ElementAt(path.Count - 2);
                graph.AddVertex(previous.position, current.position);
            }

            if (result == Tile.Open)
            {
                // recurse outwards
                DiscoverGrid(graph, tiles, path.Append((Move.North, new Point2D(current.position.X, current.position.Y - 1))).ToList());
                DiscoverGrid(graph, tiles, path.Append((Move.South, new Point2D(current.position.X, current.position.Y + 1))).ToList());
                DiscoverGrid(graph, tiles, path.Append((Move.West, new Point2D(current.position.X - 1, current.position.Y))).ToList());
                DiscoverGrid(graph, tiles, path.Append((Move.East, new Point2D(current.position.X + 1, current.position.Y))).ToList());
            }
        }

        public int Part2(string[] input)
        {
            foreach (string line in input)
            {
                throw new NotImplementedException("Part 2 not implemented");
            }

            return 0;
        }
    }
}
