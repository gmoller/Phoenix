namespace PhoenixGameLibrary.Helpers
{
    public static class SettlementFoodProduction
    {
        public static int DetermineFoodProduction(Settlement settlement)
        {
            // https://masterofmagic.fandom.com/wiki/Food
            // TODO: Animists' Guild, Foresters' Guild, Famine, Granary, Farmers' Market, Wild Game

            float foodProduction = settlement.RaceType.FarmingRate * settlement.Citizens.Farmers;
            float excess = foodProduction - settlement.BaseFoodLevel;
            if (excess > 0.0f)
            {
                foodProduction = settlement.BaseFoodLevel + excess / 2;
            }

            return (int)foodProduction;
        }
    }
}