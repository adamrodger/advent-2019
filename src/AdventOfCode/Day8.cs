using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Utilities;
using MoreLinq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 8
    /// </summary>
    public class Day8
    {
        private const int Width = 25;
        private const int Height = 6;

        public int Part1(string[] input)
        {
            string image = input[0];
            char[] minLayer = image.Batch(Width * Height)
                                   .MinBy(layer => layer.Count(c => c == '0'))
                                   .First()
                                   .ToArray();

            return minLayer.Count(c => c == '1') * minLayer.Count(c => c == '2');
        }

        public string Part2(string[] input)
        {
            string image = input[0];
            char[,] output = new char[Height, Width];

            string[] layers = image.Batch(Width * Height).Select(b => new string(b.ToArray())).ToArray();

            // 3d representation of (z,y,x) of each pixel in each layer/row/column
            List<List<char[]>> layerRows = layers.Select(layer => layer.Batch(Width)
                                                                       .Select(b => b.ToArray())
                                                                       .ToList())
                                                 .ToList();

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    var colour = layerRows.Select(l => l[y][x]).First(c => c != '2');
                    output[y, x] = colour == '1' ? 'B' : ' ';
                }
            }

            return output.Print();
        }
    }
}
