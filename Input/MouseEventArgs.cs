using System;

namespace Input
{
    public class MouseEventArgs : EventArgs
    {
        public MouseHandler Mouse { get; }
        public object WorldView { get; }
        public float DeltaTime { get; }

        public MouseEventArgs(MouseHandler mouse, object worldView, float deltaTime)
        {
            Mouse = mouse;
            WorldView = worldView;
            DeltaTime = deltaTime;
        }
    }
}