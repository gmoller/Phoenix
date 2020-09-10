using Input;
using PhoenixGamePresentation.Handlers;
using PhoenixGamePresentation.Views;
using Utilities.ExtensionMethods;

namespace PhoenixGamePresentation.Events
{
    internal static class CheckForUnitMovementFromMouseInitiationEvent
    {
        internal static void HandleEvent(object sender, MouseEventArgs e)
        {
            var stackView = (StackView) sender;

            if (!stackView.IsSelected) return;
            if (stackView.IsMovingState) return;
            if (stackView.MovementPoints.AboutEquals(0.0f)) return;

            var mustStartMovement = MovementHandler.CheckForUnitMovementFromMouseInitiation(stackView, stackView.WorldView.CellGrid, e.Mouse.Location, stackView.WorldView.Camera);

            if (mustStartMovement.startMovement)
            {
                stackView.StartUnitMovement(mustStartMovement.hexToMoveTo);
            }
        }
    }
}