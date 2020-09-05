﻿using System;

namespace Input
{
    public readonly struct MouseInputAction
    {
        public MouseInputActionType InputActionType { get; }
        private Action<object, MouseEventArgs> Handler { get; }

        public string DictionaryKey => $"Mouse.{InputActionType}";

        public MouseInputAction(MouseInputActionType inputActionType, Action<object, MouseEventArgs> handler)
        {
            InputActionType = inputActionType;
            Handler = handler;
        }

        public void Invoke(MouseHandler mouse, float deltaTime)
        {
            Handler.Invoke(this, new MouseEventArgs(mouse, deltaTime));
        }
    }
}