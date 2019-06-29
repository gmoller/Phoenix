using System.Diagnostics;

namespace PhoenixGameLibrary.GameData
{
    /// <summary>
    /// This struct is immutable.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public struct RaceType
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
}