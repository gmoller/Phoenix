using System;
using Utilities;

namespace Input
{
    public class MouseEventArgs : EventArgs
    {
        public MouseButtons Button { get; }
        public PointI Location { get; }
        public PointI MouseMovement { get; }
        public float DeltaTime { get; }

        public int X => Location.X;
        public int Y => Location.Y;

        public MouseEventArgs(PointI location, PointI mouseMovement, float deltaTime)
        {
            Location = location;
            MouseMovement = mouseMovement;
            DeltaTime = deltaTime;
        }
    }
}