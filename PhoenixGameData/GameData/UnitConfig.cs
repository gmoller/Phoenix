using System.Collections.Generic;
using PhoenixGameConfig;
using Zen.Utilities;

namespace PhoenixGameData.GameData
{
    public class UnitConfig
    {
        private readonly GameConfigRepository _gameConfigRepository;

        private readonly dynamic _unit;
        private readonly DynamicDataList _unitActions;
        private readonly DynamicDataList _unitMovements;
        private readonly DynamicDataList _unitBuildings;
        private readonly DynamicDataList _unitRaces;
        private readonly DynamicDataList _movementIncrementSightBy;

        public int Id => (int)_unit.Id;
        public string Name => (string)_unit.Name;
        public string ShortName => (string)_unit.ShortName;
        public float ConstructionCost => (float)_unit.ConstructionCost;
        public float MovementPoints => (float)_unit.MovementPoints;
        public string TextureName => (string)_unit.TextureName;
        public List<string> ActionsThisUnitCanPerform { get; }
        public List<string> MovementTypesThisUnitCanPerform { get; }
        public List<string> BuildingsRequiredToBuildThisUnit { get; }
        public List<string> RacesThatCanBuildThisUnit { get; }
        public List<int> MovementSightIncrements { get; }

        public UnitConfig(int unitId)
        {
            _gameConfigRepository = CallContext<GameConfigRepository>.GetData("GameConfigRepository");

            var units = _gameConfigRepository.GetEntities("Unit");

            _unit = units.GetById(unitId);
            _unitActions = _gameConfigRepository.GetEntities("UnitAction").Filter("UnitId", unitId);
            _unitMovements = _gameConfigRepository.GetEntities("UnitMovement").Filter("UnitId", unitId);
            _unitBuildings = _gameConfigRepository.GetEntities("UnitBuilding").Filter("UnitId", unitId);
            _unitRaces = _gameConfigRepository.GetEntities("UnitRace").Filter("UnitId", unitId);
            _movementIncrementSightBy = _gameConfigRepository.GetEntities("MovementIncrementSightBy");

            ActionsThisUnitCanPerform = _gameConfigRepository.ToList(_unitActions, "Action", "ActionId", "Name");
            MovementTypesThisUnitCanPerform = _gameConfigRepository.ToList(_unitMovements, "Movement", "MovementId", "Name");
            BuildingsRequiredToBuildThisUnit = _gameConfigRepository.ToList(_unitBuildings, "Building", "BuildingId", "Name");
            RacesThatCanBuildThisUnit = _gameConfigRepository.ToList(_unitRaces, "Race", "RaceId", "Name");
            MovementSightIncrements = new List<int>();

            foreach (var item in _unitMovements)
            {
                var movementId = (int)item.MovementId;
                var movementIncrementSightBy = _movementIncrementSightBy.Filter("MovementId", movementId);
                foreach (var item2 in movementIncrementSightBy)
                {
                    MovementSightIncrements.Add((int)item2.Value);
                }
            }
        }
    }
}