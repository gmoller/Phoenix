using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace PhoenixGameLibrary.GameData
{
    /// <summary>
    /// This class is immutable.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class RaceTypes : IEnumerable<RaceType>
    {
        private readonly Dictionary<int, RaceType> _items;

        private RaceTypes(List<RaceType> items)
        {
            _items = new Dictionary<int, RaceType>();
            foreach (RaceType item in items)
            {
                _items.Add(item.Id, item);
            }
        }

        public static RaceTypes Create(List<RaceType> items)
        {
            return new RaceTypes(items);
        }

        public int Count => _items.Count;

        public RaceType this[int index]
        {
            get
            {
                if (index < 0 || index > _items.Count - 1)
                {
                    return RaceType.Invalid;
                }

                return _items[index];
            }
        }

        public RaceType this[string name]
        {
            get
            {
                foreach (RaceType item in this)
                {
                    if (item.Name == name)
                    {
                        return item;
                    }
                }

                throw new IndexOutOfRangeException($"Index out of range. RaceType with name [{name}] not found.");
            }
        }

        public IEnumerator<RaceType> GetEnumerator()
        {
            foreach (KeyValuePair<int, RaceType> item in _items)
            {
                yield return item.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private string DebuggerDisplay => $"{{Count={_items.Count}}}";
    }
}