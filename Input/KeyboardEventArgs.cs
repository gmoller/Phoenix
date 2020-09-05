using System;
using Microsoft.Xna.Framework.Input;

namespace Input
{
    public class KeyboardEventArgs : EventArgs
    {
        public KeyboardHandler Keyboard { get; }
        public Keys Key { get; }
        public float DeltaTime { get; }

        public KeyboardEventArgs(KeyboardHandler keyboard, Keys key, float deltaTime)
        {
            Keyboard = keyboard;
            Key = key;
            DeltaTime = deltaTime;
        }
    }
}