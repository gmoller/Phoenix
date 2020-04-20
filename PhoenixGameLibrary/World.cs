using System.Globalization;
using GameLogic;
using Utilities;

namespace PhoenixGameLibrary
{
    public class World
    {
        private int _turnNumber;

        public OverlandMap OverlandMap { get; }
        public Settlements Settlements { get; }
        public Settlement Settlement { get; set; }
        public Units Units { get; set; }
        public Unit Unit { get; set; }
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

        public bool IsInSettlementView { get; set; }

        public World()
        {
            Globals.Instance.World = this;

            PlayerFaction = new Faction();
            OverlandMap = new OverlandMap(this);
            Settlements = new Settlements();
            Units = new Units();
            _turnNumber = 0;
            NotificationList = new NotificationList();
        }

        public void AddStartingSettlement()
        {
            Settlements.AddSettlement("Fairhaven", "Barbarians", new Point(12, 9), OverlandMap.CellGrid);
        }

        public void AddStartingUnit()
        {
            Units.AddUnit(new Point(11, 9));
        }

        public void Update(float deltaTime)
        {
            Settlements.Update(deltaTime);
            Units.Update(deltaTime);

            PlayerFaction.FoodPerTurn = Settlements.FoodProducedThisTurn;
        }

        public void EndTurn()
        {
            NotificationList.Clear();
            Settlements.EndTurn();
            Units.EndTurn();
            _turnNumber++;
        }
    }
}