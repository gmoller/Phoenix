using System;
using PhoenixGameLibrary;
using PhoenixGameLibrary.GameData;

namespace GameLogic
{
    public sealed class Globals
    {
        private static readonly Lazy<Globals> Lazy = new Lazy<Globals>(() => new Globals());

        public World World { get; set; }
        public TerrainTypes TerrainTypes { get; }
        public RaceTypes RaceTypes { get; }

        public BuildingTypes BuildingTypes { get; }
        public BuildingPopulationGrowthTypes BuildingPopulationGrowthTypes { get; }
        public BuildingMaximumPopulationIncreaseTypes BuildingMaximumPopulationIncreaseTypes { get; }

        //public MovementTypes MovementTypes { get; }
        //public MineralTypes MineralTypes { get; }
        //public UnitTypes UnitTypes { get; }

        public static Globals Instance => Lazy.Value;

        private Globals()
        {
            TerrainTypes = TerrainTypes.Create(TerrainTypesLoader.GetTerrainTypes());
            RaceTypes = RaceTypes.Create(RaceTypesLoader.GetRaceTypes());

            BuildingTypes = BuildingTypes.Create(BuildingTypesLoader.Load());
            BuildingPopulationGrowthTypes = BuildingPopulationGrowthTypes.Create(BuildingPopulationGrowthTypesLoader.Load());
            BuildingMaximumPopulationIncreaseTypes = BuildingMaximumPopulationIncreaseTypes.Create(BuildingMaximumPopulationIncreaseTypesLoader.Load());

            //MovementTypes = MovementTypes.Create(new List<MovementType> { MovementType.Create(1, "Ground") });
            //MineralTypes = MineralTypes.Create(MineralTypesLoader.GetMineralTypes());
            //UnitTypes = UnitTypes.Create(UnitTypesLoader.GetUnitTypes(MovementTypes));
        }
    }
}