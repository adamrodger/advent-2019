namespace AdventOfCode.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MoreLinq;

    /// <summary>
    /// A generic graph of nodes with edges between them
    /// </summary>
    /// <typeparam name="TNode">Type of the node</typeparam>
    public class Graph<TNode>
    {
        /// <summary>
        /// Nodes in the graph
        /// </summary>
        public Dictionary<TNode, List<(TNode node, int cost)>> Vertices { get; }

        /// <summary>
        /// Additional heuristic for estimating the remaining distance from a node to the destination (turns Dijkstra into A*)
        /// </summary>
        private readonly Func<TNode, TNode, int> heuristic;

        /// <summary>
        /// Initialises a new instance of the <see cref="Graph{TNode}"/> class.
        /// </summary>
        /// <param name="heuristic">(Optional) Additional heuristic for estimating the remaining distance from a node to the destination</param>
        public Graph(Func<TNode, TNode, int> heuristic = null)
        {
            this.Vertices = new Dictionary<TNode, List<(TNode next, int cost)>>();
            this.heuristic = heuristic ?? ((_, __) => 0);
        }

        /// <summary>
        /// Create an edge from start to end with the given cost (default to 1)
        /// </summary>
        /// <param name="start">Start node</param>
        /// <param name="end">End node</param>
        /// <param name="cost">Cost of moving from start to end (default to 1)</param>
        public void AddVertex(TNode start, TNode end, int cost = 1)
        {
            this.Vertices.GetOrCreate(start).Add((end, cost));
        }

        /// <summary>
        /// Find the shortest path from the start node to the end node
        /// </summary>
        /// <param name="start">Start node</param>
        /// <param name="finish">End node</param>
        /// <returns>Shortest path and cost of that path</returns>
        /// <remarks>
        /// Theory from https://web.archive.org/web/20170509000025/http://www.policyalmanac.org:80/games/aStarTutorial.htm
        /// </remarks>
        public List<(TNode node, int distance)> GetShortestPath(TNode start, TNode finish)
        {
            var parents = new Dictionary<TNode, TNode>();
            var distances = new Dictionary<TNode, int> { [start] = 0 }; // 'G' of each node, in A* nomenclature
            var open = new HashSet<TNode> { start };
            var closed = new HashSet<TNode>();

            while (open.Any())
            {
                // sort nodes by current distance plus the estimated distance to the destination
                TNode current = open.MinBy(node => distances[node] + this.heuristic(node, finish)).First();
                open.Remove(current);

                if (closed.Contains(current))
                {
                    // don't revisit
                    continue;
                }

                closed.Add(current);

                // check if we've reached destination
                if (current.Equals(finish))
                {
                    var path = new List<(TNode node, int distance)>();

                    while (parents.ContainsKey(current))
                    {
                        path.Add((current, distances[current]));
                        current = parents[current];
                    }

                    path.Reverse();
                    return path;
                }

                // walk outwards along edges to see if we can find a closer node
                foreach ((TNode next, int cost) in this.Vertices[current])
                {
                    if (closed.Contains(next))
                    {
                        continue;
                    }

                    var newDistance = distances[current] + cost;

                    // found the first or a closer route to the adjacent node (from the current node)
                    if (!distances.ContainsKey(next) || newDistance < distances[next])
                    {
                        distances[next] = newDistance;
                        parents[next] = current;

                        open.Add(next);
                    }
                }
            }

            // no path to destination
            return null;
        }
    }
}