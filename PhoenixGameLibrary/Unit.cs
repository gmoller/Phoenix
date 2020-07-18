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
        public string MovementTypeName => "Walking"; // TODO: remove hard-coding
        public Moves UnitTypeMoves => _unitType.Moves;
        public string UnitTypeTextureName => _unitType.TextureName;

        internal Unit(UnitType unitType, Point location)
        {
            Id = Guid.NewGuid();
            _unitType = unitType;
            Location = location;
            MovementPoints = unitType.Moves["Ground"].Moves;

            SetSeenCells(location);
        }

        internal void MoveTo(Point locationToMoveTo)
        {
            Location = locationToMoveTo;
            SetSeenCells(Location);

            var cellToMoveTo = Globals.Instance.World.OverlandMap.CellGrid.GetCell(locationToMoveTo.X, locationToMoveTo.Y);
            var movementCost = Globals.Instance.TerrainTypes[cellToMoveTo.TerrainTypeId].MovementCosts[MovementTypeName];
            MovementPoints -= movementCost.Cost;
        }

        internal void EndTurn()
        {
            MovementPoints = _unitType.Moves["Ground"].Moves;
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