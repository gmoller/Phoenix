using System;
using System.Collections.Generic;
using GameLogic;
using PhoenixGameLibrary.Commands;
using PhoenixGameLibrary.GameData;
using PhoenixGameLibrary.Helpers;
using Utilities;

namespace PhoenixGameLibrary
{
    /// <summary>
    /// A settlement is an immovable game entity that can be controlled by the
    /// player/AI to do things such as build new units and add buildings to
    /// improve the settlement.
    /// </summary>
    public class Settlement
    {
        private readonly int _id;

        private readonly List<Cell> _catchmentCells;
        private List<int> _buildingsBuilt;
        private int _populationGrowth;
        private CurrentlyBuilding _currentlyBuilding;

        public string Name { get; }
        public RaceType RaceType { get; }
        public Point Location { get; }
        public int Population => Citizens.TotalPopulation * 1000 + _populationGrowth; // every 1 citizen is 1000 population
        public int GrowthRate => DetermineGrowthRate();
        public int BaseFoodLevel => (int)Helpers.BaseFoodLevel.DetermineBaseFoodLevel(Location, _catchmentCells);
        public FoodBreakdown SettlementFoodProduction => Helpers.SettlementFoodProduction.DetermineFoodProduction(this, _buildingsBuilt);
        public int SettlementProduction => Helpers.SettlementProduction.DetermineProduction(this, _catchmentCells);
        public int FoodSubsistence => Citizens.TotalPopulation;
        public int FoodSurplus => SettlementFoodProduction.TotalFood - Citizens.TotalPopulation;
        //public int GoldUpkeep => DetermineGoldUpkeep();
        //public int GoldSurplus => DetermineGoldSurplus();
        //private CurrentlyBuilding CurrentlyBuilding { get; set; }

        public SettlementCitizens Citizens { get; }

        public string CurrentlyBuilding
        {
            get
            {
                if (_currentlyBuilding.BuildingId >= 0)
                {
                    var building = Globals.Instance.BuildingTypes[_currentlyBuilding.BuildingId];
                    return $"Current: {building.Name} ({_currentlyBuilding.ProductionAccrued}/{building.ConstructionCost})";
                }
                if (_currentlyBuilding.UnitId >= 0)
                {
                    var unit = Globals.Instance.UnitTypes[_currentlyBuilding.UnitId];
                    return $"Current: {unit.Name} ({_currentlyBuilding.ProductionAccrued}/{unit.ConstructionCost})";
                }

                return "Current: <nothing>";
            }
        }

        public SettlementType SettlementType
        {
            get
            {
                if (Citizens.TotalPopulation <= 4) return SettlementType.Hamlet;
                if (Citizens.TotalPopulation >= 5 && Citizens.TotalPopulation <= 8) return SettlementType.Village;
                if (Citizens.TotalPopulation >= 9 && Citizens.TotalPopulation <= 12) return SettlementType.Town;
                if (Citizens.TotalPopulation >= 13 && Citizens.TotalPopulation <= 16) return SettlementType.City;
                if (Citizens.TotalPopulation >= 17) return SettlementType.Capital;

                throw new Exception("Unknown settlement type.");
            }
        }

        public bool IsSelected { get; set; }

        public Settlement(string name, string raceTypeName, Point location, byte settlementSize, CellGrid cellGrid, params string[] buildings)
        {
            _id = (location.Y * Constants.WORLD_MAP_COLUMNS) + location.X;

            Name = name;
            RaceType = Globals.Instance.RaceTypes[raceTypeName];
            Location = location;
            _populationGrowth = 0;
            _currentlyBuilding = new CurrentlyBuilding(-1, -1, 0);
            _buildingsBuilt = new List<int>();
            foreach (var building in buildings)
            {
                _buildingsBuilt.Add(Globals.Instance.BuildingTypes[building].Id);
            }
            Citizens = new SettlementCitizens(this, settlementSize, _buildingsBuilt);

            _catchmentCells = cellGrid.GetCatchment(location.X, location.Y);
            foreach (var item in _catchmentCells)
            {
                var cell = cellGrid.GetCell(item.Column, item.Row);
                cell.BelongsToSettlement = _id;
                cellGrid.SetCell(item.Column, item.Row, cell);
            }
        }

        public void Update(float deltaTime)
        {
        }

        public bool BuildingHasBeenBuilt(string buildingName)
        {
            var building = Globals.Instance.BuildingTypes[buildingName];

            return _buildingsBuilt.Contains(building.Id);
        }

        public bool BuildingCanBeBuilt(string buildingName)
        {
            var building = Globals.Instance.BuildingTypes[buildingName];

            return building.CanBeBuiltBy(RaceType.Name);
        }

        public bool BuildingCanNotBeBuilt(string buildingName)
        {
            var building = Globals.Instance.BuildingTypes[buildingName];

            return building.CanNotBeBuiltBy(RaceType.Name);
        }

