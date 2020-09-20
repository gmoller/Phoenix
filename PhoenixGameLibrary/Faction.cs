namespace PhoenixGameLibrary
{
    public class Faction
    {
        private World World { get; }

        public int GoldInTreasury { get; set; }
        public int GoldPerTurn { get; set; }
        public int ManaInTreasury { get; set; }
        public int ManaPerTurn { get; set; }

        public Faction(World world)
        {
            World = world;
        }

        public int FoodPerTurn => World.Settlements.FoodProducedThisTurn;
    }
}