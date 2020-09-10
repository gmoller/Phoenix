using Input;
using PhoenixGameLibrary;
using PhoenixGamePresentation.Handlers;
using PhoenixGamePresentation.Views;

namespace PhoenixGamePresentation.Events
{
    internal static class DrawPotentialMovementPathEvent
    {
        internal static void HandleEvent(object sender, MouseEventArgs e)
        {
            var stackView = (StackView)sender;

            if (!stackView.IsSelected) return;
            if (stackView.Status == UnitStatus.Explore) return;
            if (stackView.IsMovingState) return;

            var path = PotentialMovementHandler.GetPotentialMovementPath(stackView, stackView.WorldView.CellGrid, e.Mouse.Location, stackView.WorldView.Camera);
            stackView.PotentialMovementPath = path;
        }
    }
}