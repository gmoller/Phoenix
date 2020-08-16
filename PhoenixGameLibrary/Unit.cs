using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;
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
        private readonly World _world;

        private readonly UnitType _unitType;
        private List<Cell> _seenCells;
        private UnitStatus _status;

        private Guid Id { get; }
        public Point Location { get; internal set; } // hex cell the unit is in
        public float MovementPoints { get; internal set; }

        private string Name => _unitType.Name;
        public EnumerableList<string> Actions => new EnumerableList<string>(_unitType.Actions);
        public EnumerableList<string> UnitTypeMovementTypes => new EnumerableList<string>(_unitType.MovementTypes);
        public string UnitTypeTextureName => _unitType.TextureName;

        public Unit(World world, UnitType unitType, Point location)
        {
            _world = world;
            Id = Guid.NewGuid();
            _unitType = unitType;
            Location = location;
            MovementPoints = unitType.MovementPoints;

            SetSeenCells(location);
        }

        internal void DoPatrolAction()
        {
            _status = UnitStatus.Patrol;
            SetSeenCells(Location);
        }

        internal void DoFortifyAction()
        {
            // TODO: increment defense by 1
            _status = UnitStatus.Fortify;
            SetSeenCells(Location);
        }

        internal void DoExploreAction()
        {
            _status = UnitStatus.Explore;
            SetSeenCells(Location);
        }

        internal void DoBuildAction()
        {
            // assume settle for now 
            // check if can settle
            // if can call create outpost command, and return true
            // if not return false
        }

        internal void SetStatusToNone()
        {
            _status = UnitStatus.None;
            SetSeenCells(Location);
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

        public GetCostToMoveIntoResult CostToMoveInto(Point location)
        {
            var cellToMoveTo = _world.OverlandMap.CellGrid.GetCell(location.X, location.Y);

            return CostToMoveInto(cellToMoveTo);
        }

        private GetCostToMoveIntoResult CostToMoveInto(Cell cell)
        {
            if (cell == Cell.Empty) return new GetCostToMoveIntoResult(false);
            if (cell.SeenState == SeenState.NeverSeen) return new GetCostToMoveIntoResult(true, 9999999.9f);

            var context = (GlobalContext)CallContext.LogicalGetData("AmbientGlobalContext");
            var terrainTypes = ((GameMetadata)context.GameMetadata).TerrainTypes;
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

        internal void SetSeenCells(Point location)
        {
            var cellGrid = _world.OverlandMap.CellGrid;
            _seenCells = cellGrid.GetCatchment(location.X, location.Y, GetScoutingRange());
            foreach (var item in _seenCells)
            {
                var cell = cellGrid.GetCell(item.Column, item.Row);
                cellGrid.SetCell(cell, SeenState.CurrentlySeen);
            }
        }

        private int GetScoutingRange()
        {
            var context = (GlobalContext)CallContext.LogicalGetData("AmbientGlobalContext");
            var movementTypes = ((GameMetadata)context.GameMetadata).MovementTypes;

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

        private string DebuggerDisplay => $"{{Id={Id},Name={Name},Location={Location}}}";
    }
}