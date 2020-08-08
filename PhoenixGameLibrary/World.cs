using System.Globalization;
using PhoenixGameLibrary.Commands;
using PhoenixGameLibrary.GameData;
using Utilities;

namespace PhoenixGameLibrary
{
    public class World
    {
        private int _turnNumber;

        public OverlandMap OverlandMap { get; }
        public Settlements Settlements { get; }
        public UnitsStacks UnitsStacks { get; set; }
        public Faction PlayerFaction { get; }
        public string CurrentDate
        {
            get
            {
                var year = 1400 + _turnNumber / 12;
                var month = _turnNumber % 12 + 1;
                var monthString = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);

                return $"{monthString} {year}";
            }
        }

        public NotificationList NotificationList { get; }

        internal World()
        {
            Globals.Instance.World = this;

            PlayerFaction = new Faction();
            OverlandMap = new OverlandMap(this);
            Settlements = new Settlements(this);
            UnitsStacks = new UnitsStacks();
            _turnNumber = 0;
            NotificationList = new NotificationList();
        }

        internal void AddSettlement(Point location, string name, string raceTypeName)
        {
            Settlements.AddSettlement(name, raceTypeName, location, OverlandMap.CellGrid);
        }

        internal void AddUnit(Point location, UnitType unitType)
        {
            var addUnitCommand = new AddUnitCommand { Payload = (location, unitType, UnitsStacks) };
            addUnitCommand.Execute();
        }

        internal void Update(float deltaTime)
        {
            Settlements.Update(deltaTime);

            PlayerFaction.FoodPerTurn = Settlements.FoodProducedThisTurn;
        }

        internal void BeginTurn()
        {
            UnitsStacks.BeginTurn();
        }

        internal void EndTurn()
        {
            NotificationList.Clear();
            Settlements.EndTurn();
            UnitsStacks.EndTurn();
            _turnNumber++;
        }
    }
}