using System.Diagnostics;

namespace PhoenixGameLibrary.GameData
{
    /// <summary>
    /// This struct is immutable.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public struct TerrainType
    {
        public static readonly TerrainType Invalid = new TerrainType(-1, "None", -1, 0.0f, 0.0f, string.Empty, 0);

        public int Id { get; }
        public string Name { get; }
        public int MovementCostWalking { get; }
        public float MaximumPopulation { get; }
        public float ProductionPercentage { get; }
        public string TexturePalette { get; }
        public byte TextureId { get; }

        private TerrainType(int id, string name, int movementCostWalking, float maximumPopulation, float productionPercentage, string texturePalette, byte textureId)
        {
            Id = id;
            Name = name;
            MovementCostWalking = movementCostWalking;
            MaximumPopulation = maximumPopulation;
            ProductionPercentage = productionPercentage;
            TexturePalette = texturePalette;
            TextureId = textureId;
        }

        public static TerrainType Create(int id, string name, int movementCost, float foodOutput, float productionPercentage, string texturePalette, byte textureId)
        {
            return new TerrainType(id, name, movementCost, foodOutput, productionPercentage, texturePalette, textureId);
        }

        private string DebuggerDisplay => $"{{Id={Id},Name={Name}}}";
    }
}