using Microsoft.Xna.Framework;
using PhoenixGameLibrary;
using PhoenixGamePresentation.Views;
using Utilities;

namespace PhoenixGamePresentation.Handlers
{
    internal static class MovementHandler
    {
        internal static bool CheckForRestartOfMovement(StackView stackView)
        {
            if (stackView.IsMovingState == false && stackView.MovementPath.Count > 0 && stackView.MovementPoints > 0.0f)
            {
                return true;
            }

            return false;
        }
        
        internal static bool MustContinueMovement(StackView stackView)
        {
            return UnitIsMoving(stackView);
        }

        internal static bool MustMoveUnitToNextCell(StackView stackView)
        {
            return UnitIsMoving(stackView) && CheckIfUnitHasReachedNextCell(stackView);
        }

        internal static (bool startMovement, PointI hexToMoveTo) CheckForUnitMovementFromMouseInitiation(StackView stackView, CellGrid cellGrid, Point mouseLocation, Camera camera)
        {
            // unit is selected, left mouse button released and unit is not already moving
            var hexToMoveTo = camera.ScreenPixelToWorldHex(mouseLocation);
            if (hexToMoveTo.Equals(stackView.LocationHex)) return (false, new PointI(0, 0));
            var cellToMoveTo = cellGrid.GetCell(hexToMoveTo.Col, hexToMoveTo.Row);
            if (cellToMoveTo.SeenState == SeenState.NeverSeen) return (false, new PointI(0, 0));

            var costToMoveIntoResult = stackView.GetCostToMoveInto(cellToMoveTo);

            return costToMoveIntoResult.CanMoveInto ? (true, hexToMoveTo.ToPointI()) : (false, new PointI(0, 0));
        }

        private static bool UnitIsMoving(StackView stackView)
        {
            return stackView.IsMovingState;
        }

        private static bool CheckIfUnitHasReachedNextCell(StackView stackView)
        {
            return stackView.MovementCountdownTime <= 0;
        }
    }
}