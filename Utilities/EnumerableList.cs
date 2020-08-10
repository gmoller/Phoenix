using System;
using System.Collections;
using System.Collections.Generic;

namespace Utilities
{
    public class EnumerableList<T> : IEnumerable<T>
    {
        private readonly List<T> _list;

        public T this[int index] => _list[index];

        public int Count => _list.Count;

        public EnumerableList(List<T> list)
        {
            _list = list;
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in _list)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Count={Count}}}";
    }
}