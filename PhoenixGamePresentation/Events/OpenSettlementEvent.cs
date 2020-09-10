using Microsoft.Xna.Framework;
using Hex;
using Input;
using MonoGameUtilities.ExtensionMethods;
using PhoenixGameLibrary;
using PhoenixGameLibrary.Commands;
using PhoenixGamePresentation.Views;

namespace PhoenixGamePresentation.Events
{
    internal static class OpenSettlementEvent
    {
        internal static void HandleEvent(object sender, MouseEventArgs e)
        {
            var worldView = (WorldView)e.WorldView;

            Settlement settlement = null;
            foreach (var item in worldView.Settlements)
            {
                if (e.Mouse != null && MousePointerIsOnHex(item.LocationHex, e.Mouse.Location, worldView.Camera))
                {
                    settlement = item;
                }
            }

            if (settlement is null) return;

            var settlements = worldView.Settlements;
            Command openSettlementCommand = new OpenSettlementCommand { Payload = (settlement, settlements) };
            openSettlementCommand.Execute();

            worldView.Camera.LookAtCell(settlement.Location);
            worldView.ChangeState(GameStatus.OverlandMap, GameStatus.CityView);
        }

        private static bool MousePointerIsOnHex(HexOffsetCoordinates settlementLocation, Point mouseLocation, Camera camera)
        {
            if (!camera.GetViewport.Contains(mouseLocation)) return false;

            return mouseLocation.IsWithinHex(settlementLocation, camera.Transform);
        }
    }
}