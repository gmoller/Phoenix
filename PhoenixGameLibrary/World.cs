using System.Globalization;
using PhoenixGameConfig;
using PhoenixGameData;
using PhoenixGameLibrary.Commands;
using Zen.Hexagons;
using Zen.Utilities;

namespace PhoenixGameLibrary
{
    public class World
    {
        #region State
        private int _turnNumber;

        public HexLibrary HexLibrary { get; }
        public OverlandMap OverlandMap { get; }
        public NotificationList NotificationList { get; }
        public Faction PlayerFaction { get; }
        public Settlements Settlements { get; }
        public Stacks Stacks { get; }
        #endregion

        public string CurrentDate => GetCurrentDate();

        public World(int numberOfColumns, int numberOfRows, Faction faction)
        {
            _turnNumber = 0;

            HexLibrary = new HexLibrary(HexType.FlatTopped, OffsetCoordinatesType.Odd, 64.0f); // new HexLibrary(HexType.PointyTopped, OffsetCoordinatesType.Odd, 64.0f);
            OverlandMap = new OverlandMap(numberOfColumns, numberOfRows);
            NotificationList = new NotificationList();

            PlayerFaction = faction;
            Settlements = new Settlements();
            Stacks = new Stacks();
        }

        internal void AddSettlement(PointI location, int raceId)
        {
            var gameConfigCache = CallContext<GameConfigCache>.GetData("GameConfigCache");
            var race = gameConfigCache.GetRaceConfigById(1);
            var townName = race.GetRandomTownName();

            var addNewOutpostCommand = new AddNewOutpostCommand { Payload = (location, townName, raceId, Settlements, this) };
            addNewOutpostCommand.Execute();
        }

        internal void AddUnit(Stack stack)
        {
            Stacks.Add(stack);
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