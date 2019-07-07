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
    public struct BuildingPopulationGrowthType
    {
        public int BuildingId { get; }
        public int PopulationGrowthRateIncrease { get; }

        private BuildingPopulationGrowthType(int buildingId, int populationGrowthRateIncrease)
        {
            BuildingId = buildingId;
            PopulationGrowthRateIncrease = populationGrowthRateIncrease;
        }

        public static BuildingPopulationGrowthType Create(int buildingId, int populationGrowthRateIncrease)
        {
            return new BuildingPopulationGrowthType(buildingId, populationGrowthRateIncrease);
        }

        private string DebuggerDisplay => $"{{BuildingId={BuildingId},PopulationGrowthRateIncrease={PopulationGrowthRateIncrease}}}";
    }

    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class BuildingPopulationGrowthTypes : IEnumerable<BuildingPopulationGrowthType>
    {
        private readonly Dictionary<int, BuildingPopulationGrowthType> _items;

        private BuildingPopulationGrowthTypes(List<BuildingPopulationGrowthType> items)
        {
            _items = new Dictionary<int, BuildingPopulationGrowthType>();
            foreach (var item in items)
            {
                _items.Add(item.BuildingId, item);
            }
        }

        public static BuildingPopulationGrowthTypes Create(List<BuildingPopulationGrowthType> items)
        {
            return new BuildingPopulationGrowthTypes(items);
        }

        public static BuildingPopulationGrowthTypes Create(IEnumerable<BuildingPopulationGrowthType> items)
        {
            return new BuildingPopulationGrowthTypes(items.ToList());
        }

        public int Count => _items.Count;

        public BuildingPopulationGrowthType this[int index]
        {
            get
            {
                if (!_items.ContainsKey(index))
                {
                    throw new IndexOutOfRangeException($"Index out of range. BuildingPopulationGrowthType with index [{index}] not found.");
                }

                return _items[index];
            }
        }

        public IEnumerator<BuildingPopulationGrowthType> GetEnumerator()
        {
            foreach (KeyValuePair<int, BuildingPopulationGrowthType> item in _items)
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

    public static class BuildingPopulationGrowthTypesLoader
    {
        public static BuildingPopulationGrowthTypes Load()
        {
            var buildingPopulationGrowthTypes = new List<BuildingPopulationGrowthType>
            {
                BuildingPopulationGrowthType.Create(26, 20),
                BuildingPopulationGrowthType.Create(27, 30)
            };

            return BuildingPopulationGrowthTypes.Create(buildingPopulationGrowthTypes);
        }
    }
}