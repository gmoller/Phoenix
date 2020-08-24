﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;
using Utilities;

namespace PhoenixGameLibrary.GameData
{
    /// <summary>
    /// This struct is immutable.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public struct TerrainType : IIdentifiedByIdAndName
    {
        public static readonly TerrainType Invalid = new TerrainType(-1, "None", null, new byte[] { 255, 255, 255, 255 });

        public int Id { get; }
        public string Name { get; }
        public MovementCosts MovementCosts { get; }
        public float FoodOutput => GetFoodOutput();
        public float ProductionPercentage => GetProductionPercentage();
        public bool CanSettleOn => GetCanSettleOn();
        public byte[] MinimapColor { get; }
        public List<Texture> PossibleTextures { get; }

        private TerrainType(int id, string name, MovementCosts movementCosts, byte[] minimapColor, params Texture[] possibleTextures)
        {
            Id = id;
            Name = name;
            MovementCosts = movementCosts;
            PossibleTextures = new List<Texture>();
            foreach (var texture in possibleTextures)
            {
                PossibleTextures.Add(texture);
            }

            MinimapColor = minimapColor;
        }

        public static TerrainType Create(int id, string name, MovementCosts movementCosts, byte[] minimapColor, params Texture[] possibleTextures)
        {
            return new TerrainType(id, name, movementCosts, minimapColor, possibleTextures);
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private float GetFoodOutput()
        {
            var context = (GlobalContext)CallContext.LogicalGetData("AmbientGlobalContext");
            var terrainFoodOutputTypes = context.GameMetadata.TerrainFoodOutputTypes;

            return terrainFoodOutputTypes.Contains(Id) ? terrainFoodOutputTypes[Id].FoodOutput : 0.0f;
        }

        private float GetProductionPercentage()
        {
            var context = (GlobalContext)CallContext.LogicalGetData("AmbientGlobalContext");
            var terrainProductionPercentageTypes = context.GameMetadata.TerrainProductionPercentageTypes;

            return terrainProductionPercentageTypes.Contains(Id) ? terrainProductionPercentageTypes[Id].ProductionPercentage : 0.0f;
        }

        private bool GetCanSettleOn()
        {
            var context = (GlobalContext)CallContext.LogicalGetData("AmbientGlobalContext");
            var terrainCanSettleOnTypes = context.GameMetadata.TerrainCanSettleOnTypes;

            return terrainCanSettleOnTypes.Contains(Id);
        }

        private string DebuggerDisplay => $"{{Id={Id},Name={Name}}}";
    }

    public static class TerrainTypesLoader
    {
        public static NamedDataDictionary<TerrainType> Load()
        {
            var terrainTypes = new List<TerrainType>
            {
                TerrainType.Create(0, "Grassland", new MovementCosts(new MovementCost("Walking", 1.0f), new MovementCost("Mountaineer", 3.0f), new MovementCost("Flying", 1.0f), new MovementCost("Sailing", 0.0f)), new byte[] { 144, 238, 144, 255}, new Texture("terrain_hextiles_basic_1", 0), new Texture("terrain_hextiles_basic_1", 1), new Texture("terrain_hextiles_basic_1", 2), new Texture("terrain_hextiles_basic_1", 3)),
                TerrainType.Create(1, "Forest", new MovementCosts(new MovementCost("Walking", 2.0f), new MovementCost("Forester", 1.0f), new MovementCost("Flying", 1.0f), new MovementCost("Sailing", 0.0f)), new byte[] { 34, 139, 34, 255}, new Texture("terrain_hextiles_basic_1", 16), new Texture("terrain_hextiles_basic_1", 17), new Texture("terrain_hextiles_basic_1", 18), new Texture("terrain_hextiles_basic_1", 19)),
                TerrainType.Create(2, "Desert", new MovementCosts(new MovementCost("Walking", 1.0f), new MovementCost("Flying", 1.0f), new MovementCost("Sailing", 0.0f)), new byte[] { 244, 164, 96, 255}, new Texture("terrain_hextiles_basic_1", 12), new Texture("terrain_hextiles_basic_1", 13), new Texture("terrain_hextiles_basic_1", 14), new Texture("terrain_hextiles_basic_1", 15)),
                TerrainType.Create(3, "Swamp", new MovementCosts(new MovementCost("Walking", 3.0f), new MovementCost("Flying", 1.0f), new MovementCost("Swimming", 1.0f), new MovementCost("Sailing", 0.0f)), new byte[] { 46, 139, 87, 255}, new Texture("terrain_hextiles_basic_1", 20), new Texture("terrain_hextiles_basic_1", 21), new Texture("terrain_hextiles_basic_1", 22), new Texture("terrain_hextiles_basic_1", 23)),
                //TerrainType.Create(4, "River", 2.0f, 2.0f, 0.0f),
                //TerrainType.Create(5, "River Mouth", 2.0f, 2.0f, 2.0f),
                TerrainType.Create(6, "Hill", new MovementCosts(new MovementCost("Walking", 3.0f), new MovementCost("Mountaineer", 1.0f), new MovementCost("Flying", 1.0f), new MovementCost("Sailing", 0.0f)), new byte[] { 210, 180, 140, 255}, new Texture("terrain_hextiles_basic_1", 32), new Texture("terrain_hextiles_basic_1", 33), new Texture("terrain_hextiles_basic_1", 34), new Texture("terrain_hextiles_basic_1", 35)),
                TerrainType.Create(7, "Mountain", new MovementCosts(new MovementCost("Walking", 4.0f), new MovementCost("Mountaineer", 1.0f), new MovementCost("Flying", 1.0f), new MovementCost("Sailing", 0.0f)), new byte[] { 139, 69, 19, 255}, new Texture("terrain_hextiles_basic_1", 8), new Texture("terrain_hextiles_basic_1", 9), new Texture("terrain_hextiles_basic_1", 10), new Texture("terrain_hextiles_basic_1", 11)),
                //TerrainType.Create(8, "Volcano", 4.0f, 0.0f, 0.0f),
                TerrainType.Create(9, "Tundra", new MovementCosts(new MovementCost("Walking", 2.0f), new MovementCost("Flying", 1.0f), new MovementCost("Swimming", 1.0f), new MovementCost("Sailing", 0.0f)), new byte[] { 255, 250, 250, 255}, new Texture("terrain_hextiles_cold_1", 8), new Texture("terrain_hextiles_cold_1", 9), new Texture("terrain_hextiles_cold_1", 10), new Texture("terrain_hextiles_cold_1", 11)),
                //TerrainType.Create(10, "Shore", -1.0f, 0.5f, 0.0f),
                TerrainType.Create(11, "Ocean", new MovementCosts(new MovementCost("Flying", 1.0f), new MovementCost("Swimming", 1.0f), new MovementCost("Sailing", 1.0f)), new byte[] { 0, 0, 255, 255}, new Texture("terrain_hextiles_basic_1", 4), new Texture("terrain_hextiles_basic_1", 5), new Texture("terrain_hextiles_basic_1", 6), new Texture("terrain_hextiles_basic_1", 7))
                //TerrainType.Create(12, "SorceryNode", 1.0f, 2.0f, 0.0f),
                //TerrainType.Create(13, "ChaosNode", 4.0f, 0.0f, 5.0f),
                //TerrainType.Create(14, "NatureNode", 2.0f, 2.5f, 3.0f)
            };

            return NamedDataDictionary<TerrainType>.Create(terrainTypes);
        }
    }
}