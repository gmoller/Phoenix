using System;
using Microsoft.Xna.Framework.Input;

namespace Input
{
    public class KeyboardEventArgs : EventArgs
    {
        public Keys Key { get; }
        public float DeltaTime { get; }

        public KeyboardEventArgs(Keys key, float deltaTime)
        {
            Key = key;
            DeltaTime = deltaTime;
        }
    }
}