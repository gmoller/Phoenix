using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PhoenixGameLibrary.GameData
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class BuildingTypes : IEnumerable<BuildingType>
    {
        private readonly Dictionary<int, BuildingType> _items;

        private BuildingTypes(List<BuildingType> items)
        {
            _items = new Dictionary<int, BuildingType>();
            foreach (BuildingType item in items)
            {
                _items.Add(item.Id, item);
            }
        }

        public static BuildingTypes Create(List<BuildingType> items)
        {
            return new BuildingTypes(items);
        }

        public static BuildingTypes Create(IEnumerable<BuildingType> items)
        {
            return new BuildingTypes(items.ToList());
        }

        public int Count => _items.Count;

        public BuildingType this[int index]
        {
            get
            {
                if (index < 0 || index > _items.Count - 1)
                {
                    throw new IndexOutOfRangeException($"Index out of range. BuildingType with index [{index}] not found.");
                }

                return _items[index];
            }
        }

        public BuildingType this[string name]
        {
            get
            {
                foreach (BuildingType item in this)
                {
                    if (item.Name == name)
                    {
                        return item;
                    }
                }

                throw new IndexOutOfRangeException($"Index out of range. BuildingType with name [{name}] not found.");
            }
        }

        public IEnumerator<BuildingType> GetEnumerator()
        {
            foreach (KeyValuePair<int, BuildingType> item in _items)
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