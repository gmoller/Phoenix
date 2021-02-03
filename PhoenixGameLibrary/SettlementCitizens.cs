using System;
using System.Collections.Generic;
using PhoenixGameConfig;
using PhoenixGameData;
using Zen.Utilities;

namespace PhoenixGameLibrary
{
    public class SettlementCitizens
    {
        private readonly Settlement _settlement;

        public int SubsistenceFarmers { get; private set; }
        public int AdditionalFarmers { get; private set; }
        public int Farmers => SubsistenceFarmers + AdditionalFarmers;
        public int Workers { get; private set; }
        //private int _rebels; // TODO: support rebels

        public int TotalPopulation => SubsistenceFarmers + AdditionalFarmers + Workers; // _rebels

        internal SettlementCitizens(Settlement settlement, int settlementSize, List<int> buildingsBuilt)
        {
            _settlement = settlement;

            SubsistenceFarmers = CalculateSubsistenceFarmers(settlementSize, buildingsBuilt);
            AdditionalFarmers = settlementSize - SubsistenceFarmers;
            Workers = 0;
        }

        internal void IncreaseByOne(List<int> buildingsBuilt)
        {
            var subsistenceFarmers = CalculateSubsistenceFarmers(TotalPopulation + 1, buildingsBuilt);

            if (subsistenceFarmers > SubsistenceFarmers)
            {
                SubsistenceFarmers++;
            }
            else
            {
                AdditionalFarmers++;
            }
        }

        public void ConvertFarmerToWorker()
        {
            if (AdditionalFarmers > 0)
            { 
                AdditionalFarmers--;
                Workers++;
            }
        }

        public void ConvertWorkerToFarmer()
        {
            if (Workers > 0)
            {
                AdditionalFarmers++;
                Workers--;
            }
        }

        //public void ReassignCitizens(float farmersRatio)
        //{
        //    var sum = AdditionalFarmers + Workers;
        //    var farmers = sum / farmersRatio;

        //    AdditionalFarmers = (byte)Math.Round(farmers);
        //    Workers = (byte)(sum - AdditionalFarmers);
        //}

        private int CalculateSubsistenceFarmers(int totalPopulation, List<int> buildingsBuilt)
        {
            // TODO: wild game not being factored in

            // buildings
            var gameConfigCache = CallContext<GameConfigCache>.GetData("GameConfigCache");

            var freeFood = 0.0;
            foreach (var item in buildingsBuilt)
            {
                var building = gameConfigCache.GetBuildingConfigById(item);
                freeFood += building.FoodOutputIncrease;
            }

            var foodUpkeep = totalPopulation - (int)freeFood;

            var farmersSubsistenceFloat = foodUpkeep / (float)_settlement.Race.FarmingRate;
            var farmersSubsistence = (int)Math.Ceiling(farmersSubsistenceFloat);

            return farmersSubsistence;
        }
    }
}