using System;
using System.Collections.Generic;
using System.Diagnostics;
using PhoenixGameLibrary.GameData;
using PhoenixGameLibrary.Helpers;
using Zen.Hexagons;
using Zen.Utilities;

namespace PhoenixGameLibrary
{
    /// <summary>
    /// A settlement is an immovable game entity that can be controlled by the
    /// player/AI to do things such as build new units and add buildings to
    /// improve the settlement.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Settlement
    {
        #region State
        private readonly GameMetadata _gameMetadata;
        private readonly World _world;

        private readonly int _id;

        private readonly List<Cell> _catchmentCells;
        private readonly List<Cell> _seenCells;
        private readonly List<int> _buildingsBuilt;
        private int _populationGrowth;
        private CurrentlyBuilding _currentlyBuilding;

        public string Name { get; }
        public RaceType RaceType { get; }

        public PointI Location { get; }

        public SettlementCitizens Citizens { get; }

        public event EventHandler SettlementOpened;
        #endregion End State

        public HexOffsetCoordinates LocationHex => new HexOffsetCoordinates(Location.X, Location.Y);
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
        public string CurrentlyBuilding => GetCurrentlyBuilding();
        public SettlementType SettlementType => GetSettlementType();

        public Settlement(World world, string name, string raceTypeName, PointI location, byte settlementSize, CellGrid cellGrid, params string[] buildings)
        {
            _gameMetadata = CallContext<GameMetadata>.GetData("GameMetadata");
            var raceTypes = _gameMetadata.RaceTypes;
            var buildingTypes = _gameMetadata.BuildingTypes;

            _world = world;
            _id = location.Y * Constants.WORLD_MAP_COLUMNS + location.X;

            Name = name;
            RaceType = raceTypes[raceTypeName];
            Location = location;
            _populationGrowth = 0;
            _currentlyBuilding = new CurrentlyBuilding(-1, -1, -1, 0);
            _buildingsBuilt = new List<int>();
            foreach (var building in buildings)
            {
                _buildingsBuilt.Add(buildingTypes[building].Id);
            }
            Citizens = new SettlementCitizens(this, settlementSize, _buildingsBuilt);

            // control
            _catchmentCells = cellGrid.GetCatchment(location.X, location.Y, 2);
            foreach (var item in _catchmentCells)
            {
                var cell = cellGrid.GetCell(item.Column, item.Row);
                cellGrid.SetCell(cell, cell.SeenState, 1);
            }
            _catchmentCells = cellGrid.GetCatchment(location.X, location.Y, 2);
            cellGrid.CellFactionChange(_catchmentCells);

            // sight
            _seenCells = cellGrid.GetCatchment(location.X, location.Y, 3);
            foreach (var item in _seenCells)
            {
                var cell = cellGrid.GetCell(item.Column, item.Row);
                cellGrid.SetCell(cell, SeenState.CurrentlySeen);
            }
        }

        public bool BuildingHasBeenBuilt(string buildingName)
        {
            var buildingTypes = _gameMetadata.BuildingTypes;
            var building = buildingTypes[buildingName];

            return _buildingsBuilt.Contains(building.Id);
        }

        public bool BuildingCanBeBuilt(string buildingName)
        {
            var buildingTypes = _gameMetadata.BuildingTypes;
            var building = buildingTypes[buildingName];

            return building.CanBeBuiltBy(RaceType.Name);
        }

        public bool BuildingCanNotBeBuilt(string buildingName)
        {
            var buildingTypes = _gameMetadata.BuildingTypes;
            var building = buildingTypes[buildingName];

            return building.CanNotBeBuiltBy(RaceType.Name);
        }

        public bool BuildingReadyToBeBeBuilt(string buildingName)
        {
            var buildingTypes = _gameMetadata.BuildingTypes;
            var building = buildingTypes[buildingName];
            var isReadyToBeBuilt = building.IsReadyToBeBuilt(_buildingsBuilt);

            return isReadyToBeBuilt;
        }

        public bool UnitCanBeBuilt(string unitName)
        {
            var unitTypes = _gameMetadata.UnitTypes;
            var unit = unitTypes[unitName];
            if (unit.CanBeBuiltBy(RaceType.Name) && unit.IsReadyToBeBuilt(_buildingsBuilt))
            {
                return true;
            }

            return false;
        }

        public void AddToProductionQueue(BuildingType building)
        {
            _currentlyBuilding = new CurrentlyBuilding(building.Id, -1, -1, _currentlyBuilding.ProductionAccrued);
        }

        public void AddToProductionQueue(UnitType unit)
        {
            _currentlyBuilding = new CurrentlyBuilding(-1, unit.Id, -1, _currentlyBuilding.ProductionAccrued);
        }

        //public void AddToProductionQueue(OtherType other)
        //{
        //    _currentlyBuilding = new CurrentlyBuilding(-1, -1, other.Id, _currentlyBuilding.ProductionAccrued);
        //}

        internal void BeginTurn()
        {
        }

        internal void EndTurn()
        {
            _populationGrowth += GrowthRate;
            if (_populationGrowth >= 1000 && Citizens.TotalPopulation < Constants.MAXIMUM_POPULATION_SIZE)
            {
                Citizens.IncreaseByOne(_buildingsBuilt);
                _populationGrowth = 0;
                _world.NotificationList.Add($"- {Name} has grown to a population of {Citizens.TotalPopulation}");
            }

            if (_currentlyBuilding.BuildingId >= 0)
            {
                var buildingTypes = _gameMetadata.BuildingTypes;

                _currentlyBuilding = new CurrentlyBuilding(_currentlyBuilding.BuildingId, -1, -1, _currentlyBuilding.ProductionAccrued + SettlementProduction);
                if (_currentlyBuilding.ProductionAccrued >= buildingTypes[_currentlyBuilding.BuildingId].ConstructionCost)
                {
                    _buildingsBuilt.Add(_currentlyBuilding.BuildingId);
                    _world.NotificationList.Add($"- {Name} has produced a {buildingTypes[_currentlyBuilding.BuildingId].Name}");
                    _currentlyBuilding = new CurrentlyBuilding(-1, -1, -1, 0);

                    OnSettlementOpened(EventArgs.Empty);
                }
            }
            if (_currentlyBuilding.UnitId >= 0)
            {
                var unitTypes = _gameMetadata.UnitTypes;

                _currentlyBuilding = new CurrentlyBuilding(-1, _currentlyBuilding.UnitId, -1, _currentlyBuilding.ProductionAccrued + SettlementProduction);
                if (_currentlyBuilding.ProductionAccrued >= unitTypes[_currentlyBuilding.UnitId].ConstructionCost)
                {
                    var unitType = unitTypes[_currentlyBuilding.UnitId];
                    _world.AddUnit(Location, unitType);
                    _world.NotificationList.Add($"- {Name} has produced a {unitType.Name}");
                    _currentlyBuilding = new CurrentlyBuilding(-1, _currentlyBuilding.UnitId, -1, 0);
                }
            }
            if (_currentlyBuilding.OtherId >= 0)
            {
                // TODO: others
            }
        }

        private void OnSettlementOpened(EventArgs e)
        {
            SettlementOpened?.Invoke(this, e);
        }

        internal bool CanSeeCell(Cell cell)
        {
            // if cell is within 3 hexes
            foreach (var item in _seenCells)
            {
                if (cell.Column == item.Column && cell.Row == item.Row)
                {
                    return true;
                }
            }

            return false;
        }

        private string GetCurrentlyBuilding()
        {
            if (_currentlyBuilding.BuildingId >= 0)
            {
                var buildingTypes = _gameMetadata.BuildingTypes;
                var building = buildingTypes[_currentlyBuilding.BuildingId];

                return $"Current: {building.Name} ({_currentlyBuilding.ProductionAccrued}/{building.ConstructionCost})";
            }
            if (_currentlyBuilding.UnitId >= 0)
            {
                var unitTypes = _gameMetadata.UnitTypes;
                var unit = unitTypes[_currentlyBuilding.UnitId];

                return $"Current: {unit.Name} ({_currentlyBuilding.ProductionAccrued}/{unit.ConstructionCost})";
            }

            return "Current: <nothing>";
        }

        private SettlementType GetSettlementType()
        {
            if (Citizens.TotalPopulation <= 4) return SettlementType.Hamlet;
            if (Citizens.TotalPopulation >= 5 && Citizens.TotalPopulation <= 8) return SettlementType.Village;
            if (Citizens.TotalPopulation >= 9 && Citizens.TotalPopulation <= 12) return SettlementType.Town;
            if (Citizens.TotalPopulation >= 13 && Citizens.TotalPopulation <= 16) return SettlementType.City;
            if (Citizens.TotalPopulation >= 17) return SettlementType.Capital;

            throw new Exception("Unknown settlement type.");
        }

        private int DetermineGrowthRate()
        {
            var maxSettlementSize = DetermineMaximumSettlementSize();
            var growthRate = PopulationGrowthRate.DetermineGrowthRate(maxSettlementSize, Citizens.TotalPopulation, RaceType, _buildingsBuilt);

            return growthRate;
        }

        private int DetermineMaximumSettlementSize()
        {
            var baseFoodLevel = BaseFoodLevel;

            // settlement buildings
            var buildingMaximumPopulationIncreaseTypes = _gameMetadata.BuildingMaximumPopulationIncreaseTypes;
            foreach (var item in buildingMaximumPopulationIncreaseTypes)
            {
                if (_buildingsBuilt.Contains(item.BuildingId))
                {
                    baseFoodLevel += item.MaximumPopulationIncrease;
                }
            }

            return baseFoodLevel > Constants.MAXIMUM_POPULATION_SIZE ? Constants.MAXIMUM_POPULATION_SIZE : baseFoodLevel;
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Id={_id},Name={Name}}}";
    }

    public readonly struct CurrentlyBuilding
    {
        public int BuildingId { get; }
        public int UnitId { get; }
        public int OtherId { get; }
        public int ProductionAccrued { get; }

        public CurrentlyBuilding(int buildingId, int unitId, int otherId, int productionAccrued)
        {
            BuildingId = buildingId;
            UnitId = unitId;
            OtherId = otherId;
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