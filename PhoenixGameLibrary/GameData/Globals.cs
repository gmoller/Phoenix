using System;

namespace PhoenixGameLibrary.GameData
{
    public sealed class Globals
    {
        private static readonly Lazy<Globals> Lazy = new Lazy<Globals>(() => new Globals());

        public World World { get; set; }

        public NamedDataList<TerrainType> TerrainTypes { get; }
        public DataList<TerrainFoodOutputType> TerrainFoodOutputTypes { get; }
        public DataList<TerrainProductionPercentageType> TerrainProductionPercentageTypes { get; }
        public DataList<TerrainCanSettleOnType> TerrainCanSettleOnTypes { get; }

        public NamedDataList<RaceType> RaceTypes { get; }

        public NamedDataList<BuildingType> BuildingTypes { get; }
        public DataList<BuildingPopulationGrowthType> BuildingPopulationGrowthTypes { get; }
        public DataList<BuildingMaximumPopulationIncreaseType> BuildingMaximumPopulationIncreaseTypes { get; }
        public DataList<BuildingFoodOutputIncreaseType> BuildingFoodOutputIncreaseTypes { get; }

        public NamedDataList<MovementType> MovementTypes { get; }
        //public MineralTypes MineralTypes { get; }
        public NamedDataList<UnitType> UnitTypes { get; }
        public NamedDataList<ActionType> ActionTypes { get; }

        public static Globals Instance => Lazy.Value;

        private Globals()
        {
            TerrainTypes = NamedDataList<TerrainType>.Create(TerrainTypesLoader.Load());
            TerrainFoodOutputTypes = DataList<TerrainFoodOutputType>.Create(TerrainFoodOutputTypesLoader.Load());
            TerrainProductionPercentageTypes = DataList<TerrainProductionPercentageType>.Create(TerrainProductionPercentageTypesLoader.Load());
            TerrainCanSettleOnTypes = DataList<TerrainCanSettleOnType>.Create(TerrainCanSettleOnTypesLoader.Load());

            RaceTypes = NamedDataList<RaceType>.Create(RaceTypesLoader.Load());

            BuildingTypes = NamedDataList<BuildingType>.Create(BuildingTypesLoader.Load());
            BuildingPopulationGrowthTypes = DataList<BuildingPopulationGrowthType>.Create(BuildingPopulationGrowthTypesLoader.Load());
            BuildingMaximumPopulationIncreaseTypes = DataList<BuildingMaximumPopulationIncreaseType>.Create(BuildingMaximumPopulationIncreaseTypesLoader.Load());
            BuildingFoodOutputIncreaseTypes = DataList<BuildingFoodOutputIncreaseType>.Create(BuildingFoodOutputIncreaseTypesLoader.Load());

            MovementTypes = NamedDataList<MovementType>.Create(MovementTypesLoader.Load());
            //MineralTypes = MineralTypes.Create(MineralTypesLoader.GetMineralTypes());
            UnitTypes = NamedDataList<UnitType>.Create(UnitTypesLoader.Load());
            ActionTypes = NamedDataList<ActionType>.Create(ActionTypesLoader.Load());
        }
    }
}