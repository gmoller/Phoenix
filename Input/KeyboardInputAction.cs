using System;
using Microsoft.Xna.Framework.Input;

namespace Input
{
    public readonly struct KeyboardInputAction
    {
        public Keys Key { get; }
        public KeyboardInputActionType InputActionType { get; }
        private Action<object, EventArgs> Handler { get; }

        public string DictionaryKey => $"Keyboard.{Key}.{InputActionType}";

        public KeyboardInputAction(Keys key, KeyboardInputActionType inputActionType, Action<object, EventArgs> handler)
        {
            Key = key;
            InputActionType = inputActionType;
            Handler = handler;
        }

        public void Invoke()
        {
            Handler.Invoke(this, new KeyboardEventArgs(Key));
        }
    }
}