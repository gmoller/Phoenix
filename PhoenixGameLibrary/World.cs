using System.Globalization;
using PhoenixGameLibrary.Commands;
using PhoenixGameLibrary.GameData;
using Utilities;

namespace PhoenixGameLibrary
{
    public class World
    {
        #region State
        private int _turnNumber;

        public OverlandMap OverlandMap { get; }
        public Settlements Settlements { get; }
        public Stacks Stacks { get; }
        public Faction PlayerFaction { get; }
        public NotificationList NotificationList { get; }
        #endregion

        public string CurrentDate => GetCurrentDate();

        internal World(int numberOfColumns, int numberOfRows)
        {
            PlayerFaction = new Faction();
            OverlandMap = new OverlandMap(this, numberOfColumns, numberOfRows);
            Settlements = new Settlements(this);
            Stacks = new Stacks();
            _turnNumber = 0;
            NotificationList = new NotificationList();
        }

        internal void AddSettlement(Point location, string name, string raceTypeName)
        {
            var addNewOutpostCommand = new AddNewOutpostCommand { Payload = (location, name, raceTypeName, Settlements, this) };
            addNewOutpostCommand.Execute();
        }

        internal void AddUnit(Point location, UnitType unitType)
        {
            var addUnitCommand = new AddUnitCommand { Payload = (location, unitType, Stacks, this) };
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

        private string GetCurrentDate()
        {
            var month = _turnNumber % 12 + 1;
            var monthString = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
            var year = 1400 + _turnNumber / 12;

            return $"{monthString} {year}";
        }
    }
}