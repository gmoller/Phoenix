using System;
using System.Collections.Generic;

namespace Utilities
{
    public class PriorityQueue<T> where T : IComparable<T>
    {
        private readonly List<T> _data;

        public PriorityQueue()
        {
            _data = new List<T>();
        }

        public void Enqueue(T item)
        {
            _data.Add(item);
            var ci = _data.Count - 1;
            while (ci > 0)
            {
                var pi = (ci - 1) / 2;
                if (_data[ci].CompareTo(_data[pi]) >= 0)  break;
                var tmp = _data[ci];
                _data[ci] = _data[pi];
                _data[pi] = tmp;
                ci = pi;
            }
        }

        public T Dequeue()
        {
            // Assumes pq isn't empty
            var li = _data.Count - 1;
            var frontItem = _data[0];
            _data[0] = _data[li];
            _data.RemoveAt(li);

            --li;
            var pi = 0;
            while (true)
            {
                var ci = pi * 2 + 1;
                if (ci > li) break;
                var rc = ci + 1;
                if (rc <= li && _data[rc].CompareTo(_data[ci]) < 0)
                {
                    ci = rc;
                }

                if (_data[pi].CompareTo(_data[ci]) <= 0) break;
                var tmp = _data[pi];
                _data[pi] = _data[ci];
                _data[ci] = tmp;
                pi = ci;
            }

            return frontItem;
        }

        public T Peek()
        {
            var frontItem = _data[0];

            return frontItem;
        }

        public int Count()
        {
            return _data.Count;
        }

        public bool IsConsistent()
        {
            if (_data.Count == 0) return true;
            var li = _data.Count - 1; // last index
            for (var pi = 0; pi < _data.Count; ++pi) // each parent index
            {
                var lci = 2 * pi + 1; // left child index
                var rci = 2 * pi + 2; // right child index
                if (lci <= li && _data[pi].CompareTo(_data[lci]) > 0) return false;
                if (rci <= li && _data[pi].CompareTo(_data[rci]) > 0) return false;
            }

            return true; // Passed all checks
        }

        public override string ToString()
        {
            var s = string.Empty;
            for (var i = 0; i < _data.Count; ++i)
            {
                s += $"{_data[i]} ";
            }

            s += $"count = {_data.Count}";

            return s;
        }
    }
}