using System;

namespace PhoenixGamePresentationLibrary
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