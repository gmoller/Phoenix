using System;
using PhoenixGameLibrary.GameData;
using Utilities;

namespace PhoenixGameLibrary
{
    /// <summary>
    /// A Unit is a game entity that can be moved around the map.
    /// </summary>
    public class Unit
    {
        public Guid Id { get; }
        public Point Location { get; set; } // hex cell the unit is in
        public float MovementPoints { get; set; }
        public bool IsSelected { get; internal set; }

        private readonly UnitType _unitType;

        public string Name => _unitType.Name;
        public string ShortName => _unitType.ShortName;
        public string MovementTypeName => "Walking"; // TODO: remove hard-coding

        public Unit(UnitType unitType, Point location)
        {
            Id = Guid.NewGuid();
            _unitType = unitType;
            Location = location;
            MovementPoints = unitType.Moves.Moves;
        }

        //public void Update(float deltaTime)
        //{
        //}

        public void MoveTo(Point locationToMoveTo)
        {
            Location = locationToMoveTo;

            var cellToMoveTo = Globals.Instance.World.OverlandMap.CellGrid.GetCell(locationToMoveTo.X, locationToMoveTo.Y);
            var movementCost = Globals.Instance.TerrainTypes[cellToMoveTo.TerrainTypeId].MovementCosts[MovementTypeName];
            MovementPoints -= movementCost.Moves;
        }

        public void EndTurn()
        {
            MovementPoints = _unitType.Moves.Moves;
        }
    }
}