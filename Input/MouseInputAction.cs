using System;
using Microsoft.Xna.Framework;
using Utilities;

namespace Input
{
    public readonly struct MouseInputAction
    {
        public MouseInputActionType InputActionType { get; }
        private Action<object, EventArgs> Handler { get; }

        public string DictionaryKey => $"Mouse.{InputActionType}";

        public MouseInputAction(MouseInputActionType inputActionType, Action<object, EventArgs> handler)
        {
            InputActionType = inputActionType;
            Handler = handler;
        }

        public void Invoke(Point mousePosition, Point mouseMovement, float deltaTime)
        {
            Handler.Invoke(this, new MouseEventArgs(new PointI(mousePosition.X, mousePosition.Y), new PointI(mouseMovement.X, mouseMovement.Y), deltaTime));
        }
    }
}