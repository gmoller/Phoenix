using System;
using System.Collections.Generic;
using System.Diagnostics;
using PhoenixGameData;
using PhoenixGameData.Enumerations;
using PhoenixGameData.Tuples;
using PhoenixGameLibrary.GameData;
using Zen.Utilities;

namespace PhoenixGameLibrary
{
    /// <summary>
    /// A Unit is a game entity that can be moved around the map.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Unit
    {
        #region State
        private readonly UnitRecord _unitRecord;
        private List<Cell> _seenCells;
        #endregion

        private string Name
        {
            get
            {
                var gameMetadata = CallContext<GameMetadata>.GetData("GameMetadata");
                var unitTypes = gameMetadata.UnitTypes;

                return unitTypes[_unitRecord.UnitTypeId].Name;
            }
        }

        public float MovementPoints
        {
            get => _unitRecord.MovementPoints;
            set => _unitRecord.MovementPoints = value;
        }

        public EnumerableList<string> Actions
        {
            get
            {
                var gameMetadata = CallContext<GameMetadata>.GetData("GameMetadata");
                var unitTypes = gameMetadata.UnitTypes;

                return new EnumerableList<string>(unitTypes[_unitRecord.UnitTypeId].Actions);
            }
        }

        public EnumerableList<string> UnitTypeMovementTypes
        {
            get
            {
                var gameMetadata = CallContext<GameMetadata>.GetData("GameMetadata");
                var unitTypes = gameMetadata.UnitTypes;

                return new EnumerableList<string>(unitTypes[_unitRecord.UnitTypeId].MovementTypes);
            }
        }

        public string UnitTypeTextureName
        {
            get
            {
                var gameMetadata = CallContext<GameMetadata>.GetData("GameMetadata");
                var unitTypes = gameMetadata.UnitTypes; 
                
                return unitTypes[_unitRecord.UnitTypeId].TextureName;
            }
        }

        public int SightRange => GetSightRange();

        public Unit(UnitRecord unit)
        {
            _unitRecord = unit;

            MovementPoints = unit.MovementPoints;

            var gameDataRepository = CallContext<GameDataRepository>.GetData("GameDataRepository");
            var stackRecord = gameDataRepository.GetStackById(unit.StackId);
            SetSeenCells(stackRecord.LocationHex);
        }

        public Unit(UnitType unitType, int stackId)
        {
            var gameDataRepository = CallContext<GameDataRepository>.GetData("GameDataRepository");
            _unitRecord = new UnitRecord(unitType.Id, stackId);
            gameDataRepository.Add(_unitRecord);

            MovementPoints = unitType.MovementPoints;

            var stackRecord = gameDataRepository.GetStackById(stackId);
            SetSeenCells(stackRecord.LocationHex);
        }

        internal void DoPatrolAction()
        {
            var gameDataRepository = CallContext<GameDataRepository>.GetData("GameDataRepository");
            var stackRecord = gameDataRepository.GetStackById(_unitRecord.StackId);
            stackRecord.Status = UnitStatus.Patrol;
            MovementPoints = 0.0f;
            SetSeenCells(stackRecord.LocationHex);
        }

        internal void DoFortifyAction()
        {
            // TODO: increment defense by 1
            var gameDataRepository = CallContext<GameDataRepository>.GetData("GameDataRepository");
            var stackRecord = gameDataRepository.GetStackById(_unitRecord.StackId);
            stackRecord.Status = UnitStatus.Fortify;
            MovementPoints = 0.0f;
            SetSeenCells(stackRecord.LocationHex);
        }

        internal void DoExploreAction()
        {
            var gameDataRepository = CallContext<GameDataRepository>.GetData("GameDataRepository");
            var stackRecord = gameDataRepository.GetStackById(_unitRecord.StackId);
            stackRecord.Status = UnitStatus.Explore;
            SetSeenCells(stackRecord.LocationHex);
        }

        internal void DoBuildAction()
        {
            // assume settler for now
            var world = CallContext<World>.GetData("GameWorld");
            var gameDataRepository = CallContext<GameDataRepository>.GetData("GameDataRepository");
            var stackRecord = gameDataRepository.GetStackById(_unitRecord.StackId);
            world.AddSettlement(stackRecord.LocationHex, "Barbarians"); // TODO: get new from user and race type name from faction
        }

        internal void SetStatusToNone()
        {
            var gameDataRepository = CallContext<GameDataRepository>.GetData("GameDataRepository");
            var stackRecord = gameDataRepository.GetStackById(_unitRecord.StackId);
            stackRecord.Status = UnitStatus.None;
            SetSeenCells(stackRecord.LocationHex);
        }

        internal void EndTurn()
        {
            var gameMetadata = CallContext<GameMetadata>.GetData("GameMetadata");
            var unitTypes = gameMetadata.UnitTypes;
            _unitRecord.MovementPoints = unitTypes[_unitRecord.UnitTypeId].MovementPoints;
        }

        internal bool CanSeeCell(Cell cell)
        {
            // if cell is within 4 hexes
            foreach (var item in _seenCells)
            {
                if (cell.Column == item.Column && cell.Row == item.Row)
                {
                    return true;
                }
            }

            return false;
        }

        public GetCostToMoveIntoResult CostToMoveInto(PointI location)
        {
            var world = CallContext<World>.GetData("GameWorld");
            var cellToMoveTo = world.OverlandMap.CellGrid.GetCell(location.X, location.Y);

            return CostToMoveInto(cellToMoveTo);
        }

        private GetCostToMoveIntoResult CostToMoveInto(Cell cell)
        {
            if (cell == Cell.Empty) return new GetCostToMoveIntoResult(false);
            if (cell.SeenState == SeenState.NeverSeen) return new GetCostToMoveIntoResult(true, 9999999.9f);

            var gameMetadata = CallContext<GameMetadata>.GetData("GameMetadata");
            var terrainTypes = gameMetadata.TerrainTypes;
            var terrainType = terrainTypes[cell.TerrainTypeId];

            return CostToMoveInto(terrainType);
        }

        private GetCostToMoveIntoResult CostToMoveInto(TerrainType terrainType)
        {
            var potentialMovementCosts = GetPotentialMovementCosts(terrainType);
            var canMoveInto = potentialMovementCosts.Count > 0;

            if (!canMoveInto) return new GetCostToMoveIntoResult(false);

            float costToMoveInto = float.MaxValue;
            bool foundCost = false;
            foreach (var potentialMovementCost in potentialMovementCosts)
            {
                if (potentialMovementCost.Cost < costToMoveInto)
                {
                    costToMoveInto = potentialMovementCost.Cost;
                    foundCost = true;
                }
            }

            if (!foundCost) throw new Exception($"No cost found for Terrain Type [{terrainType}], UnitTypeMovementTypes [{UnitTypeMovementTypes}].");

            return new GetCostToMoveIntoResult(true, costToMoveInto);
        }

        private List<MovementCost> GetPotentialMovementCosts(TerrainType terrainType)
        {
            var potentialMovementCosts = new List<MovementCost>();
            foreach (var unitMovementType in UnitTypeMovementTypes)
            {
                foreach (var movementCost in terrainType.MovementCosts)
                {
                    if (unitMovementType != movementCost.MovementType.Name) continue;
                    if (movementCost.Cost > 0.0)
                    {
                        potentialMovementCosts.Add(movementCost);
                    }
                }
            }

            return potentialMovementCosts;
        }

        internal void SetSeenCells(PointI locationHex)
        {
            var world = CallContext<World>.GetData("GameWorld");
            var cellGrid = world.OverlandMap.CellGrid;
            _seenCells = cellGrid.GetCatchment(locationHex.X, locationHex.Y, GetSightRange());
            foreach (var item in _seenCells)
            {
                var cell = cellGrid.GetCell(item.Column, item.Row);
                cellGrid.SetCell(cell, SeenState.CurrentlySeen);
            }
        }

        private int GetSightRange()
        {
            var gameMetadata = CallContext<GameMetadata>.GetData("GameMetadata");
            var movementTypes = gameMetadata.MovementTypes;
            var unitTypes = gameMetadata.UnitTypes;

            var scoutingRange = 1;

            var incrementByForMovementType = 0;
            foreach (var movementTypeKey in unitTypes[_unitRecord.UnitTypeId].MovementTypes)
            {
                var movementType = movementTypes[movementTypeKey];
                var incrementSightBy = movementType.IncrementSightBy;
                if (incrementSightBy > incrementByForMovementType)
                {
                    incrementByForMovementType = incrementSightBy;
                }
            }
            scoutingRange += incrementByForMovementType;

            var gameDataRepository = CallContext<GameDataRepository>.GetData("GameDataRepository");
            var stackRecord = gameDataRepository.GetStackById(_unitRecord.StackId);
            if (stackRecord.Status == UnitStatus.Patrol)
            {
                scoutingRange += 1;
            }

            return scoutingRange;
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay
        {
            get
            {
                var gameDataRepository = CallContext<GameDataRepository>.GetData("GameDataRepository");
                var stackRecord = gameDataRepository.GetStackById(_unitRecord.StackId);
                var message = $"{{Id={_unitRecord.Id},Name={Name},LocationHex={stackRecord.LocationHex}}}";

                return message;
            }
        }
    }
}