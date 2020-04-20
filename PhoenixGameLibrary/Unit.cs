namespace PhoenixGameLibrary
{
    /// <summary>
    /// 
    /// </summary>
    public class Unit
    {
        public Point Location { get; }

        public Unit(Point location)
        {
            Location = location;
        }

        public void Update(float deltaTime)
        {
        }

        public void EndTurn()
        {
        }
    }
}