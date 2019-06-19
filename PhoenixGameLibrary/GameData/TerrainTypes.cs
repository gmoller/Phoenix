using System.Collections.Generic;
using System.Diagnostics;

namespace PhoenixGameLibrary.GameData
{
    /// <summary>
    /// This class is immutable.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class TerrainTypes
    {
        private readonly Dictionary<int, TerrainType> _items;

        private TerrainTypes(List<TerrainType> items)
        {
            _items = new Dictionary<int, TerrainType>();
            foreach (TerrainType item in items)
            {
                _items.Add(item.Id, item);
            }
        }

        public static TerrainTypes Create(List<TerrainType> items)
        {
            return new TerrainTypes(items);
        }

        public int Count => _items.Count;

        public TerrainType this[int index] => _items[index];

        private string DebuggerDisplay => $"{{Count={_items.Count}}}";
    }
}