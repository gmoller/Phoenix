using System;
using System.Collections.Generic;

namespace Utilities
{
    public class BreadthFirstSearch
    {
        public static void Search(Graph<string> graph, string start)
        {
            var frontier = new Queue<string>();
            frontier.Enqueue(start);

            var visited = new HashSet<string> { start };

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();

                Console.WriteLine("Visiting {0}", current);
                foreach (var next in graph.Neighbors(current))
                {
                    if (visited.Contains(next)) continue;
                    frontier.Enqueue(next);
                    visited.Add(next);
                }
            }
        }
    }
}