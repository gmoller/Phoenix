using PhoenixGameLibrary.GameData2;
using Zen.Utilities;

namespace PhoenixGameLibrary
{
    public class Faction
    {
        private readonly FactionRecord _factionRecord;

        public string RaceTypeName
        {
            get
            {
                var gameMetadata = CallContext<GameMetadata>.GetData("GameMetadata");
                var raceType = gameMetadata.RaceTypes[_factionRecord.RaceTypeId];
                var name = raceType.Name;

                return name;
            }
        }

        public int GoldInTreasury => _factionRecord.GoldInTreasury;

        public int GoldPerTurn => _factionRecord.GoldPerTurn;

        public int ManaInTreasury => _factionRecord.ManaInTreasury;

        public int ManaPerTurn => _factionRecord.ManaPerTurn;

        public int FoodPerTurn
        {
            get
            {
                var world = CallContext<World>.GetData("GameWorld");

                return world.Settlements.FoodProducedThisTurn;
            }
        }

        public Faction(int raceTypeId)
        {
            var gameDataRepository = CallContext<GameDataRepository>.GetData("GameDataRepository");
            _factionRecord = new FactionRecord(raceTypeId);
            gameDataRepository.Add(_factionRecord);
        }
    }
}