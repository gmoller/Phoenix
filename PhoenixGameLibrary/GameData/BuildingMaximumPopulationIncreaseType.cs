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
    public struct BuildingMaximumPopulationIncreaseType
    {
        public int BuildingId { get; }
        public int MaximumPopulationIncrease { get; }

        private BuildingMaximumPopulationIncreaseType(int buildingId, int maximumPopulationIncrease)
        {
            BuildingId = buildingId;
            MaximumPopulationIncrease = maximumPopulationIncrease;
        }

        public static BuildingMaximumPopulationIncreaseType Create(int buildingId, int populationGrowthRateIncrease)
        {
            return new BuildingMaximumPopulationIncreaseType(buildingId, populationGrowthRateIncrease);
        }

        private string DebuggerDisplay => $"{{BuildingId={BuildingId},MaximumPopulationIncrease={MaximumPopulationIncrease}}}";
    }

    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class BuildingMaximumPopulationIncreaseTypes : IEnumerable<BuildingMaximumPopulationIncreaseType>
    {
        private readonly Dictionary<int, BuildingMaximumPopulationIncreaseType> _items;

        private BuildingMaximumPopulationIncreaseTypes(List<BuildingMaximumPopulationIncreaseType> items)
        {
            _items = new Dictionary<int, BuildingMaximumPopulationIncreaseType>();
            foreach (var item in items)
            {
                _items.Add(item.BuildingId, item);
            }
        }

        public static BuildingMaximumPopulationIncreaseTypes Create(List<BuildingMaximumPopulationIncreaseType> items)
        {
            return new BuildingMaximumPopulationIncreaseTypes(items);
        }

        public static BuildingMaximumPopulationIncreaseTypes Create(IEnumerable<BuildingMaximumPopulationIncreaseType> items)
        {
            return new BuildingMaximumPopulationIncreaseTypes(items.ToList());
        }

        public int Count => _items.Count;

        public BuildingMaximumPopulationIncreaseType this[int index]
        {
            get
            {
                if (!_items.ContainsKey(index))
                {
                    throw new IndexOutOfRangeException($"Index out of range. BuildingMaximumPopulationIncreaseType with index [{index}] not found.");
                }

                return _items[index];
            }
        }

        public IEnumerator<BuildingMaximumPopulationIncreaseType> GetEnumerator()
        {
            foreach (KeyValuePair<int, BuildingMaximumPopulationIncreaseType> item in _items)
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

    public static class BuildingMaximumPopulationIncreaseTypesLoader
    {
        public static BuildingMaximumPopulationIncreaseTypes Load()
        {
            var buildingMaximumPopulationIncreaseTypes = new List<BuildingMaximumPopulationIncreaseType>
            {
                BuildingMaximumPopulationIncreaseType.Create(26, 2),
                BuildingMaximumPopulationIncreaseType.Create(27, 3)
            };

            return BuildingMaximumPopulationIncreaseTypes.Create(buildingMaximumPopulationIncreaseTypes);
        }
    }
}