using System.Collections.Generic;
using System.Diagnostics;
using PhoenixGameData;
using PhoenixGameData.Enumerations;
using PhoenixGameData.StrongTypes;
using PhoenixGameData.Tuples;
using Zen.Utilities;

namespace PhoenixGameLibrary
{
    /// <summary>
    /// A Unit is a game entity that can be moved around the map.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Unit
    {
        private readonly GameConfigCache _gameConfigCache;
        private readonly GameDataRepository _gameDataRepository;

        private UnitRecord _unitRecord;
        private List<Cell> SeenCells { get; set; }

        private string Name
        {
            get
            {
                var name = _gameConfigCache.GetUnitConfigById(_unitRecord.UnitTypeId.Value).Name;

                return name;
            }
        }

        public float MovementPoints => _unitRecord.MovementPoints.Value;

        public EnumerableList<string> Actions
        {
            get
            {
                var actions = _gameConfigCache.GetUnitConfigById(_unitRecord.UnitTypeId.Value).ActionsThisUnitCanPerform;

                return new EnumerableList<string>(actions);
            }
        }

        public EnumerableList<string> UnitTypeMovementTypes
        {
            get
            {
                var movements = _gameConfigCache.GetUnitConfigById(_unitRecord.UnitTypeId.Value).MovementTypesThisUnitCanPerform;

                return new EnumerableList<string>(movements);
            }
        }

        public string UnitTypeTextureName
        {
            get
            {
                var textureName = _gameConfigCache.GetUnitConfigById(_unitRecord.UnitTypeId.Value).TextureName;

                return textureName;
            }
        }

        public int SightRange => GetSightRange(_unitRecord.UnitTypeId.Value, _unitRecord.StackId.Value);

        public Unit(int id)
        {
            _gameConfigCache = CallContext<GameConfigCache>.GetData("GameConfigCache");
            _gameDataRepository = CallContext<GameDataRepository>.GetData("GameDataRepository");
            _unitRecord = _gameDataRepository.GetUnitById(id);

            _gameDataRepository.UnitUpdated += UnitUpdated;
        }

        private void UnitUpdated(object sender, UnitRecord unitRecord)
        {
            if (unitRecord.Id == _unitRecord.Id)
            {
                _unitRecord = unitRecord;
            }
        }

        internal void DoPatrolAction()
        {
            var stackRecord = _gameDataRepository.GetStackById(_unitRecord.StackId.Value);
            var updatedStack = new StackRecord(stackRecord, new Status(UnitStatus.Patrol));
            _gameDataRepository.Update(updatedStack);

            _unitRecord = new UnitRecord(_unitRecord, new MovementPoints(0.0f));
            _gameDataRepository.Update(_unitRecord);
            //SetSeenCells(stackRecord.LocationHex.Value);
        }

        internal void DoFortifyAction()
        {
            // TODO: increment defense by 1
            var stackRecord = _gameDataRepository.GetStackById(_unitRecord.StackId.Value);
            var updatedStack = new StackRecord(stackRecord, new Status(UnitStatus.Fortify));
            _gameDataRepository.Update(updatedStack);
            
            _unitRecord = new UnitRecord(_unitRecord, new MovementPoints(0.0f));
            _gameDataRepository.Update(_unitRecord);
            //SetSeenCells(stackRecord.LocationHex.Value);
        }

        internal void DoExploreAction()
        {
            var stackRecord = _gameDataRepository.GetStackById(_unitRecord.StackId.Value);
            var updatedStack = new StackRecord(stackRecord, new Status(UnitStatus.Explore));
            _gameDataRepository.Update(updatedStack);

            //SetSeenCells(stackRecord.LocationHex.Value);
        }

        internal void DoBuildAction()
        {
            // assume settler for now
            var world = CallContext<World>.GetData("GameWorld");
            var stackRecord = _gameDataRepository.GetStackById(_unitRecord.StackId.Value);
            world.AddSettlement(stackRecord.LocationHex.Value, 1); // TODO: get new from user and race type name from faction
        }

        internal void SetStatusToNone()
        {
            var stackRecord = _gameDataRepository.GetStackById(_unitRecord.StackId.Value);
            var updatedStack = new StackRecord(stackRecord, new Status(UnitStatus.None));
            _gameDataRepository.Update(updatedStack);
            //SetSeenCells(stackRecord.LocationHex.Value);
        }

        internal void EndTurn()
        {
            var movementPoints = _gameConfigCache.GetUnitConfigById(_unitRecord.UnitTypeId.Value).MovementPoints;

            var updatedUnit = new UnitRecord(_unitRecord, new MovementPoints(movementPoints));
            _gameDataRepository.Update(updatedUnit);
        }

        internal bool CanSeeCell(Cell cell)
        {
            // if cell is within 4 hexes
            foreach (var item in SeenCells)
            {
                if (cell.Column == item.Column && cell.Row == item.Row)
                {
                    return true;
                }
            }

            return false;
        }

        internal void SetSeenCells(PointI locationHex)
        {
            var world = CallContext<World>.GetData("GameWorld");
            var cellGrid = world.OverlandMap.CellGrid;
            SeenCells = cellGrid.GetCatchment(locationHex.X, locationHex.Y, GetSightRange(_unitRecord.UnitTypeId.Value, _unitRecord.StackId.Value));
            foreach (var item in SeenCells)
            {
                var cell = cellGrid.GetCell(item.Column, item.Row);
                cellGrid.SetCell(cell, SeenState.CurrentlySeen);
            }
        }

        internal static int GetSightRange(int unitId, int stackId)
        {
            var gameConfigCache = CallContext<GameConfigCache>.GetData("GameConfigCache");

            var scoutingRange = 1;

            var incrementByForMovementType = 0;
            var unitConfig = gameConfigCache.GetUnitConfigById(unitId);
            foreach (var movementSightIncrement in unitConfig.MovementSightIncrements)
            { 
                if (movementSightIncrement > incrementByForMovementType)
                {
                    incrementByForMovementType = movementSightIncrement;
                }
            }
            scoutingRange += incrementByForMovementType;

            var gameDataRepository = CallContext<GameDataRepository>.GetData("GameDataRepository");
            var stackRecord = gameDataRepository.GetStackById(stackId);
            if (stackRecord.Status.Value == UnitStatus.Patrol)
            {
                scoutingRange += 1;
            }

            return scoutingRange;
        }

        public override string ToString() => DebuggerDisplay;

        private string DebuggerDisplay
        {
            get
            {
                var stackRecord = _gameDataRepository.GetStackById(_unitRecord.StackId.Value);
                var message = $"{{Id={_unitRecord.Id},Name={Name},LocationHex={stackRecord.LocationHex}}}";

                return message;
            }
        }
    }
}