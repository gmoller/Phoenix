using System.Collections.Generic;
using PhoenixGameConfig;
using Zen.Utilities;

namespace PhoenixGameData.GameData
{
    public class TerrainConfig
    {
        private readonly GameConfigRepository _gameConfigRepository;

        private readonly dynamic _terrain;
        private readonly DynamicDataList _terrainMovements;
        private readonly DynamicDataList _terrainTextures;

        public int Id => (int)_terrain.Id;
        public string Name => _terrain.Name;
        public ColorRgba MinimapColor => new ColorRgba { R = _terrain.MinimapColor.R, G = _terrain.MinimapColor.G, B = _terrain.MinimapColor.B, A = _terrain.MinimapColor.A };
        public List<dynamic> Movements { get; }
        public List<Texture> PossibleTexturesForThisTerrain { get; }
        public bool CanSettleOn { get; }
        public float FoodOutput { get; }
        public float ProductionPercentage { get; }

        public TerrainConfig(int terrainId)
        {
            _gameConfigRepository = CallContext<GameConfigRepository>.GetData("GameConfigRepository");

            var terrains = _gameConfigRepository.GetEntities("Terrain");

            _terrain = terrains.GetById(terrainId);
            _terrainMovements = _gameConfigRepository.GetEntities("TerrainMovement").Filter("TerrainId", terrainId);
            _terrainTextures = _gameConfigRepository.GetEntities("TerrainTexture").Filter("TerrainId", terrainId);

            Movements = DetermineMovementsForThisTerrain();
            PossibleTexturesForThisTerrain = DeterminePossibleTexturesForThisTerrain();

            CanSettleOn = DetermineCanSettleOn(terrainId);
            FoodOutput = DetermineFoodOutput(terrainId);
            ProductionPercentage = DetermineProductionPercentage(terrainId);
        }

        private List<dynamic> DetermineMovementsForThisTerrain()
        {
            var list = new List<dynamic>();
            foreach (var item in _terrainMovements)
            {
                list.Add(item);
            }

            return list;
        }

        private List<Texture> DeterminePossibleTexturesForThisTerrain()
        {
            var toList = _gameConfigRepository.GetEntities("Texture");
            var list = new List<Texture>();
            foreach (var item in _terrainTextures)
            {
                var fromAttributeValue = (int)item["TextureId"];
                var toItem = toList.GetById(fromAttributeValue);
                var toAttribute1 = (string)toItem["TexturePalette"];
                var toAttribute2 = (byte)toItem["TextureId"];
                list.Add(new Texture(toAttribute1, toAttribute2));
            }

            return list;
        }

        private bool DetermineCanSettleOn(int terrainId)
        {
            var terrainCanSettleOns = _gameConfigRepository.GetEntities("TerrainCanSettleOn");
            var filtered = terrainCanSettleOns.Filter("TerrainId", terrainId);
            var canSettleOn = filtered.Count > 0;

            return canSettleOn;
        }

        private float DetermineFoodOutput(int terrainId)
        {
            var terrainFoodOutputs = _gameConfigRepository.GetEntities("TerrainFoodOutput");
            var filtered = terrainFoodOutputs.Filter("TerrainId", terrainId);

            var amt = 0.0f;
            foreach (var item in filtered)
            {
                amt += (float)item.Value;
            }

            return amt;
        }

        private float DetermineProductionPercentage(int terrainId)
        {
            var terrainProductionPercentages = _gameConfigRepository.GetEntities("TerrainProductionPercentage");
            var filtered = terrainProductionPercentages.Filter("TerrainId", terrainId);

            var amt = 0.0f;
            foreach (var item in filtered)
            {
                amt += (float)item.Value;
            }

            return amt;
        }
    }
}