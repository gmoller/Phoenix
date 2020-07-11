using System.Collections.Generic;
using System.Diagnostics;

namespace PhoenixGameLibrary.GameData
{
    /// <summary>
    /// This struct is immutable.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public struct TerrainType : IIdentifiedByIdAndName
    {
        public static readonly TerrainType Invalid = new TerrainType(-1, "None", -1);

        public int Id { get; }
        public string Name { get; }
        public float MovementCostWalking { get; }
        public float FoodOutput => Globals.Instance.TerrainFoodOutputTypes.Contains(Id) ? Globals.Instance.TerrainFoodOutputTypes[Id].FoodOutput  : 0.0f;
        public float ProductionPercentage => Globals.Instance.TerrainProductionPercentageTypes.Contains(Id) ? Globals.Instance.TerrainProductionPercentageTypes[Id].ProductionPercentage : 0.0f;
        public bool CanSettleOn => Globals.Instance.TerrainCanSettleOnTypes.Contains(Id) ? true : false;
        public List<Texture> PossibleTextures { get; }

        private TerrainType(int id, string name, float movementCostWalking, params Texture[] possibleTextures)
        {
            Id = id;
            Name = name;
            MovementCostWalking = movementCostWalking;
            PossibleTextures = new List<Texture>();
            foreach (var texture in possibleTextures)
            {
                PossibleTextures.Add(texture);
            }
        }

        public static TerrainType Create(int id, string name, float movementCost, params Texture[] possibleTextures)
        {
            return new TerrainType(id, name, movementCost, possibleTextures);
        }

        private string DebuggerDisplay => $"{{Id={Id},Name={Name}}}";
    }

    public static class TerrainTypesLoader
    {
        public static NamedDataList<TerrainType> Load()
        {
            var terrainTypes = new List<TerrainType>
            {
                TerrainType.Create(0, "Grassland", 1.0f, new Texture("terrain_hextiles_basic_1", 0), new Texture("terrain_hextiles_basic_1", 1), new Texture("terrain_hextiles_basic_1", 2), new Texture("terrain_hextiles_basic_1", 3)),
                TerrainType.Create(1, "Forest", 2.0f, new Texture("terrain_hextiles_basic_1", 16), new Texture("terrain_hextiles_basic_1", 17), new Texture("terrain_hextiles_basic_1", 18), new Texture("terrain_hextiles_basic_1", 19)),
                TerrainType.Create(2, "Desert", 1.0f, new Texture("terrain_hextiles_basic_1", 12), new Texture("terrain_hextiles_basic_1", 13), new Texture("terrain_hextiles_basic_1", 14), new Texture("terrain_hextiles_basic_1", 15)),
                TerrainType.Create(3, "Swamp", 3.0f, new Texture("terrain_hextiles_basic_1", 20), new Texture("terrain_hextiles_basic_1", 21), new Texture("terrain_hextiles_basic_1", 22), new Texture("terrain_hextiles_basic_1", 23)),
                //TerrainType.Create(4, "River", 2.0f, 2.0f, 0.0f),
                //TerrainType.Create(5, "River Mouth", 2.0f, 2.0f, 2.0f),
                TerrainType.Create(6, "Hill", 3.0f, new Texture("terrain_hextiles_basic_1", 32), new Texture("terrain_hextiles_basic_1", 33), new Texture("terrain_hextiles_basic_1", 34), new Texture("terrain_hextiles_basic_1", 35)),
                TerrainType.Create(7, "Mountain", 4.0f, new Texture("terrain_hextiles_basic_1", 8), new Texture("terrain_hextiles_basic_1", 9), new Texture("terrain_hextiles_basic_1", 10), new Texture("terrain_hextiles_basic_1", 11)),
                //TerrainType.Create(8, "Volcano", 4.0f, 0.0f, 0.0f),
                TerrainType.Create(9, "Tundra", 2.0f, new Texture("terrain_hextiles_cold_1", 8), new Texture("terrain_hextiles_cold_1", 9), new Texture("terrain_hextiles_cold_1", 10), new Texture("terrain_hextiles_cold_1", 11)),
                //TerrainType.Create(10, "Shore", -1.0f, 0.5f, 0.0f),
                TerrainType.Create(11, "Ocean", -1.0f, new Texture("terrain_hextiles_basic_1", 4), new Texture("terrain_hextiles_basic_1", 5), new Texture("terrain_hextiles_basic_1", 6), new Texture("terrain_hextiles_basic_1", 7))
                //TerrainType.Create(12, "SorceryNode", 1.0f, 2.0f, 0.0f),
                //TerrainType.Create(13, "ChaosNode", 4.0f, 0.0f, 5.0f),
                //TerrainType.Create(14, "NatureNode", 2.0f, 2.5f, 3.0f)
            };

            return NamedDataList<TerrainType>.Create(terrainTypes);
        }
    }
}