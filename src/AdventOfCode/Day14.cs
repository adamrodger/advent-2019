using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 14
    /// </summary>
    public class Day14
    {
        public int Part1(string[] input)
        {
            var reactions = new Dictionary<string, Reaction>(input.Length);
            Dictionary<string, int> got = new Dictionary<string, int>(reactions.Count);
            Dictionary<string, int> needed = new Dictionary<string, int>(reactions.Count);

            foreach (string line in input)
            {
                MatchCollection matches = Regex.Matches(line, @"(\d+) ([A-Z]+)");
                (int quantity, string chemical)[] parsed = matches.Cast<Match>().Select(m => (int.Parse(m.Groups[1].Value), m.Groups[2].Value)).ToArray();

                (int quantity, string chemical) output = parsed.Last();

                reactions[output.chemical] = new Reaction
                {
                    Inputs = parsed.Take(parsed.Length - 1).ToArray(),
                    Output = output
                };

                needed[output.chemical] = 0;
                got[output.chemical] = 0;
            }

            got["ORE"] = 0;
            needed["ORE"] = 0;
            
            needed["FUEL"] = 1;

            // recursively work backwards from FUEL, adding required amounts of input for each output
            React("FUEL", reactions, got, needed);
            
            return needed["ORE"];

            // guessed 174729 -- too low
        }

        public void React(string chemical, Dictionary<string, Reaction> reactions, Dictionary<string, int> got, Dictionary<string, int> needed)
        {
            Reaction reaction = reactions[chemical];
            int multiplier = (needed[chemical] - got[chemical]) / reaction.Output.quantity;

            // react up some more of it
            got[chemical] += reaction.Output.quantity * multiplier;

            foreach ((int quantity, string chemical) input in reaction.Inputs)
            {
                needed[input.chemical] += input.quantity * multiplier;

                if (input.chemical != "ORE") // ORE has no inputs, don't follow backwards
                {
                    // follow the reaction backwards
                    React(input.chemical, reactions, got, needed);
                }
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

    public class Reaction
    {
        public ICollection<(int quantity, string chemical)> Inputs { get; set; }
        public (int quantity, string chemical) Output { get; set; }

        public override string ToString()
        {
            return $"{string.Join(", ", Inputs.Select(i => i.quantity + " " + i.chemical))} => {this.Output.quantity} {this.Output.chemical}";
        }
    }
}
