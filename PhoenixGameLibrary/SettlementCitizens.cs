using System;

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

        public SettlementCitizens(Settlement settlement, int settlementSize)
        {
            _settlement = settlement;

            SubsistenceFarmers = CalculateSubsistenceFarmers(settlementSize);
            AdditionalFarmers = settlementSize - SubsistenceFarmers;
            Workers = 0;
        }

        public void IncreaseByOne()
        {
            var subsistenceFarmers = CalculateSubsistenceFarmers(TotalPopulation + 1);

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

        public void ReassignCitizens(float farmersRatio)
        {
            var sum = AdditionalFarmers + Workers;
            var farmers = sum / farmersRatio;

            AdditionalFarmers = (byte)Math.Round(farmers);
            Workers = (byte)(sum - AdditionalFarmers);
        }

        private int CalculateSubsistenceFarmers(int totalPopulation)
        {
            // TODO: buldings, wild game not being factored in
            var foodUpkeep = totalPopulation;

            var farmersSubsistenceFloat = foodUpkeep / 2.0f; // 2 is farming rate: TODO: halfling race has higher farming rate
            var farmersSubsistence = (int)Math.Ceiling(farmersSubsistenceFloat);

            return farmersSubsistence;
        }
    }
}