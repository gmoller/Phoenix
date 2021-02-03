using System.Collections.Generic;
using PhoenixGameConfig;
using PhoenixGameData.GameData;
using Zen.Utilities;

namespace PhoenixGameData
{
    public class GameConfigCache
    {
        private readonly List<int> _actionConfigIds = new List<int>();
        private readonly Dictionary<int, ActionConfig> _actionConfigsById = new Dictionary<int, ActionConfig>();
        private readonly Dictionary<string, int> _actionConfigsByName = new Dictionary<string, int>();

        private readonly List<int> _buildingConfigIds = new List<int>();
        private readonly Dictionary<int, BuildingConfig> _buildingConfigsById = new Dictionary<int, BuildingConfig>();
        private readonly Dictionary<string, int> _buildingConfigsByName = new Dictionary<string, int>();

        private readonly List<int> _movementConfigIds = new List<int>();
        private readonly Dictionary<int, MovementConfig> _movementConfigsById = new Dictionary<int, MovementConfig>();
        private readonly Dictionary<string, int> _movementConfigsByName = new Dictionary<string, int>();

        private readonly List<int> _raceConfigIds = new List<int>();
        private readonly Dictionary<int, RaceConfig> _raceConfigsById = new Dictionary<int, RaceConfig>();
        private readonly Dictionary<string, int> _raceConfigsByName = new Dictionary<string, int>();

        private readonly List<int> _terrainConfigIds = new List<int>();
        private readonly Dictionary<int, TerrainConfig> _terrainConfigsById = new Dictionary<int, TerrainConfig>();
        private readonly Dictionary<string, int> _terrainConfigsByName = new Dictionary<string, int>();

        private readonly List<int> _unitConfigIds = new List<int>();
        private readonly Dictionary<int, UnitConfig> _unitConfigsById = new Dictionary<int, UnitConfig>();
        private readonly Dictionary<string, int> _unitConfigsByName = new Dictionary<string, int>();

        public GameConfigCache()
        {
            var gameConfigRepository = CallContext<GameConfigRepository>.GetData("GameConfigRepository");

            var actions = gameConfigRepository.GetEntities("Action");
            foreach (var action in actions)
            {
                var id = (int)action.Id;
                var config = new ActionConfig(id);
                _actionConfigsById.Add(id, config);
                _actionConfigIds.Add(id);
                _actionConfigsByName.Add(config.Name, id);
            }

            var buildings = gameConfigRepository.GetEntities("Building");
            foreach (var building in buildings)
            {
                var id = (int)building.Id;
                var config = new BuildingConfig(id);
                _buildingConfigsById.Add(id, config);
                _buildingConfigIds.Add(id);
                _buildingConfigsByName.Add(config.Name, id);
            }

            var movements = gameConfigRepository.GetEntities("Movement");
            foreach (var movement in movements)
            {
                var id = (int)movement.Id;
                var config = new MovementConfig(id);
                _movementConfigsById.Add(id, config);
                _movementConfigIds.Add(id);
                _movementConfigsByName.Add(config.Name, id);
            }

            var races = gameConfigRepository.GetEntities("Race");
            foreach (var race in races)
            {
                var id = (int)race.Id;
                var config = new RaceConfig(id);
                _raceConfigsById.Add(id, config);
                _raceConfigIds.Add(id);
                _raceConfigsByName.Add(config.Name, id);
            }

            var terrains = gameConfigRepository.GetEntities("Terrain");
            foreach (var terrain in terrains)
            {
                var id = (int)terrain.Id;
                var config = new TerrainConfig(id);
                _terrainConfigsById.Add(id, config);
                _terrainConfigIds.Add(id);
                _terrainConfigsByName.Add(config.Name, id);
            }

            var units = gameConfigRepository.GetEntities("Unit");
            foreach (var unit in units)
            {
                var id = (int)unit.Id;
                var config = new UnitConfig(id);
                _unitConfigsById.Add(id, config);
                _unitConfigIds.Add(id);
                _unitConfigsByName.Add(config.Name, id);
            }
        }

        public List<int> GetActionConfigIds()
        {
            return _actionConfigIds;
        }

        public ActionConfig GetActionConfigById(int id)
        {
            return _actionConfigsById[id];
        }

        public ActionConfig GetActionConfigByName(string name)
        {
            var id = _actionConfigsByName[name];
            var actionConfig = GetActionConfigById(id);

            return actionConfig;
        }

        public List<int> GetBuildingConfigIds()
        {
            return _buildingConfigIds;
        }

        public BuildingConfig GetBuildingConfigById(int id)
        {
            return _buildingConfigsById[id];
        }

        public BuildingConfig GetBuildingConfigByName(string name)
        {
            var id = _buildingConfigsByName[name];
            var buildingConfig = GetBuildingConfigById(id);

            return buildingConfig;
        }

        public List<int> GetMovementConfigIds()
        {
            return _movementConfigIds;
        }

        public MovementConfig GetMovementConfigById(int id)
        {
            return _movementConfigsById[id];
        }

        public MovementConfig GetMovementConfigByName(string name)
        {
            var id = _movementConfigsByName[name];
            var movementConfig = GetMovementConfigById(id);

            return movementConfig;
        }

        public List<int> GetRaceConfigIds()
        {
            return _raceConfigIds;
        }

        public RaceConfig GetRaceConfigById(int id)
        {
            return _raceConfigsById[id];
        }

        public RaceConfig GetRaceConfigByName(string name)
        {
            var id = _raceConfigsByName[name];
            var raceConfig = GetRaceConfigById(id);

            return raceConfig;
        }

        public List<int> GetTerrainConfigIds()
        {
            return _terrainConfigIds;
        }

        public TerrainConfig GetTerrainConfigById(int id)
        {
            return _terrainConfigsById[id];
        }

        public TerrainConfig GetTerrainConfigByName(string name)
        {
            var id = _terrainConfigsByName[name];
            var terrainConfig = GetTerrainConfigById(id);

            return terrainConfig;
        }

        public List<int> GetUnitConfigIds()
        {
            return _unitConfigIds;
        }

        public UnitConfig GetUnitConfigById(int id)
        {
            return _unitConfigsById[id];
        }

        public UnitConfig GetUnitConfigByName(string name)
        {
            var id = _unitConfigsByName[name];
            var unitConfig = GetUnitConfigById(id);

            return unitConfig;
        }
    }
} 