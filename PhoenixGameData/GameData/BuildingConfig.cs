using System.Collections.Generic;
using PhoenixGameConfig;
using Zen.Utilities;

namespace PhoenixGameData.GameData
{
    public class BuildingConfig
    {
        private readonly GameConfigRepository _gameConfigRepository;

        private readonly dynamic _building;
        private readonly DynamicDataList _raceBuildings;
        private readonly DynamicDataList _unitBuildings;
        private readonly DynamicDataList _requiredBuildings;

        public int Id => (int)_building.Id;
        public string Name => (string)_building.Name;
        public PointI Slot => new PointI((int)_building.Slot.X, (int)_building.Slot.Y);
        public float ConstructionCost => (float)_building.ConstructionCost;
        public float UpkeepGold => (float)_building.UpkeepGold;
        public float UpkeepMana => (float)_building.UpkeepMana;
        public List<string> RacesThatCanBuildThisBuilding { get; }
        public List<string> UnitsThatAreRequiredByThisBuilding { get; }
        public List<string> BuildingsRequiredToBuildThisBuilding { get; }
        public float MaximumPopulationIncrease { get; }
        public float PopulationGrowthRateIncrease { get; }
        public float FoodOutputIncrease { get; }

        public BuildingConfig(int buildingId)
        {
            _gameConfigRepository = CallContext<GameConfigRepository>.GetData("GameConfigRepository");
            var buildings = _gameConfigRepository.GetEntities("Building");

            _building = buildings.GetById(buildingId);
            _raceBuildings = _gameConfigRepository.GetEntities("RaceBuilding").Filter("BuildingId", buildingId);
            _unitBuildings = _gameConfigRepository.GetEntities("UnitBuilding").Filter("BuildingId", buildingId);
            _requiredBuildings = _gameConfigRepository.GetEntities("RequiredBuilding").Filter("BuildingId", buildingId);

            RacesThatCanBuildThisBuilding = _gameConfigRepository.ToList(_raceBuildings, "Race", "RaceId", "Name");
            UnitsThatAreRequiredByThisBuilding = _gameConfigRepository.ToList(_unitBuildings, "Unit", "UnitId", "Name");
            BuildingsRequiredToBuildThisBuilding = _gameConfigRepository.ToList(_requiredBuildings, "Building", "RequiredBuildingId", "Name");

            MaximumPopulationIncrease = DetermineMaximumPopulationIncrease(buildingId);
            PopulationGrowthRateIncrease = DeterminePopulationGrowthRateIncrease(buildingId);
            FoodOutputIncrease = DetermineFoodOutputIncrease(buildingId);
        }

        private float DetermineMaximumPopulationIncrease(int buildingId)
        {
            var buildingMaximumPopulationIncreases = _gameConfigRepository.GetEntities("BuildingMaximumPopulationIncrease");
            var filtered = buildingMaximumPopulationIncreases.Filter("BuildingId", buildingId);

            var amt = 0.0f;
            foreach (var item in filtered)
            {
                amt += (float)item.Value;
            }

            return amt;
        }

        private float DeterminePopulationGrowthRateIncrease(int buildingId)
        {
            var buildingPopulationGrowthRateIncrease = _gameConfigRepository.GetEntities("BuildingPopulationGrowthRateIncrease");
            var filtered = buildingPopulationGrowthRateIncrease.Filter("BuildingId", buildingId);

            var amt = 0.0f;
            foreach (var item in filtered)
            {
                amt += (float)item.Value;
            }

            return amt;
        }

        private float DetermineFoodOutputIncrease(int buildingId)
        {
            var buildingFoodOutputIncrease = _gameConfigRepository.GetEntities("BuildingFoodOutputIncrease");
            var filtered = buildingFoodOutputIncrease.Filter("BuildingId", buildingId);

            var amt = 0.0f;
            foreach (var item in filtered)
            {
                amt += (float)item.Value;
            }

            return amt;
        }
    }
}