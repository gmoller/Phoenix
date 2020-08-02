using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Utilities
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class NamedDataDictionary<T> : IEnumerable<T> where T : IIdentifiedByIdAndName
    {
        private readonly Dictionary<int, T> _items;

        private NamedDataDictionary(List<T> items)
        {
            _items = new Dictionary<int, T>();
            foreach (var item in items)
            {
                _items.Add(item.Id, item);
            }
        }

        public static NamedDataDictionary<T> Create(List<T> items)
        {
            return new NamedDataDictionary<T>(items);
        }

        public static NamedDataDictionary<T> Create(IEnumerable<T> items)
        {
            return new NamedDataDictionary<T>(items.ToList());
        }

        public int Count => _items.Count;

        public T this[int index]
        {
            get
            {
                if (!_items.ContainsKey(index))
                {
                    throw new IndexOutOfRangeException($"Index out of range. Item with index [{index}] not found.");
                }

                return _items[index];
            }
        }

        public T this[string name]
        {
            get
            {
                foreach (var item in this)
                {
                    if (item.Name == name)
                    {
                        return item;
                    }
                }

                throw new IndexOutOfRangeException($"Index out of range. Item with name [{name}] not found.");
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (KeyValuePair<int, T> item in _items)
            {
                yield return item.Value;
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

        private string DebuggerDisplay => $"{{Count={_items.Count}}}";
    }
}