using Hex;
using Input;
using Microsoft.Xna.Framework;
using MonoGameUtilities.ExtensionMethods;
using PhoenixGamePresentation.Views;

namespace PhoenixGamePresentation.Events
{
    internal static class SelectStackEvent
    {
        internal static void HandleEvent(object sender, MouseEventArgs e)
        {
            var stackView = (StackView)sender;

            if (stackView.IsSelected) return;
            if (!MousePointerIsOnHex(stackView.LocationHex, e.Mouse.Location, stackView.WorldView.Camera.Transform)) return;

            stackView.SetAsCurrent();
        }

        private static bool MousePointerIsOnHex(HexOffsetCoordinates settlementLocation, Point mouseLocation, Matrix transform)
        {
            return mouseLocation.IsWithinHex(settlementLocation, transform);
        }
    }
}