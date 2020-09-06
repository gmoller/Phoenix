using Microsoft.Xna.Framework;
using Hex;
using Input;
using MonoGameUtilities.ExtensionMethods;
using PhoenixGameLibrary.Commands;
using PhoenixGamePresentation.Views;

namespace PhoenixGamePresentation.Events
{
    internal static class OpenSettlementEvent
    {
        internal static void HandleEvent(object sender, MouseEventArgs e)
        {
            var overlandSettlementView = (OverlandSettlementView)sender;

            if (overlandSettlementView.WorldView.GameStatus != GameStatus.OverlandMap) return;
            if (!MousePointerIsOnHex(overlandSettlementView.Settlement.LocationHex, e.Mouse.Location, overlandSettlementView.WorldView)) return;

            Command openSettlementCommand = new OpenSettlementCommand { Payload = (overlandSettlementView.Settlement, overlandSettlementView.WorldView.World.Settlements) };
            openSettlementCommand.Execute();

            overlandSettlementView.WorldView.Camera.LookAtCell(overlandSettlementView.Settlement.Location);

            overlandSettlementView.WorldView.GameStatus = GameStatus.CityView;
        }

        private static bool MousePointerIsOnHex(HexOffsetCoordinates settlementLocation, Point mouseLocation, WorldView worldView)
        {
            return mouseLocation.IsWithinHex(settlementLocation, worldView.Camera.Transform);
        }
    }
}