using PhoenixGameData;
using PhoenixGameData.Tuples;
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

        public int GoldInTreasury
        {
            get => _factionRecord.GoldInTreasury;
            set => _factionRecord.GoldInTreasury = value;
        }

        public int GoldPerTurn => 0;

        public int ManaInTreasury
        {
            get => _factionRecord.ManaInTreasury;
            set => _factionRecord.ManaInTreasury = value;
        }

        public int ManaPerTurn => 0;

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