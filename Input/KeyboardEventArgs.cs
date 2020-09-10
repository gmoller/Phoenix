using System;
using Microsoft.Xna.Framework.Input;

namespace Input
{
    public class KeyboardEventArgs : EventArgs
    {
        public KeyboardHandler Keyboard { get; }
        public object WorldView { get; }
        public Keys Key { get; }
        public float DeltaTime { get; }

        public KeyboardEventArgs(KeyboardHandler keyboard, Keys key, object worldView, float deltaTime)
        {
            Keyboard = keyboard;
            Key = key;
            WorldView = worldView;
            DeltaTime = deltaTime;
        }
    }
}