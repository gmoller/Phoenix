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
        public Guid Id { get; }
        public Point Location { get; set; } // hex cell the unit is in
        public float MovementPoints { get; set; }

        private readonly UnitType _unitType;
        private List<Cell> _seenCells;

        public string Name => _unitType.Name;
        public string ShortName => _unitType.ShortName;
        public List<string> UnitTypeMovementTypes => _unitType.MovementTypes;
        public string UnitTypeTextureName => _unitType.TextureName;

        public UnitsStack UnitsStack { get; set; }

        public List<Point> PotentialMovementPath { get; set; }
        public List<Point> MovementPath { get; set; }

        internal Unit(UnitType unitType, Point location)
        {
            Id = Guid.NewGuid();
            _unitType = unitType;
            Location = location;
            MovementPoints = unitType.MovementPoints;
            MovementPath = new List<Point>();
            PotentialMovementPath = new List<Point>();

            SetSeenCells(location);
        }

        internal void MoveTo(Point locationToMoveTo)
        {
            Location = locationToMoveTo;
            SetSeenCells(Location);

            var cellToMoveTo = Globals.Instance.World.OverlandMap.CellGrid.GetCell(locationToMoveTo.X, locationToMoveTo.Y);
            var movementCost = CostToMoveInto(cellToMoveTo);

            MovementPoints -= movementCost;
            if (MovementPoints < 0.0f)
            {
                MovementPoints = 0.0f;
            }
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

        public bool CanMoveInto(Point location)
        {
            var cellToMoveTo = Globals.Instance.World.OverlandMap.CellGrid.GetCell(location.X, location.Y);

            return CanMoveInto(cellToMoveTo);
        }

        public bool CanMoveInto(Cell cell)
        {
            var terrainType = Globals.Instance.TerrainTypes[cell.TerrainTypeId];

            return CanMoveInto(terrainType);
        }

        public bool CanMoveInto(TerrainType terrainType)
        {
            var potentialMovements = new List<MovementCost>();
            foreach (var unitMovementType in UnitTypeMovementTypes)
            {
                foreach (var movementCost in terrainType.MovementCosts)
                {
                    if (unitMovementType != movementCost.MovementType.Name) continue;
                    if (movementCost.Cost > 0.0)
                    {
                        potentialMovements.Add(movementCost);
                    }
                }
            }

            var canMoveInto = potentialMovements.Count > 0;

            return canMoveInto;
        }

        public float CostToMoveInto(Cell cell)
        {
            var terrainType = Globals.Instance.TerrainTypes[cell.TerrainTypeId];

            return CostToMoveInto(terrainType);
        }

        public float CostToMoveInto(TerrainType terrainType)
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

            return costToMoveInto;
        }

        private void SetSeenCells(Point location)
        {
            var cellGrid = Globals.Instance.World.OverlandMap.CellGrid;
            _seenCells = cellGrid.GetCatchment(location.X, location.Y, 2);
            foreach (var item in _seenCells)
            {
                var cell = cellGrid.GetCell(item.Column, item.Row);
                cell.SeenState = SeenState.Current;
                cellGrid.SetCell(item.Column, item.Row, cell);
            }
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Id={Id},Name={Name},Location={Location}}}";
    }
}