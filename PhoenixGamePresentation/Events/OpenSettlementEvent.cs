using Input;
using MonoGameUtilities.ExtensionMethods;
using PhoenixGameLibrary;
using PhoenixGamePresentation.Views;

namespace PhoenixGamePresentation.Events
{
    internal static class OpenSettlementEvent
    {
        internal static void HandleEvent(object sender, MouseEventArgs e)
        {
            var worldView = (WorldView)e.WorldView;
            if (!worldView.Camera.GetViewport.Contains(e.Mouse.Location)) return;

            Settlement settlement = null;
            foreach (var item in worldView.Settlements)
            {
                if (e.Mouse.Location.IsWithinHex(item.LocationHex, worldView.Camera.Transform))
                {
                    settlement = item;
                }
            }

            if (settlement is null) return;

            var settlements = worldView.Settlements;
            settlements.Selected = settlement;

            //settlement.FocusCameraOn();
            worldView.Camera.LookAtCell(settlement.Location);
            worldView.ChangeState(GameStatus.OverlandMap, GameStatus.CityView);
        }
    }
}