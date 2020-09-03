using System;
using Utilities;

namespace Input
{
    public class MouseEventArgs : EventArgs
    {
        public PointI Location { get; }

        public MouseEventArgs(PointI location)
        {
            Location = location;
        }
    }
}