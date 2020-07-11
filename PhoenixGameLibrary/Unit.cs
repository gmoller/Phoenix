using PhoenixGameLibrary.GameData;
using Utilities;

namespace PhoenixGameLibrary
{
    /// <summary>
    /// 
    /// </summary>
    public class Unit
    {
        private const float BLINK_TIME_IN_MILLISECONDS = 500.0f;

        public Point Location { get; set; }
        public float MovementPoints { get; set; }
        public bool IsSelected { get; internal set; }
        public bool Blink { get; private set;  }

        private readonly UnitType _unitType;

        private float _blinkCooldownInMilliseconds = BLINK_TIME_IN_MILLISECONDS;

        public string Name => _unitType.Name;
        public string ShortName => _unitType.ShortName;

        public Unit(UnitType unitType, Point location)
        {
            _unitType = unitType;
            Location = location;
            MovementPoints = unitType.Moves.Moves;
        }

        public void Update(float deltaTime)
        {
            if (IsSelected)
            {
                _blinkCooldownInMilliseconds -= deltaTime;
                if (_blinkCooldownInMilliseconds <= 0)
                {
                    Blink = !Blink;
                    _blinkCooldownInMilliseconds = BLINK_TIME_IN_MILLISECONDS;
                }
            }
            else
            {
                Blink = false;
            }
        }

        public void MoveTo(Point locationToMoveTo)
        {
            Location = locationToMoveTo;

            var cellToMoveTo = Globals.Instance.World.OverlandMap.CellGrid.GetCell(locationToMoveTo.X, locationToMoveTo.Y);
            var movementCost = Globals.Instance.TerrainTypes[cellToMoveTo.TerrainTypeId].MovementCostWalking;
            MovementPoints -= movementCost;
        }

        public void EndTurn()
        {
            MovementPoints = _unitType.Moves.Moves;
        }
    }
}