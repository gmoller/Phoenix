using System.Runtime.Remoting.Messaging;
using Input;
using PhoenixGameLibrary;
using PhoenixGamePresentationLibrary.Views;

namespace PhoenixGamePresentationLibrary.Handlers
{
    internal static class SelectionHandler
    {
        internal static bool CheckForUnitSelection(InputHandler input, StackView stackView)
        {
            if (stackView.IsSelected) return false;

            var selectUnit = input.IsRightMouseButtonReleased && CursorIsOnThisStack(stackView);

            return selectUnit;
        }

        private static bool CursorIsOnThisStack(StackView stackView)
        {
            var context = (GlobalContext)CallContext.LogicalGetData("AmbientGlobalContext");
            var hexPoint = context.WorldHexPointedAtByMouseCursor;

            return stackView.Location == hexPoint;
        }

        internal static void SelectStack(StackView stackView)
        {
            stackView.SetAsCurrent();
        }
    }
}