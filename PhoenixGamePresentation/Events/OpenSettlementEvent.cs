using Input;
using Microsoft.Xna.Framework;
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
            
            var settlement = IsMouseOnSettlement(worldView, e.Mouse.Location);
            if (settlement == null) return;

            OpenSettlement(settlement, worldView);
        }

        internal static void HandleEvent2(object sender, MouseEventArgs e)
        {
            var worldView = (WorldView)e.WorldView;
            var settlement = (Settlement)sender;

            OpenSettlement(settlement, worldView);
        }

        private static Settlement IsMouseOnSettlement(WorldView worldView, Point mouseLocation)
        {
            if (!worldView.Camera.GetViewport.Contains(mouseLocation)) return null;

            Settlement settlement = null;
            foreach (var item in worldView.Settlements)
            {
                if (mouseLocation.IsWithinHex(item.LocationHex, worldView.Camera.Transform, worldView.HexLibrary))
                {
                    settlement = item;
                }
            }

            return settlement;
        }

        private static void OpenSettlement(Settlement settlement, WorldView worldView)
        {
            var settlements = worldView.Settlements;
            settlements.Selected = settlement;

            worldView.Camera.LookAtCell(settlement.Location);
            worldView.ChangeState(GameStatus.OverlandMap, GameStatus.CityView);
        }
    }
}