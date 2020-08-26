using System;

namespace PhoenixGamePresentation
{
    public class ButtonClickEventArgs : EventArgs
    {
        public string Action { get; }

        public ButtonClickEventArgs(string action)
        {
            Action = action;
        }
    }
}