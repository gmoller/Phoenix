using Input;
using Utilities;

namespace PhoenixGamePresentationLibrary
{
    internal class SelectionHandler
    {
        internal bool HandleSelection(InputHandler input, StackView stackView)
        {
            if (stackView.IsSelected) return false;

            var selectUnit = CheckForUnitSelection(input, stackView);

            return selectUnit;
        }

        private bool CheckForUnitSelection(InputHandler input, StackView stackView)
        {
            return input.IsRightMouseButtonReleased && CursorIsOnThisStack(stackView);
        }

        private bool CursorIsOnThisStack(StackView stackView)
        {
            var hexPoint = DeviceManager.Instance.WorldHexPointedAtByMouseCursor;

            return stackView.Location == hexPoint;
        }
    }
}