using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PhoenixGameLibrary.GameData
{
    /// <summary>
    /// This struct is immutable.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public struct BuildingFoodOutputIncreaseType
    {
        public int BuildingId { get; }
        public int FoodOutputIncrease { get; }

        private BuildingFoodOutputIncreaseType(int buildingId, int foodOutputIncrease)
        {
            BuildingId = buildingId;
            FoodOutputIncrease = foodOutputIncrease;
        }

        public static BuildingFoodOutputIncreaseType Create(int buildingId, int foodOutputIncrease)
        {
            return new BuildingFoodOutputIncreaseType(buildingId, foodOutputIncrease);
        }

        private string DebuggerDisplay => $"{{BuildingId={BuildingId},FoodOutputIncrease={FoodOutputIncrease}}}";
    }

    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class BuildingFoodOutputIncreaseTypes : IEnumerable<BuildingFoodOutputIncreaseType>
    {
        private readonly Dictionary<int, BuildingFoodOutputIncreaseType> _items;

        private BuildingFoodOutputIncreaseTypes(List<BuildingFoodOutputIncreaseType> items)
        {
            _items = new Dictionary<int, BuildingFoodOutputIncreaseType>();
            foreach (var item in items)
            {
                _items.Add(item.BuildingId, item);
            }
        }

        public static BuildingFoodOutputIncreaseTypes Create(List<BuildingFoodOutputIncreaseType> items)
        {
            return new BuildingFoodOutputIncreaseTypes(items);
        }

        public static BuildingFoodOutputIncreaseTypes Create(IEnumerable<BuildingFoodOutputIncreaseType> items)
        {
            return new BuildingFoodOutputIncreaseTypes(items.ToList());
        }

        public int Count => _items.Count;

        public BuildingFoodOutputIncreaseType this[int index]
        {
            get
            {
                if (!_items.ContainsKey(index))
                {
                    throw new IndexOutOfRangeException($"Index out of range. BuildingFoodOutputIncreaseType with index [{index}] not found.");
                }

                return _items[index];
            }
        }

        public IEnumerator<BuildingFoodOutputIncreaseType> GetEnumerator()
        {
            foreach (KeyValuePair<int, BuildingFoodOutputIncreaseType> item in _items)
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

    public static class BuildingFoodOutputIncreaseTypesLoader
    {
        public static BuildingFoodOutputIncreaseTypes Load()
        {
            var buildingFoodOutputIncreaseTypes = new List<BuildingFoodOutputIncreaseType>
            {
                BuildingFoodOutputIncreaseType.Create(26, 2),
                BuildingFoodOutputIncreaseType.Create(27, 3),
                BuildingFoodOutputIncreaseType.Create(28, 2)
            };

            return BuildingFoodOutputIncreaseTypes.Create(buildingFoodOutputIncreaseTypes);
        }
    }
}