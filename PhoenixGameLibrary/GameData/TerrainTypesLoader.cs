using System.Collections.Generic;

namespace PhoenixGameLibrary.GameData
{
    public static class TerrainTypesLoader
    {
        public static List<TerrainType> GetTerrainTypes()
        {
            var terrainTypes = new List<TerrainType>
            {
                TerrainType.Create(0, "Grassland", 1, 1.5f, 0.0f, new Texture("terrain_hextiles_basic_1", 0), new Texture("terrain_hextiles_basic_1", 1), new Texture("terrain_hextiles_basic_1", 2), new Texture("terrain_hextiles_basic_1", 3)),
                TerrainType.Create(1, "Forest", 2, 0.5f, 3.0f, new Texture("terrain_hextiles_basic_1", 16), new Texture("terrain_hextiles_basic_1", 17), new Texture("terrain_hextiles_basic_1", 18), new Texture("terrain_hextiles_basic_1", 19)),
                TerrainType.Create(2, "Desert", 1, 0.0f, 3.0f, new Texture("terrain_hextiles_basic_1", 12), new Texture("terrain_hextiles_basic_1", 13), new Texture("terrain_hextiles_basic_1", 14), new Texture("terrain_hextiles_basic_1", 15)),
                TerrainType.Create(3, "Swamp", 3, 0.0f, 0.0f, new Texture("terrain_hextiles_basic_1", 20), new Texture("terrain_hextiles_basic_1", 21), new Texture("terrain_hextiles_basic_1", 22), new Texture("terrain_hextiles_basic_1", 23)),
                //TerrainType.Create(4, "River", 2, 2.0f, 0.0f),
                //TerrainType.Create(5, "River Mouth", 2, 2.0f, 2.0f),
                TerrainType.Create(6, "Hill", 3, 0.5f, 3.0f, new Texture("terrain_hextiles_basic_1", 32), new Texture("terrain_hextiles_basic_1", 33), new Texture("terrain_hextiles_basic_1", 34), new Texture("terrain_hextiles_basic_1", 35)),
                TerrainType.Create(7, "Mountain", 4, 0.0f, 5.0f, new Texture("terrain_hextiles_basic_1", 8), new Texture("terrain_hextiles_basic_1", 9), new Texture("terrain_hextiles_basic_1", 10), new Texture("terrain_hextiles_basic_1", 11)),
                //TerrainType.Create(8, "Volcano", 4, 0.0f, 0.0f),
                TerrainType.Create(9, "Tundra", 2, 0.0f, 0.0f, new Texture("terrain_hextiles_cold_1", 8), new Texture("terrain_hextiles_cold_1", 9), new Texture("terrain_hextiles_cold_1", 10), new Texture("terrain_hextiles_cold_1", 11)),
                //TerrainType.Create(10, "Shore", -1, 0.5f, 0.0f),
                TerrainType.Create(11, "Ocean", -1, 0.0f, 0.0f, new Texture("terrain_hextiles_basic_1", 4), new Texture("terrain_hextiles_basic_1", 5), new Texture("terrain_hextiles_basic_1", 6), new Texture("terrain_hextiles_basic_1", 7))
                //TerrainType.Create(12, "SorceryNode", 1, 2.0f, 0.0f),
                //TerrainType.Create(13, "ChaosNode", 4, 0.0f, 5.0f),
                //TerrainType.Create(14, "NatureNode", 2, 2.5f, 3.0f)
            };

            return terrainTypes;
        }
    }
}