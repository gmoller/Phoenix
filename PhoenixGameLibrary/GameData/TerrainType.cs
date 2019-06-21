using System.Collections.Generic;
using System.Diagnostics;

namespace PhoenixGameLibrary.GameData
{
    /// <summary>
    /// This struct is immutable.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public struct TerrainType
    {
        public static readonly TerrainType Invalid = new TerrainType(-1, "None", -1, 0.0f, 0.0f);

        public int Id { get; }
        public string Name { get; }
        public int MovementCostWalking { get; }
        public float MaximumPopulation { get; }
        public float ProductionPercentage { get; }
        public List<Texture> PossibleTextures { get; }

        private TerrainType(int id, string name, int movementCostWalking, float maximumPopulation, float productionPercentage, params Texture[] possibleTextures)
        {
            Id = id;
            Name = name;
            MovementCostWalking = movementCostWalking;
            MaximumPopulation = maximumPopulation;
            ProductionPercentage = productionPercentage;
            PossibleTextures = new List<Texture>();
            foreach (var texture in possibleTextures)
            {
                PossibleTextures.Add(texture);
            }
        }

        public static TerrainType Create(int id, string name, int movementCost, float foodOutput, float productionPercentage, params Texture[] possibleTextures)
        {
            return new TerrainType(id, name, movementCost, foodOutput, productionPercentage, possibleTextures);
        }

        private string DebuggerDisplay => $"{{Id={Id},Name={Name}}}";
    }
}