        public bool BuildingReadyToBeBeBuilt(string buildingName)
        {
            var building = Globals.Instance.BuildingTypes[buildingName];
            var isReadyTobeBuilt = building.IsReadyToBeBuilt(_buildingsBuilt);

            return isReadyTobeBuilt;
        }

        public bool UnitCanBeBuilt(string unitName)
        {
            var unit = Globals.Instance.UnitTypes[unitName];
            if (unit.CanBeBuiltBy(RaceType.Name) && unit.IsReadyToBeBuilt(_buildingsBuilt))
            {
                return true;
            }

            return false;
        }

        public void AddToProductionQueue(BuildingType building)
        {
            _currentlyBuilding = new CurrentlyBuilding(building.Id, -1, _currentlyBuilding.ProductionAccrued);
        }

        public void AddToProductionQueue(UnitType unit)
        {
            _currentlyBuilding = new CurrentlyBuilding(-1, unit.Id, _currentlyBuilding.ProductionAccrued);
        }

        public void EndTurn()
        {
            _populationGrowth += GrowthRate;
            if (_populationGrowth >= 1000 && Citizens.TotalPopulation < Constants.MAXIMUM_POPULATION_SIZE)
            {
                Citizens.IncreaseByOne(_buildingsBuilt);
                _populationGrowth = 0;
                Globals.Instance.World.NotificationList.Add($"- {Name} has grown to a population of {Citizens.TotalPopulation}");
            }

            if (_currentlyBuilding.BuildingId >= 0)
            {
                _currentlyBuilding = new CurrentlyBuilding(_currentlyBuilding.BuildingId, -1, _currentlyBuilding.ProductionAccrued + SettlementProduction);
                if (_currentlyBuilding.ProductionAccrued >= Globals.Instance.BuildingTypes[_currentlyBuilding.BuildingId].ConstructionCost)
                {
                    _buildingsBuilt.Add(_currentlyBuilding.BuildingId);
                    Globals.Instance.World.NotificationList.Add($"- {Name} has produced a {Globals.Instance.BuildingTypes[_currentlyBuilding.BuildingId].Name}");
                    _currentlyBuilding = new CurrentlyBuilding(-1, -1, 0);
                    Command openSettlementCommand = new OpenSettlementCommand();
                    openSettlementCommand.Payload = this;
                    Globals.Instance.MessageQueue.Enqueue(openSettlementCommand);
                    // TODO: look at settlement
                }
            }
            if (_currentlyBuilding.UnitId >= 0)
            {
                _currentlyBuilding = new CurrentlyBuilding(-1, _currentlyBuilding.UnitId, _currentlyBuilding.ProductionAccrued + SettlementProduction);
                if (_currentlyBuilding.ProductionAccrued >= Globals.Instance.UnitTypes[_currentlyBuilding.UnitId].ConstructionCost)
                {
                    var unitType = Globals.Instance.UnitTypes[_currentlyBuilding.UnitId];
                    Globals.Instance.World.NotificationList.Add($"- {Name} has produced a {unitType.Name}");
                    _currentlyBuilding = new CurrentlyBuilding(-1, -1, 0);

                    var addUnitCommand = new AddUnitCommand
                    {
                        Payload = (Location, unitType)
                    };
                    Globals.Instance.MessageQueue.Enqueue(addUnitCommand);
                }
            }
        }

        private int DetermineGrowthRate()
        {
            int maxSettlementSize = DetermineMaximumSettlementSize();
            int growthRate = PopulationGrowthRate.DetermineGrowthRate(maxSettlementSize, Citizens.TotalPopulation, RaceType, _buildingsBuilt);

            return growthRate;
        }

        private int DetermineMaximumSettlementSize()
        {
            int baseFoodLevel = BaseFoodLevel;

            // buildings
            foreach (var item in Globals.Instance.BuildingMaximumPopulationIncreaseTypes)
            {
                if (_buildingsBuilt.Contains(item.Id))
                {
                    baseFoodLevel += item.MaximumPopulationIncrease;
                }
            }

            return baseFoodLevel > Constants.MAXIMUM_POPULATION_SIZE ? Constants.MAXIMUM_POPULATION_SIZE : baseFoodLevel;
        }
    }

    public struct CurrentlyBuilding
    {
        public int BuildingId { get; }
        public int UnitId { get; }
        public int ProductionAccrued { get; }

        public CurrentlyBuilding(int buildingId, int unitId, int productionAccrued)
        {
            BuildingId = buildingId;
            UnitId = unitId;
            ProductionAccrued = productionAccrued;
        }
    }

    public enum SettlementType
    {
        Outpost,
        Hamlet,
        Village,
        Town,
        City,
        Capital
    }
}