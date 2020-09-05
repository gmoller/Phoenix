using System;
using Microsoft.Xna.Framework.Input;

namespace Input
{
    public readonly struct KeyboardInputAction
    {
        public Keys Key { get; }
        public KeyboardInputActionType InputActionType { get; }
        private Action<object, KeyboardEventArgs> Handler { get; }

        public string DictionaryKey => $"Keyboard.{Key}.{InputActionType}";

        public KeyboardInputAction(Keys key, KeyboardInputActionType inputActionType, Action<object, KeyboardEventArgs> handler)
        {
            Key = key;
            InputActionType = inputActionType;
            Handler = handler;
        }

        public void Invoke(KeyboardHandler keyboard, float deltaTime)
        {
            Handler.Invoke(this, new KeyboardEventArgs(keyboard, Key, deltaTime));
        }
    }
}