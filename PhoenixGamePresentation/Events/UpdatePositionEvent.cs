using Input;
using MonoGameUtilities.ExtensionMethods;
using PhoenixGamePresentation.Views;

namespace PhoenixGamePresentation.Events
{
    internal static class UpdatePositionEvent
    {
        internal static void HandleEvent(object sender, MouseEventArgs e)
        {
            var cursorView = (CursorView)sender;

            cursorView.Position = e.Mouse.Location.ToPointI();
            cursorView.CursorImage.SetTopLeftPosition(cursorView.Position);
        }
    }
}