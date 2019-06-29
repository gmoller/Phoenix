using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using PhoenixGameLibrary.GameData;
using PhoenixGameLibrary.Helpers;
using PhoenixGameLibrary.Views;

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

        private int _populationGrowth;

        public string Name { get; }
        public RaceType RaceType { get; }
        public Point Location { get; }
        public int Population => Citizens.TotalPopulation * 1000 + _populationGrowth; // every 1 citizen is 1000 population
        public int BaseFoodLevel => (int)Helpers.BaseFoodLevel.DetermineBaseFoodLevel(Location, _cellGrid);
        public int GrowthRate => DetermineGrowthRate();
        public int SettlementFoodProduction => Helpers.SettlementFoodProduction.DetermineFoodProduction(this);
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

        public Settlement(string name, RaceType raceType, Point location, byte settlementSize, CellGrid cellGrid)
        {
            _cellGrid = cellGrid;
            Name = name;
            RaceType = raceType;
            Location = location;
            Citizens = new SettlementCitizens(this, settlementSize);
            _populationGrowth = 0;

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

        public void Draw()
        {
            _overlandSettlementView.Draw();
            View.Draw();
        }

        public void EndTurn()
        {
            _populationGrowth += GrowthRate;
            if (_populationGrowth >= 1000 && Citizens.TotalPopulation < Constants.MAXIMUM_POPULATION_SIZE)
            {
                Citizens.IncreaseByOne();
                _populationGrowth = 0;
            }
        }

        private int DetermineGrowthRate()
        {
            int maxSettlementSize = DetermineMaximumSettlementSize();
            int growthRate = PopulationGrowthRate.DetermineGrowthRate(maxSettlementSize, Citizens.TotalPopulation, RaceType);

            return growthRate;
        }

        private int DetermineMaximumSettlementSize()
        {
            int baseFoodLevel = BaseFoodLevel;

            return baseFoodLevel > Constants.MAXIMUM_POPULATION_SIZE ? Constants.MAXIMUM_POPULATION_SIZE : baseFoodLevel;
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