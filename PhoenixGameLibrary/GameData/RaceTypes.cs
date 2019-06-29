using System.Collections.Generic;
using System.Diagnostics;

namespace PhoenixGameLibrary.GameData
{
    /// <summary>
    /// This class is immutable.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class RaceTypes
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

        private string DebuggerDisplay => $"{{Count={_items.Count}}}";
    }
}