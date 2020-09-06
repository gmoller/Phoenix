using Input;
using PhoenixGamePresentation.Views;

namespace PhoenixGamePresentation.Events
{
    internal static class EndTurnEvent
    {
        internal static void HandleEvent(object sender, KeyboardEventArgs e)
        {
            var overlandMapView = (OverlandMapView)sender;

            // TODO: only allow if all units have been given orders
            overlandMapView.WorldView.EndTurn();
        }
    }
}