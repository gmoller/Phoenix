using System.Collections.Generic;
using System.Diagnostics;

namespace PhoenixGameLibrary.GameData
{
    /// <summary>
    /// This struct is immutable.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public struct RaceType : IIdentifiedByIdAndName
    {
        public static readonly RaceType Invalid = new RaceType(-1, "None", 0.0f, 0, 0.0f, 0.0f);

        public int Id { get; }
        public string Name { get; }
        public float FarmingRate { get; }
        public int GrowthRateModifier { get; }
        public float WorkerProductionRate { get; }
        public float FarmerProductionRate { get; }

        private RaceType(int id, string name, float farmingRate, int growthRateModifier, float workerProductionRate, float farmerProductionRate)
        {
            Id = id;
            Name = name;
            FarmingRate = farmingRate;
            GrowthRateModifier = growthRateModifier;
            WorkerProductionRate = workerProductionRate;
            FarmerProductionRate = farmerProductionRate;
        }

        public static RaceType Create(int id, string name, float farmingRate, int growthRateModifier, float workerProductionRate, float farmerProductionRate)
        {
            return new RaceType(id, name, farmingRate, growthRateModifier, workerProductionRate, farmerProductionRate);
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Id={Id},Name={Name}}}";
    }

    public static class RaceTypesLoader
    {
        public static NamedDataList<RaceType> GetRaceTypes()
        {
            var raceTypes = new List<RaceType>
            {
                RaceType.Create(0, "Barbarians", 2.0f, 20, 2.0f, 0.5f),
                RaceType.Create(1, "Beastmen", 2.0f, 0, 2.0f, 0.5f),
                RaceType.Create(2, "Dark Elves", 2.0f, -20, 2.0f, 0.5f),
                RaceType.Create(3, "Draconians", 2.0f, -10, 2.0f, 0.5f),
                RaceType.Create(4, "Dwarves", 2.0f, -20, 3.0f, 0.5f),
                RaceType.Create(5, "Gnolls", 2.0f, -10, 2.0f, 0.5f),
                RaceType.Create(6, "Halflings", 3.0f, 0, 2.0f, 0.5f),
                RaceType.Create(7, "High Elves", 2.0f, -20, 2.0f, 0.5f),
                RaceType.Create(8, "High Men", 2.0f, 0, 2.0f, 0.5f),
                RaceType.Create(9, "Klackons", 2.0f, -10, 3.0f, 0.5f),
                RaceType.Create(10, "Lizardmen", 2.0f, 10, 2.0f, 0.5f),
                RaceType.Create(11, "Nomads", 2.0f, -10, 2.0f, 0.5f),
                RaceType.Create(12, "Orcs", 2.0f, 0, 2.0f, 0.5f),
                RaceType.Create(13, "Trolls", 2.0f, -20, 2.0f, 0.5f)
            };

            return NamedDataList<RaceType>.Create(raceTypes);
        }
    }
}