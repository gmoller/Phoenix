using System.Diagnostics;
using PhoenixGameConfig;
using PhoenixGameData;
using PhoenixGameData.Tuples;
using Zen.Utilities;

namespace PhoenixGameLibrary
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Faction
    {
        private readonly GameConfigCache _gameConfigCache;
        private readonly GameDataRepository _gameDataRepository;

        private FactionRecord _factionRecord;

        public string RaceName => GetRaceName();
        public int GoldInTreasury => _factionRecord.GoldInTreasury.Value;
        public int GoldPerTurn => 0;
        public int ManaInTreasury => _factionRecord.ManaInTreasury.Value;
        public int ManaPerTurn => 0;
        public int FoodPerTurn => GetFoodPerTurn();

        public Faction(int factionId)
        {
            _gameConfigCache = CallContext<GameConfigCache>.GetData("GameConfigCache");
            _gameDataRepository = CallContext<GameDataRepository>.GetData("GameDataRepository");
            _factionRecord = _gameDataRepository.GetFactionById(factionId);

            _gameDataRepository.FactionUpdated += FactionUpdated;
        }

        private void FactionUpdated(object sender, FactionRecord factionRecord)
        {
            if (factionRecord.Id == _factionRecord.Id)
            {
                _factionRecord = factionRecord;
            }
        }

        private string GetRaceName()
        {
            var race = _gameConfigCache.GetRaceConfigById(_factionRecord.RaceTypeId.Value);
            var name = race.Name;

            return name;
        }

        private int GetFoodPerTurn()
        {
            var world = CallContext<World>.GetData("GameWorld");
            var foodPerTurn = world.Settlements.FoodProducedThisTurn;

            return foodPerTurn;
        }

        public override string ToString() => DebuggerDisplay;

        private string DebuggerDisplay => $"{{Id={_factionRecord.Id}}}";
    }
}