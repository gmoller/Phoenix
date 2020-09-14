using System;
using System.Collections.Generic;
using System.Diagnostics;
using PhoenixGameLibrary.GameData;
using Utilities;

namespace PhoenixGameLibrary
{
    /// <summary>
    /// A Unit is a game entity that can be moved around the map.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Unit
    {
        #region State
        private readonly World _world;

        private readonly UnitType _unitType;
        private List<Cell> _seenCells;
        private UnitStatus _status;

        private Guid Id { get; }
        public PointI LocationHex { get; internal set; } // hex cell the unit is in
        public float MovementPoints { get; internal set; }
        #endregion

        private string Name => _unitType.Name;
        public EnumerableList<string> Actions => new EnumerableList<string>(_unitType.Actions);
        public EnumerableList<string> UnitTypeMovementTypes => new EnumerableList<string>(_unitType.MovementTypes);
        public string UnitTypeTextureName => _unitType.TextureName;
        public int SightRange => GetSightRange();

        public Unit(World world, UnitType unitType, PointI locationHex)
        {
            _world = world;
            Id = Guid.NewGuid();
            _unitType = unitType;
            LocationHex = locationHex;
            MovementPoints = unitType.MovementPoints;

            SetSeenCells(locationHex);
        }

        internal void DoPatrolAction()
        {
            _status = UnitStatus.Patrol;
            MovementPoints = 0.0f;
            SetSeenCells(LocationHex);
        }

        internal void DoFortifyAction()
        {
            // TODO: increment defense by 1
            _status = UnitStatus.Fortify;
            MovementPoints = 0.0f;
            SetSeenCells(LocationHex);
        }

        internal void DoExploreAction()
        {
            _status = UnitStatus.Explore;
            SetSeenCells(LocationHex);
        }

        internal void DoBuildAction()
        {
            // assume settler for now
            _world.AddSettlement(LocationHex, "Barbarians"); // TODO: get new from user and race type name from faction
        }

        internal void SetStatusToNone()
        {
            _status = UnitStatus.None;
            SetSeenCells(LocationHex);
        }

        internal void EndTurn()
        {
            MovementPoints = _unitType.MovementPoints;
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
            var cellToMoveTo = _world.OverlandMap.CellGrid.GetCell(location.X, location.Y);

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
            var cellGrid = _world.OverlandMap.CellGrid;
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

            var scoutingRange = 1;

            var incrementByForMovementType = 0;
            foreach (var movementTypeKey in _unitType.MovementTypes)
            {
                var movementType = movementTypes[movementTypeKey];
                var incrementSightBy = movementType.IncrementSightBy;
                if (incrementSightBy > incrementByForMovementType)
                {
                    incrementByForMovementType = incrementSightBy;
                }
            }
            scoutingRange += incrementByForMovementType;

            if (_status == UnitStatus.Patrol)
            {
                scoutingRange += 1;
            }

            return scoutingRange;
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Id={Id},Name={Name},LocationHex={LocationHex}}}";
    }
}