using System;
using Utilities;

namespace GuiControls
{
    public class MouseEventArgs : EventArgs
    {
        public Point Location { get; }

        public MouseEventArgs(Point location)
        {
            Location = location;
        }
    }
}