using System;
using Microsoft.Xna.Framework.Input;

namespace Input
{
    public readonly struct KeyboardInputAction
    {
        private object Sender { get; }
        public Keys Key { get; }
        public KeyboardInputActionType InputActionType { get; }
        private Action<object, KeyboardEventArgs> Handler { get; }

        public KeyboardInputAction(object sender, Keys key, KeyboardInputActionType inputActionType, Action<object, KeyboardEventArgs> handler)
        {
            Sender = sender;
            Key = key;
            InputActionType = inputActionType;
            Handler = handler;
        }

        public void Invoke(KeyboardHandler keyboard, float deltaTime)
        {
            Handler.Invoke(Sender, new KeyboardEventArgs(keyboard, Key, deltaTime));
        }
    }
}