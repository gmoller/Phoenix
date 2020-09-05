using Microsoft.Xna.Framework;
using Input;
using MonoGameUtilities.ExtensionMethods;
using PhoenixGameLibrary;
using PhoenixGamePresentation.Views;
using Utilities;
using Utilities.ExtensionMethods;

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

        internal static bool CheckForStartOfMovement(InputHandler input, StackView stackView, WorldView worldView, Point mouseLocation)
        {
            if (worldView.GameStatus == GameStatus.InHudView) return false;

            var (startMovementMouse, _) = CheckForUnitMovementFromMouseInitiation(input, stackView, worldView.World.OverlandMap.CellGrid, mouseLocation, worldView.Camera.Transform);

            return startMovementMouse;
        }

        internal static bool MustContinueMovement(StackView stackView)
        {
            return UnitIsMoving(stackView);
        }

        internal static bool MustMoveUnitToNextCell(StackView stackView)
        {
            return UnitIsMoving(stackView) && CheckIfUnitHasReachedNextCell(stackView);
        }

        internal static PointI GetHexToMoveTo(InputHandler input, StackView stackView, CellGrid cellGrid, Point mouseLocation, Matrix transform)
        {
            var (startMovementMouse, hexToMoveToMouse) = CheckForUnitMovementFromMouseInitiation(input, stackView, cellGrid, mouseLocation, transform);

            return startMovementMouse ? hexToMoveToMouse : PointI.Zero;
        }

        private static (bool startMovement, PointI hexToMoveTo) CheckForUnitMovementFromMouseInitiation(InputHandler input, StackView stackView, CellGrid cellGrid, Point mouseLocation, Matrix transform)
        {
            if (stackView.IsMovingState || stackView.MovementPoints.AboutEquals(0.0f) || !input.IsLeftMouseButtonReleased) return (false, new PointI(0, 0));

            // unit is selected, left mouse button released and unit is not already moving
            var hexToMoveTo = mouseLocation.ToWorldHex(transform);
            if (hexToMoveTo == stackView.Location) return (false, new PointI(0, 0));
            var cellToMoveTo = cellGrid.GetCell(hexToMoveTo.X, hexToMoveTo.Y);
            if (cellToMoveTo.SeenState == SeenState.NeverSeen) return (false, new PointI(0, 0));

            var costToMoveIntoResult = stackView.GetCostToMoveInto(cellToMoveTo);

            return costToMoveIntoResult.CanMoveInto ? (true, hexToMoveTo) : (false, new PointI(0, 0));
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