﻿using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using Zen.Utilities;

namespace PhoenixGameLibrary.GameData
{
    /// <summary>
    /// This struct is immutable.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public readonly struct BuildingFoodOutputIncreaseType : IIdentifiedById
    {
        #region State
        public int Id { get; }
        public int BuildingId { get; }
        public float FoodOutputIncrease { get; }
        #endregion End State

        private BuildingFoodOutputIncreaseType(int id, int buildingId, float foodOutputIncrease)
        {
            Id = id;
            BuildingId = buildingId;
            FoodOutputIncrease = foodOutputIncrease;
        }

        public static BuildingFoodOutputIncreaseType Create(int id, int buildingId, float foodOutputIncrease)
        {
            return new BuildingFoodOutputIncreaseType(id, buildingId, foodOutputIncrease);
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Id={Id},BuildingId={BuildingId},FoodOutputIncrease={FoodOutputIncrease}}}";
    }

    public struct BuildingFoodOutputIncreaseTypeForDeserialization
    {
        public int Id { get; set; }
        public string Building { get; set; }
        public float FoodOutputIncrease { get; set; }
    }

    public static class BuildingFoodOutputIncreaseTypesLoader
    {
        public static DataDictionary<BuildingFoodOutputIncreaseType> Load()
        {
            var buildingFoodOutputIncreaseTypes = new List<BuildingFoodOutputIncreaseType>
            {
                BuildingFoodOutputIncreaseType.Create(1, 26, 2.0f),
                BuildingFoodOutputIncreaseType.Create(2, 27, 3.0f),
                BuildingFoodOutputIncreaseType.Create(3, 28, 2.0f)
            };

            return DataDictionary<BuildingFoodOutputIncreaseType>.Create(buildingFoodOutputIncreaseTypes);
        }

        public static List<BuildingFoodOutputIncreaseType> LoadFromJsonFile(string fileName, NamedDataDictionary<BuildingType> buildingTypes)
        {
            var jsonString = File.ReadAllText($@".\Content\GameMetadata\{fileName}.json");
            var deserialized = JsonSerializer.Deserialize<List<BuildingFoodOutputIncreaseTypeForDeserialization>>(jsonString);
            var list = new List<BuildingFoodOutputIncreaseType>();
            foreach (var item in deserialized)
            {
                list.Add(BuildingFoodOutputIncreaseType.Create(item.Id, buildingTypes[item.Building].Id, item.FoodOutputIncrease));
            }

            return list;
        }
    }
}