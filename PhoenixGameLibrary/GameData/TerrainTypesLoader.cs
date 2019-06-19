using System.Collections.Generic;

namespace PhoenixGameLibrary.GameData
{
    public static class TerrainTypesLoader
    {
        public static List<TerrainType> GetTerrainTypes()
        {
            var terrainTypes = new List<TerrainType>
            {
                TerrainType.Create(0, "Grassland", 1, 1.5f, 0.0f, "terrain_hextiles_basic_1", 0),
                TerrainType.Create(1, "Grassland", 1, 1.5f, 0.0f, "terrain_hextiles_basic_1", 1),
                TerrainType.Create(2, "Grassland", 1, 1.5f, 0.0f, "terrain_hextiles_basic_1", 2),
                TerrainType.Create(3, "Grassland", 1, 1.5f, 0.0f, "terrain_hextiles_basic_1", 3),

                TerrainType.Create(4, "Forest", 2, 0.5f, 3.0f, "terrain_hextiles_basic_1", 16),
                TerrainType.Create(5, "Forest", 2, 0.5f, 3.0f, "terrain_hextiles_basic_1", 17),
                TerrainType.Create(6, "Forest", 2, 0.5f, 3.0f, "terrain_hextiles_basic_1", 18),
                TerrainType.Create(7, "Forest", 2, 0.5f, 3.0f, "terrain_hextiles_basic_1", 19),

                TerrainType.Create(8, "Desert", 1, 0.0f, 3.0f, "terrain_hextiles_basic_1", 12),
                TerrainType.Create(9, "Desert", 1, 0.0f, 3.0f, "terrain_hextiles_basic_1", 13),
                TerrainType.Create(10, "Desert", 1, 0.0f, 3.0f, "terrain_hextiles_basic_1", 14),
                TerrainType.Create(11, "Desert", 1, 0.0f, 3.0f, "terrain_hextiles_basic_1", 15),

                TerrainType.Create(12, "Swamp", 3, 0.0f, 0.0f, "terrain_hextiles_basic_1", 20),
                TerrainType.Create(13, "Swamp", 3, 0.0f, 0.0f, "terrain_hextiles_basic_1", 21),
                TerrainType.Create(14, "Swamp", 3, 0.0f, 0.0f, "terrain_hextiles_basic_1", 22),
                TerrainType.Create(15, "Swamp", 3, 0.0f, 0.0f, "terrain_hextiles_basic_1", 23),

                //TerrainType.Create(4, "River", 2, 2.0f, 0.0f),
                //TerrainType.Create(5, "River Mouth", 2, 2.0f, 2.0f),

                TerrainType.Create(16, "Hill", 3, 0.5f, 3.0f, "terrain_hextiles_basic_1", 32),
                TerrainType.Create(17, "Hill", 3, 0.5f, 3.0f, "terrain_hextiles_basic_1", 33),
                TerrainType.Create(18, "Hill", 3, 0.5f, 3.0f, "terrain_hextiles_basic_1", 34),
                TerrainType.Create(19, "Hill", 3, 0.5f, 3.0f, "terrain_hextiles_basic_1", 35),

                TerrainType.Create(20, "Mountain", 4, 0.0f, 5.0f, "terrain_hextiles_basic_1", 8),
                TerrainType.Create(21, "Mountain", 4, 0.0f, 5.0f, "terrain_hextiles_basic_1", 9),
                TerrainType.Create(22, "Mountain", 4, 0.0f, 5.0f, "terrain_hextiles_basic_1", 10),
                TerrainType.Create(23, "Mountain", 4, 0.0f, 5.0f, "terrain_hextiles_basic_1", 11),

                //TerrainType.Create(8, "Volcano", 4, 0.0f, 0.0f),

                TerrainType.Create(24, "Tundra", 2, 0.0f, 0.0f, "terrain_hextiles_cold_1", 8),
                TerrainType.Create(25, "Tundra", 2, 0.0f, 0.0f, "terrain_hextiles_cold_1", 9),
                TerrainType.Create(26, "Tundra", 2, 0.0f, 0.0f, "terrain_hextiles_cold_1", 10),
                TerrainType.Create(27, "Tundra", 2, 0.0f, 0.0f, "terrain_hextiles_cold_1", 11),

                //TerrainType.Create(10, "Shore", -1, 0.5f, 0.0f),

                TerrainType.Create(28, "Ocean", -1, 0.0f, 0.0f, "terrain_hextiles_basic_1", 4),
                TerrainType.Create(29, "Ocean", -1, 0.0f, 0.0f, "terrain_hextiles_basic_1", 5),
                TerrainType.Create(30, "Ocean", -1, 0.0f, 0.0f, "terrain_hextiles_basic_1", 6),
                TerrainType.Create(31, "Ocean", -1, 0.0f, 0.0f, "terrain_hextiles_basic_1", 7),

                //TerrainType.Create(12, "SorceryNode", 1, 2.0f, 0.0f),
                //TerrainType.Create(13, "ChaosNode", 4, 0.0f, 5.0f),
                //TerrainType.Create(14, "NatureNode", 2, 2.5f, 3.0f)
            };

            return terrainTypes;
        }
    }
}