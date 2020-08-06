﻿using System;
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
        private readonly World _world;

        private readonly UnitType _unitType;
        private List<Cell> _seenCells;

        public Guid Id { get; }
        public Point Location { get; set; } // hex cell the unit is in
        public float MovementPoints { get; set; }

        public string Name => _unitType.Name;
        public string ShortName => _unitType.ShortName;
        public EnumerableList<string> Actions => new EnumerableList<string>(_unitType.Actions);
        public EnumerableList<string> UnitTypeMovementTypes => new EnumerableList<string>(_unitType.MovementTypes);
        public string UnitTypeTextureName => _unitType.TextureName;

        public UnitsStack UnitsStack { get; set; }

        public Unit(World world, UnitType unitType, Point location)
        {
            _world = world;
            Id = Guid.NewGuid();
            _unitType = unitType;
            Location = location;
            MovementPoints = unitType.MovementPoints;

            SetSeenCells(location);
        }

        internal void MoveTo(Point locationToMoveTo)
        {
            Location = locationToMoveTo;
            SetSeenCells(Location);

            var cellToMoveTo = Globals.Instance.World.OverlandMap.CellGrid.GetCell(locationToMoveTo.X, locationToMoveTo.Y);
            var movementCost = CostToMoveInto(cellToMoveTo);

            MovementPoints -= movementCost.CostToMoveInto;
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

        public CostToMoveIntoResult CostToMoveInto(Point location)
        {
            var cellToMoveTo = _world.OverlandMap.CellGrid.GetCell(location.X, location.Y);

            return CostToMoveInto(cellToMoveTo);
        }

        public CostToMoveIntoResult CostToMoveInto(Cell cell)
        {
            if (cell.SeenState == SeenState.Never) return new CostToMoveIntoResult(false);

            var terrainType = Globals.Instance.TerrainTypes[cell.TerrainTypeId];

            return CostToMoveInto(terrainType);
        }

        public CostToMoveIntoResult CostToMoveInto(TerrainType terrainType)
        {
            var potentialMovementCosts = GetPotentialMovementCosts(terrainType);
            var canMoveInto = potentialMovementCosts.Count > 0;

            if (!canMoveInto) return new CostToMoveIntoResult(false);

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

            return new CostToMoveIntoResult(true, costToMoveInto);
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