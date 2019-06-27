using System;

namespace PhoenixGameLibrary
{
    public class SettlementCitizens
    {
        private readonly Settlement _settlement;

        public byte SubsistenceFarmers { get; private set; }
        public byte AdditionalFarmers { get; private set; }
        public byte Workers { get; private set; }
        //private byte _rebels; // TODO: support rebels

        public byte TotalPopulation => (byte)(SubsistenceFarmers + AdditionalFarmers + Workers); // _rebels

        public SettlementCitizens(Settlement settlement, byte settlementSize)
        {
            _settlement = settlement;

            SubsistenceFarmers = CalculateSubsistenceFarmers(settlementSize);
            AdditionalFarmers = (byte)(settlementSize - SubsistenceFarmers);
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

        private byte CalculateSubsistenceFarmers(int totalPopulation)
        {
            // TODO: buldings, wild game not being factored in
            var foodUpkeep = totalPopulation;

            var farmersSubsistenceFloat = foodUpkeep / 2.0f; // 2 is farming rate: TODO: halfling race has higher farming rate
            var farmersSubsistence = (byte)Math.Ceiling(farmersSubsistenceFloat);

            return farmersSubsistence;
        }
    }
}