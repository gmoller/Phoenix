using PhoenixGameLibrary.GameData;
using Utilities;

namespace PhoenixGameLibrary
{
    public class GameMetadata
    {
        public NamedDataDictionary<TerrainType> TerrainTypes { get; }
        public DataDictionary<TerrainFoodOutputType> TerrainFoodOutputTypes { get; }
        public DataDictionary<TerrainProductionPercentageType> TerrainProductionPercentageTypes { get; }
        public DataDictionary<TerrainCanSettleOnType> TerrainCanSettleOnTypes { get; }

        public NamedDataDictionary<RaceType> RaceTypes { get; }

        public NamedDataDictionary<BuildingType> BuildingTypes { get; }
        public DataDictionary<BuildingPopulationGrowthType> BuildingPopulationGrowthTypes { get; }
        public DataDictionary<BuildingMaximumPopulationIncreaseType> BuildingMaximumPopulationIncreaseTypes { get; }
        public DataDictionary<BuildingFoodOutputIncreaseType> BuildingFoodOutputIncreaseTypes { get; }

        public NamedDataDictionary<MovementType> MovementTypes { get; }
        //public MineralTypes MineralTypes { get; }
        public NamedDataDictionary<UnitType> UnitTypes { get; }
        public NamedDataDictionary<ActionType> ActionTypes { get; }

        public GameMetadata()
        {
            TerrainTypes = NamedDataDictionary<TerrainType>.Create(TerrainTypesLoader.Load());
            TerrainFoodOutputTypes = DataDictionary<TerrainFoodOutputType>.Create(TerrainFoodOutputTypesLoader.Load());
            TerrainProductionPercentageTypes = DataDictionary<TerrainProductionPercentageType>.Create(TerrainProductionPercentageTypesLoader.Load());
            TerrainCanSettleOnTypes = DataDictionary<TerrainCanSettleOnType>.Create(TerrainCanSettleOnTypesLoader.Load());

            RaceTypes = NamedDataDictionary<RaceType>.Create(RaceTypesLoader.Load());

            BuildingTypes = NamedDataDictionary<BuildingType>.Create(BuildingTypesLoader.Load());
            BuildingPopulationGrowthTypes = DataDictionary<BuildingPopulationGrowthType>.Create(BuildingPopulationGrowthTypesLoader.Load());
            BuildingMaximumPopulationIncreaseTypes = DataDictionary<BuildingMaximumPopulationIncreaseType>.Create(BuildingMaximumPopulationIncreaseTypesLoader.Load());
            BuildingFoodOutputIncreaseTypes = DataDictionary<BuildingFoodOutputIncreaseType>.Create(BuildingFoodOutputIncreaseTypesLoader.Load());

            MovementTypes = NamedDataDictionary<MovementType>.Create(MovementTypesLoader.Load());
            //MineralTypes = MineralTypes.Create(MineralTypesLoader.GetMineralTypes());
            UnitTypes = NamedDataDictionary<UnitType>.Create(UnitTypesLoader.Load());
            ActionTypes = NamedDataDictionary<ActionType>.Create(ActionTypesLoader.Load());
        }
    }
}