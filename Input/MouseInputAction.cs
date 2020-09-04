using System;
using Utilities;

namespace Input
{
    public readonly struct MouseInputAction
    {
        public MouseButtons MouseWidget { get; }
        public MouseInputActionType InputActionType { get; }
        private Action<object, EventArgs> Handler { get; }

        public string DictionaryKey => $"Mouse.{MouseWidget}.{InputActionType}";

        public MouseInputAction(MouseButtons mouseWidget, MouseInputActionType inputActionType, Action<object, EventArgs> handler)
        {
            MouseWidget = mouseWidget;
            InputActionType = inputActionType;
            Handler = handler;
        }

        public void Invoke(InputHandler input, float deltaTime)
        {
            Handler.Invoke(this, new MouseEventArgs(new PointI(input.MousePosition.X, input.MousePosition.Y), new PointI(input.MouseMovement.X, input.MouseMovement.Y), deltaTime));
        }
    }
}