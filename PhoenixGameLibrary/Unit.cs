using Utilities;

namespace PhoenixGameLibrary
{
    /// <summary>
    /// 
    /// </summary>
    public class Unit
    {
        private const float BLINK_TIME_IN_MILLISECONDS = 500.0f;

        public Point Location { get; set; }
        public float MovementPoints { get; set; }
        public bool IsSelected { get; internal set; }
        public bool Blink { get; private set;  }

        private float _blinkCooldownInMilliseconds = BLINK_TIME_IN_MILLISECONDS;

        public Unit(Point location)
        {
            Location = location;
            MovementPoints = 2.0f;
        }

        public void Update(float deltaTime)
        {
            if (IsSelected)
            {
                _blinkCooldownInMilliseconds -= deltaTime;
                if (_blinkCooldownInMilliseconds <= 0)
                {
                    Blink = !Blink;
                    _blinkCooldownInMilliseconds = BLINK_TIME_IN_MILLISECONDS;
                }
            }
            else
            {
                Blink = false;
            }
        }

        public void EndTurn()
        {
            MovementPoints = 2.0f;
        }
    }
}