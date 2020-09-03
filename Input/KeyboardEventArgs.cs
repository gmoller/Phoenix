using System;
using Microsoft.Xna.Framework.Input;

namespace Input
{
    public class KeyboardEventArgs : EventArgs
    {
        public Keys Key { get; }

        public KeyboardEventArgs(Keys key)
        {
            Key = key;
        }
    }
}