using System;
using System.Collections.Generic;
using GameLogic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using PhoenixGameLibrary.GameData;
using PhoenixGameLibrary.Helpers;
using PhoenixGameLibrary.Views;
using PhoenixGameLibrary.Views.SettlementView;

namespace PhoenixGameLibrary
{
    /// <summary>
    /// A settlement is an immovable game entity that can be controlled by the
    /// player/AI to do things such as build new units and add buildings to
    /// improve the settlement.
    /// </summary>
    public class Settlement
    {
        private readonly CellGrid _cellGrid;
        private OverlandSettlementView _overlandSettlementView;
        private List<int> _buildingsBuilt;
        private int _populationGrowth;

        public string Name { get; }
        public RaceType RaceType { get; }
        public Point Location { get; }
        public int Population => Citizens.TotalPopulation * 1000 + _populationGrowth; // every 1 citizen is 1000 population
        public int BaseFoodLevel => (int)Helpers.BaseFoodLevel.DetermineBaseFoodLevel(Location, _cellGrid);
        public int GrowthRate => DetermineGrowthRate();
        public int SettlementFoodProduction => Helpers.SettlementFoodProduction.DetermineFoodProduction(this, _buildingsBuilt);
        public int FoodSurplus => SettlementFoodProduction - Citizens.TotalPopulation;
        //public int GoldUpkeep => DetermineGoldUpkeep();
        //public int GoldSurplus => DetermineGoldSurplus();
        public CurrentlyBuilding CurrentlyBuilding { get; private set; }

        public int SettlementProduction => Helpers.SettlementProduction.DetermineProduction(this, _cellGrid);

        public SettlementCitizens Citizens { get; }
        public SettlementView View { get; }

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

        public Settlement(string name, string raceTypeName, Point location, byte settlementSize, CellGrid cellGrid, params string[] buildings)
        {
            _cellGrid = cellGrid;
            Name = name;
            RaceType = Globals.Instance.RaceTypes[raceTypeName];
            Location = location;
            _populationGrowth = 0;
            CurrentlyBuilding = new CurrentlyBuilding(-1, 0);
            _buildingsBuilt = new List<int>();
            foreach (var building in buildings)
            {
                _buildingsBuilt.Add(Globals.Instance.BuildingTypes[building].Id);
            }
            Citizens = new SettlementCitizens(this, settlementSize, _buildingsBuilt);

            View = new SettlementView(this);
            _overlandSettlementView = new OverlandSettlementView(this);
        }

        public void LoadContent(ContentManager content)
        {
            _overlandSettlementView.LoadContent(content);
            View.LoadContent(content);
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
            _overlandSettlementView.Update(gameTime, input);
            View.Update(gameTime, input);
        }

        public void DrawOverland()
        {
            _overlandSettlementView.Draw();
        }

        public void DrawSettlement()
        {
            View.Draw();
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

        public void AddToProductionQueue(BuildingType building)
        {
            CurrentlyBuilding = new CurrentlyBuilding(building.Id, CurrentlyBuilding.ProductionAccrued);
        }

        public void EndTurn()
        {
            _populationGrowth += GrowthRate;
            if (_populationGrowth >= 1000 && Citizens.TotalPopulation < Constants.MAXIMUM_POPULATION_SIZE)
            {
                Citizens.IncreaseByOne(_buildingsBuilt);
                _populationGrowth = 0;
            }

            if (CurrentlyBuilding.BuildingId != -1)
            {
                CurrentlyBuilding = new CurrentlyBuilding(CurrentlyBuilding.BuildingId, CurrentlyBuilding.ProductionAccrued + SettlementProduction);
                if (CurrentlyBuilding.ProductionAccrued >= Globals.Instance.BuildingTypes[CurrentlyBuilding.BuildingId].ConstructionCost)
                {
                    _buildingsBuilt.Add(CurrentlyBuilding.BuildingId);
                    CurrentlyBuilding = new CurrentlyBuilding(-1, 0);
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
                if (_buildingsBuilt.Contains(item.BuildingId))
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
        public int ProductionAccrued { get; }

        public CurrentlyBuilding(int buildingId, int productionAccrued)
        {
            BuildingId = buildingId;
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