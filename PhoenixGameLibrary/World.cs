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
        public Stacks Stacks { get; }
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

        internal World(int numberOfColumns, int numberOfRows)
        {
            Globals.Instance.World = this;

            PlayerFaction = new Faction();
            OverlandMap = new OverlandMap(this, numberOfColumns, numberOfRows);
            Settlements = new Settlements(this);
            Stacks = new Stacks();
            _turnNumber = 0;
            NotificationList = new NotificationList();
        }

        internal void AddSettlement(Point location, string name, string raceTypeName)
        {
            Settlements.AddSettlement(name, raceTypeName, location, OverlandMap.CellGrid);
        }

        internal void AddUnit(Point location, UnitType unitType)
        {
            var addUnitCommand = new AddUnitCommand { Payload = (location, unitType, Stacks) };
            addUnitCommand.Execute();
        }

        internal void Update(float deltaTime)
        {
            Settlements.Update(deltaTime);

            PlayerFaction.FoodPerTurn = Settlements.FoodProducedThisTurn;
        }

        internal void BeginTurn()
        {
            Stacks.BeginTurn();
        }

        internal void EndTurn()
        {
            NotificationList.Clear();
            Settlements.EndTurn();
            Stacks.EndTurn();
            _turnNumber++;
        }
    }
}