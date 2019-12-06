using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 6
    /// </summary>
    public class Day6
    {
        public int Part1(string[] input)
        {
            var nodes = input.Select(i => new Node(i)).ToDictionary(n => n.Id);

            return nodes.Values.Sum(n => n.PathToRoot(nodes).Count());
        }

        public int Part2(string[] input)
        {
            var nodes = input.Select(i => new Node(i)).ToDictionary(n => n.Id);

            Node you = nodes["YOU"];
            Node san = nodes["SAN"];

            var youPath = you.PathToRoot(nodes).ToList();
            var sanPath = san.PathToRoot(nodes).ToList();

            // subtracting the common nodes gives 2 shorter paths to a common ancestor
            var common = youPath.Intersect(sanPath).ToList();

            // -1 because the you/san nodes themselves don't count
            return (youPath.Count - common.Count - 1) + (sanPath.Count - common.Count - 1);
        }
    }

    public class Node
    {
        public string Id { get; }

        public string Parent { get; }

        public Node(string input)
        {
            var split = input.Split(')');
            this.Id = split[1];
            this.Parent = split[0];
        }

        public IEnumerable<Node> PathToRoot(IDictionary<string, Node> graph)
        {
            yield return this;

            string parent = this.Parent;

            while (graph.ContainsKey(parent))
            {
                yield return graph[parent];
                parent = graph[parent].Parent;
            }
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"{nameof(this.Id)}: {this.Id}, {nameof(this.Parent)}: {this.Parent}";
        }
    }
}
