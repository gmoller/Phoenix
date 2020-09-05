using System;

namespace Input
{
    public class MouseEventArgs : EventArgs
    {
        public MouseHandler Mouse { get; }
        public float DeltaTime { get; }

        public MouseEventArgs(MouseHandler mouse, float deltaTime)
        {
            Mouse = mouse;
            DeltaTime = deltaTime;
        }
    }
}