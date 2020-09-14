using Input;
using PhoenixGamePresentation.Views;

namespace PhoenixGamePresentation.Events
{
    internal static class EndTurnEvent
    {
        internal static void HandleEvent(object sender, KeyboardEventArgs e)
        {
            var worldView = (WorldView)e.WorldView;

            worldView.EndTurn();
        }
    }
}