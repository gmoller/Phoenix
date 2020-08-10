using System;
using Input;
using Utilities;

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
            var hexPoint = DeviceManager.Instance.WorldHexPointedAtByMouseCursor;

            return stackView.Location == hexPoint;
        }
    }
}