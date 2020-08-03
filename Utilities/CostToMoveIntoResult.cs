namespace Utilities
{
    public struct CostToMoveIntoResult
    {
        public bool CanMoveInto { get; set; }
        public float CostToMoveInto { get; set; }

        public CostToMoveIntoResult(bool canMoveInto)
        {
            CanMoveInto = canMoveInto;
            CostToMoveInto = 0.0f;
        }

        public CostToMoveIntoResult(bool canMoveInto, float costToMoveInto)
        {
            CanMoveInto = canMoveInto;
            CostToMoveInto = costToMoveInto;
        }
    }
}