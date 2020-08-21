using System;
using System.Runtime.Remoting.Messaging;
using Input;
using PhoenixGameLibrary;

namespace PhoenixGamePresentationLibrary
{
    internal static class SelectionHandler
    {
        internal static void HandleSelection(InputHandler input, StackView stackView, Action action)
        {
            if (stackView.IsSelected) return;

            var selectUnit = CheckForUnitSelection(input, stackView);
            if (selectUnit)
            {
                action();
            }
        }

        private static bool CheckForUnitSelection(InputHandler input, StackView stackView)
        {
            return input.IsRightMouseButtonReleased && CursorIsOnThisStack(stackView);
        }

        private static bool CursorIsOnThisStack(StackView stackView)
        {
            var context = (GlobalContext)CallContext.LogicalGetData("AmbientGlobalContext");
            var hexPoint = context.WorldHexPointedAtByMouseCursor;

            return stackView.Location == hexPoint;
        }
    }
}