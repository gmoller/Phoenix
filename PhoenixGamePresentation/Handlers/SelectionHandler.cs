using Input;
using PhoenixGameLibrary;
using PhoenixGamePresentation.Views;
using Utilities;

namespace PhoenixGamePresentation.Handlers
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
            var context = CallContext<GlobalContext>.GetData("AmbientGlobalContext");
            var hexPoint = context.WorldHexPointedAtByMouseCursor;

            return stackView.Location == hexPoint;
        }

        internal static void SelectStack(StackView stackView)
        {
            stackView.SetAsCurrent();
        }
    }
}