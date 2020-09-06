using System;

namespace Input
{
    public readonly struct MouseInputAction
    {
        private object Sender { get; }
        public MouseInputActionType InputActionType { get; }
        private Action<object, MouseEventArgs> Handler { get; }

        public MouseInputAction(object sender, MouseInputActionType inputActionType, Action<object, MouseEventArgs> handler)
        {
            Sender = sender;
            InputActionType = inputActionType;
            Handler = handler;
        }

        public void Invoke(MouseHandler mouse, float deltaTime)
        {
            Handler.Invoke(Sender, new MouseEventArgs(mouse, deltaTime));
        }
    }
}