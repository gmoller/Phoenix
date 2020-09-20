using System.Globalization;
using System.Linq;
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

        public World(int numberOfColumns, int numberOfRows)
        {
            PlayerFaction = new Faction(this);
            OverlandMap = new OverlandMap(this, numberOfColumns, numberOfRows);
            Settlements = new Settlements();
            Stacks = new Stacks();
            _turnNumber = 0;
            NotificationList = new NotificationList();
        }

        internal void AddSettlement(PointI location, string raceTypeName)
        {
            var gameMetadata = CallContext<GameMetadata>.GetData("GameMetadata");
            var raceTypes = gameMetadata.RaceTypes;

            var raceType = raceTypes[raceTypeName];
            var townNames = raceType.TownNames;
            var chosenIndex = RandomNumberGenerator.Instance.GetRandomInt(0, townNames.Length - 1);
            var name = townNames[chosenIndex];

            var foo = townNames.ToList();

            var addNewOutpostCommand = new AddNewOutpostCommand { Payload = (location, name, raceTypeName, Settlements, this) };
            addNewOutpostCommand.Execute();
        }

        internal void AddUnit(PointI location, UnitType unitType)
        {
            var addUnitCommand = new AddUnitCommand { Payload = (location, unitType, Stacks, this) };
            addUnitCommand.Execute();
        }

        public void BeginTurn()
        {
            Stacks.BeginTurn();
        }

        public void EndTurn()
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