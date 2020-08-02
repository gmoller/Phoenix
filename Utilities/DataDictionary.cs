using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Utilities
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class DataDictionary<T> : IEnumerable<T> where T : IIdentifiedById
    {
        protected readonly Dictionary<int, T> Items;

        protected DataDictionary()
        {
            Items = new Dictionary<int, T>();
        }

        private DataDictionary(List<T> items)
        {
            Items = new Dictionary<int, T>();
            foreach (var item in items)
            {
                Items.Add(item.Id, item);
            }
        }

        public static DataDictionary<T> Create(List<T> items)
        {
            return new DataDictionary<T>(items);
        }

        public static DataDictionary<T> Create(IEnumerable<T> items)
        {
            return new DataDictionary<T>(items.ToList());
        }

        public int Count => Items.Count;

        public bool Contains(int index)
        {
            return Items.ContainsKey(index);
        }

        public T this[int index]
        {
            get
            {
                if (!Items.ContainsKey(index))
                {
                    throw new IndexOutOfRangeException($"Index out of range. Item with index [{index}] not found.");
                }

                return Items[index];
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (KeyValuePair<int, T> item in Items)
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

        private string DebuggerDisplay => $"{{Count={Items.Count}}}";
    }
}