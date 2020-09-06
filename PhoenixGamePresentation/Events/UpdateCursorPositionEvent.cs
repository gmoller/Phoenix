using Input;
using MonoGameUtilities.ExtensionMethods;
using PhoenixGamePresentation.Views;

namespace PhoenixGamePresentation.Events
{
    internal static class UpdateCursorPositionEvent
    {
        internal static void HandleEvent(object sender, MouseEventArgs e)
        {
            var cursorView = (CursorView)sender;

            cursorView.SetPosition(e.Mouse.Location.ToPointI());
        }
    }
}