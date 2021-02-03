using System.Collections.Generic;
using PhoenixGameConfig;
using Zen.Utilities;

namespace PhoenixGameData.GameData
{
    public class RaceConfig
    {
        private GameConfigRepository _gameConfigRepository;

        private readonly dynamic _race;
        private readonly DynamicDataList _raceTownNames;
        private readonly DynamicDataList _unitRaces;
        private readonly DynamicDataList _raceBuildings;

        public int Id => (int)_race.Id;
        public string Name => (string)_race.Name;
        public float FarmingRate => (float)_race.FarmingRate;
        public float GrowthRateModifier => (float)_race.GrowthRateModifier;
        public float FarmerProductionRate => (float)_race.FarmerProductionRate;
        public float WorkerProductionRate => (float)_race.WorkerProductionRate;
        public List<string> TownNamesForThisRace { get; }
        public List<string> UnitsThatCanBeBuiltByThisRace { get; }
        public List<string> BuildingsThatCanBeBuiltByThisRace { get; }

        public RaceConfig(int raceId)
        {
            _gameConfigRepository = CallContext<GameConfigRepository>.GetData("GameConfigRepository");
            var races = _gameConfigRepository.GetEntities("Race");

            _race = races.GetById(raceId);
            _raceTownNames = _gameConfigRepository.GetEntities("RaceTownName").Filter("RaceId", raceId);
            _unitRaces = _gameConfigRepository.GetEntities("UnitRace").Filter("RaceId", raceId);
            _raceBuildings = _gameConfigRepository.GetEntities("RaceBuilding").Filter("RaceId", raceId);

            TownNamesForThisRace = _gameConfigRepository.ToList(_raceTownNames, "TownName", "TownNameId", "Name");
            UnitsThatCanBeBuiltByThisRace = _gameConfigRepository.ToList(_unitRaces, "Unit", "UnitId", "Name");
            BuildingsThatCanBeBuiltByThisRace = _gameConfigRepository.ToList(_raceBuildings, "Building", "BuildingId", "Name");
        }

        public string GetRandomTownName()
        {
            var chosenIndex = RandomNumberGenerator.Instance.GetRandomInt(0, TownNamesForThisRace.Count - 1);
            var raceTownName = TownNamesForThisRace[chosenIndex];

            return raceTownName;
        }
    }
}