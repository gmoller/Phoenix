using System.Collections.Generic;

namespace Utilities
{
    public class Graph<T>
    {
        // NameValueCollection would be a reasonable alternative here, if
        // you're always using string location types
        public Dictionary<T, T[]> Edges = new Dictionary<T, T[]>();

        public T[] Neighbors(T id)
        {
            return Edges[id];
        }
    }
}