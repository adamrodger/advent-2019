using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Utilities
{
    public static class Maze
    {
        public static Graph<Point2D> ToGraph(this char[,] maze, Point2D start, params char[] walls)
        {
            var graph = new Graph<Point2D>();
            var open = new Queue<Point2D>();
            var closed = new HashSet<Point2D>();

            open.Enqueue(start);

            while (open.Any())
            {
                Point2D current = open.Dequeue();

                // BFS
                foreach (Point2D destination in current.Adjacent4())
                {
                    if (closed.Contains(destination))
                    {
                        // already visited
                        continue;
                    }

                    closed.Add(current);

                    char c = maze[destination.Y, destination.X];

                    if (walls.Any(w => w == c))
                    {
                        continue;
                    }

                    graph.AddVertex(current, destination);
                    graph.AddVertex(destination, current);

                    open.Enqueue(destination);
                }
            }

            return graph;
        }
    }
}
