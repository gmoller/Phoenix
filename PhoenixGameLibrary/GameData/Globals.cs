using System;
using PhoenixGameLibrary;
using PhoenixGameLibrary.GameData;

namespace GameLogic
{
    public sealed class Globals
    {
        private static readonly Lazy<Globals> Lazy = new Lazy<Globals>(() => new Globals());

        public World World { get; set; }
        //public MovementTypes MovementTypes { get; }
        public TerrainTypes TerrainTypes { get; }
        //public MineralTypes MineralTypes { get; }
        //public UnitTypes UnitTypes { get; }
        //public RaceTypes RaceTypes { get; }
        //public BuildingTypes BuildingTypes { get; }

        public static Globals Instance => Lazy.Value;

        private Globals()
        {
            //GameWorld = GameWorld.Create();
            //MovementTypes = MovementTypes.Create(new List<MovementType> { MovementType.Create(1, "Ground") });
            TerrainTypes = TerrainTypes.Create(TerrainTypesLoader.GetTerrainTypes());
            //MineralTypes = MineralTypes.Create(MineralTypesLoader.GetMineralTypes());
            //UnitTypes = UnitTypes.Create(UnitTypesLoader.GetUnitTypes(MovementTypes));
            //RaceTypes = RaceTypes.Create(RaceTypesLoader.GetRaceTypes());
            //BuildingTypes = BuildingTypesLoader.GetBuildingTypes();
        }
    }
}