using System;
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
        public int Part1(string[] input)
        {
            string image = input[0];
            int minZeroes = int.MaxValue;
            int result = int.MinValue;

            for (int i = 0; i < image.Length; i += 25 * 6)
            {
                string layer = new string(image.Skip(i).Take(25 * 6).ToArray());

                int zeroes = layer.Count(l => l == '0');

                if (zeroes < minZeroes)
                {
                    minZeroes = zeroes;
                    result = layer.Count(l => l == '1') * layer.Count(l => l == '2');
                }
            }

            return result;
        }

        public int Part2(string[] input)
        {
            string image = input[0];

            string[] layers = image.Batch(25 * 6).Select(b => new string(b.ToArray())).ToArray();
            List<List<string>> layerRows = new List<List<string>>(layers.Length);
            char[,] output = new char[6,25];

            foreach (string layer in layers)
            {
                var rows = layer.Batch(25).Select(b => new string(b.ToArray())).ToList();
                layerRows.Add(rows);
            }

            for (int x = 0; x < 25; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    var colour = layerRows.Select(l => l[y][x]).First(c => c != '2') == '1' ? 'B' : ' ';
                    output[y, x] = colour;
                }
            }

            output.Print();

            return int.MinValue;
        }

        private static string[] BuildRows(string layer)
        {   
            string[] rows = new string[6];

            for (int j = 0; j < layer.Length; j += 6)
            {
                string row = new string(layer.Skip(j).Take(6).ToArray());
                rows[j % 6] = row;
            }

            return rows;
        }
    }
}
