using Input;
using PhoenixGamePresentation.Views;

namespace PhoenixGamePresentation.Events
{
    internal static class FocusCameraOnLocationEvent
    {
        internal static void HandleEvent(object sender, KeyboardEventArgs e)
        {
            var stackView = (StackView)sender;

            if (!stackView.IsSelected) return;

            stackView.WorldView.Camera.LookAtCell(stackView.Location);
        }
    }
}