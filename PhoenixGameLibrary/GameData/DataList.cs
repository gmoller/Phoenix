using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PhoenixGameLibrary.GameData
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class DataList<T> : IEnumerable<T> where T : IIdentifiedById
    {
        private readonly Dictionary<int, T> _items;

        private DataList(List<T> items)
        {
            _items = new Dictionary<int, T>();
            foreach (var item in items)
            {
                _items.Add(item.Id, item);
            }
        }

        public static DataList<T> Create(List<T> items)
        {
            return new DataList<T>(items);
        }

        public static DataList<T> Create(IEnumerable<T> items)
        {
            return new DataList<T>(items.ToList());
        }

        public int Count => _items.Count;

        public bool Contains(int index)
        {
            return _items.ContainsKey(index);
        }

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

    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class NamedDataList<T> : IEnumerable<T> where T : IIdentifiedByIdAndName
    {
        private readonly Dictionary<int, T> _items;

        private NamedDataList(List<T> items)
        {
            _items = new Dictionary<int, T>();
            foreach (var item in items)
            {
                _items.Add(item.Id, item);
            }
        }

        public static NamedDataList<T> Create(List<T> items)
        {
            return new NamedDataList<T>(items);
        }

        public static NamedDataList<T> Create(IEnumerable<T> items)
        {
            return new NamedDataList<T>(items.ToList());
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

    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class DataList2<T> : IEnumerable<T> where T : IIdentifiedById
    {
        private readonly List<T> _items;

        private DataList2()
        {
            _items = new List<T>();
        }

        private DataList2(List<T> items)
        {
            _items = new List<T>();
            foreach (var item in items)
            {
                _items.Add(item);
            }
        }

        public static DataList2<T> Create()
        {
            return new DataList2<T>();
        }

        public static DataList2<T> Create(List<T> items)
        {
            return new DataList2<T>(items);
        }

        public static DataList2<T> Create(IEnumerable<T> items)
        {
            return new DataList2<T>(items.ToList());
        }

        public int Count => _items.Count;

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= _items.Count)
                {
                    throw new IndexOutOfRangeException($"Index out of range. Item with index [{index}] not found.");
                }

                return _items[index];
            }
        }

        public void Add(T item)
        {
            _items.Add(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in _items)
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

        private string DebuggerDisplay => $"{{Count={_items.Count}}}";
    }
}