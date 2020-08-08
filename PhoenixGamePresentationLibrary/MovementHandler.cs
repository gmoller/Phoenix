using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using HexLibrary;
using Input;
using PhoenixGameLibrary;
using PhoenixGameLibrary.Commands;
using PhoenixGameLibrary.GameData;
using Utilities;
using Point = Utilities.Point;

namespace PhoenixGamePresentationLibrary
{
    internal class MovementHandler
    {
        private const float MOVEMENT_TIME_BETWEEN_CELLS_IN_MILLISECONDS = 250.0f;

        internal void HandleMovement(InputHandler input, StackView stackView, float deltaTime)
        {
            var restartMovement = CheckForRestartOfMovement(stackView);
            if (restartMovement)
            {
                RestartUnitMovement(stackView);
            }

            var startUnitMovement = CheckForUnitMovementFromKeyboardInitiation(input, stackView);
            if (startUnitMovement.startMovement)
            {
                StartUnitMovement(stackView, startUnitMovement.hexToMoveTo);
            }
            else
            {
                startUnitMovement = CheckForUnitMovementFromMouseInitiation(input, stackView);
                if (startUnitMovement.startMovement)
                {
                    StartUnitMovement(stackView, startUnitMovement.hexToMoveTo);
                }
            }

            if (UnitIsMoving(stackView))
            {
                MoveUnit(stackView, deltaTime);
                var unitHasReachedNextCell = CheckIfUnitHasReachedNextCell(stackView);
                if (unitHasReachedNextCell) MoveUnitToCell(stackView);
            }
        }

        private bool CheckForRestartOfMovement(StackView stackView)
        {
            if (stackView.IsMovingState == false && stackView.MovementPath.Count > 0 && stackView.MovementPoints > 0.0f)
            {
                return true;
            }

            return false;
        }

        private void RestartUnitMovement(StackView stackView)
        {
            stackView.IsMovingState = true;
            stackView.MovementCountdownTime = MOVEMENT_TIME_BETWEEN_CELLS_IN_MILLISECONDS;
        }

        private (bool startMovement, Point hexToMoveTo) CheckForUnitMovementFromKeyboardInitiation(InputHandler input, StackView stackView)
        {
            if (!stackView.IsSelected || stackView.IsMovingState || stackView.MovementPoints.AboutEquals(0.0f) || !input.AreAnyNumPadKeysDown) return (false, new Point(0, 0));

            var direction = DetermineDirection(input);
            if (direction == Direction.None) return (false, new Point(0, 0));

            var neighbor = HexOffsetCoordinates.GetNeighbor(stackView.Location.X, stackView.Location.Y, direction);
            var hexToMoveTo = new Point(neighbor.Col, neighbor.Row);

            var costToMoveIntoResult = stackView.FirstUnit.CostToMoveInto(hexToMoveTo); // TODO: fix this, make work on stack

            return costToMoveIntoResult.CanMoveInto ? (true, hexToMoveTo) : (false, new Point(0, 0));
        }

        private (bool startMovement, Point hexToMoveTo) CheckForUnitMovementFromMouseInitiation(InputHandler input, StackView stackView)
        {
            if (!stackView.IsSelected || stackView.IsMovingState || stackView.MovementPoints.AboutEquals(0.0f) || !input.IsLeftMouseButtonReleased || input.Eaten) return (false, new Point(0, 0));

            // unit is selected, left mouse button released and unit is not already moving
            var hexToMoveTo = DeviceManager.Instance.WorldHexPointedAtByMouseCursor;
            if (hexToMoveTo == stackView.FirstUnit.Location) return (false, new Point(0, 0));
            var cellToMoveTo = Globals.Instance.World.OverlandMap.CellGrid.GetCell(hexToMoveTo.X, hexToMoveTo.Y);
            if (cellToMoveTo.SeenState == SeenState.Never) return (false, new Point(0, 0));

            var costToMoveIntoResult = stackView.FirstUnit.CostToMoveInto(cellToMoveTo);

            return costToMoveIntoResult.CanMoveInto ? (true, hexToMoveTo) : (false, new Point(0, 0));
        }

        private Direction DetermineDirection(InputHandler input)
        {
            if (input.IsKeyDown(Keys.NumPad4))
            {
                return Direction.West;
            }

            if (input.IsKeyDown(Keys.NumPad6))
            {
                return Direction.East;
            }

            if (input.IsKeyDown(Keys.NumPad7))
            {
                return Direction.NorthWest;
            }

            if (input.IsKeyDown(Keys.NumPad9))
            {
                return Direction.NorthEast;
            }

            if (input.IsKeyDown(Keys.NumPad1))
            {
                return Direction.SouthWest;
            }

            if (input.IsKeyDown(Keys.NumPad3))
            {
                return Direction.SouthEast;
            }

            return Direction.None;
        }

        private void StartUnitMovement(StackView stackView, Point hexToMoveTo)
        {
            stackView.SetMovementPath(MovementPathDeterminer.DetermineMovementPath(stackView.FirstUnit, stackView.Location, hexToMoveTo));

            stackView.IsMovingState = true;
            stackView.MovementCountdownTime = MOVEMENT_TIME_BETWEEN_CELLS_IN_MILLISECONDS;
        }

        private bool UnitIsMoving(StackView stackView)
        {
            return stackView.IsMovingState;
        }

        private void MoveUnit(StackView stackView, float deltaTime)
        {
            stackView.MovementCountdownTime -= deltaTime;

            // determine start cell screen position
            var startPosition = HexOffsetCoordinates.ToPixel(stackView.Location.X, stackView.Location.Y);
            // determine end cell screen position
            var hexToMoveTo = stackView.MovementPath[0];
            var endPosition = HexOffsetCoordinates.ToPixel(hexToMoveTo.X, hexToMoveTo.Y);
            // lerp between the two positions
            var newPosition = Vector2.Lerp(startPosition, endPosition, 1.0f - stackView.MovementCountdownTime / MOVEMENT_TIME_BETWEEN_CELLS_IN_MILLISECONDS);

            stackView.CurrentPositionOnScreen = newPosition;
        }

        private bool CheckIfUnitHasReachedNextCell(StackView stackView)
        {
            return stackView.MovementCountdownTime <= 0;
        }

        private void MoveUnitToCell(StackView stackView)
        {
            stackView.MovementCountdownTime = MOVEMENT_TIME_BETWEEN_CELLS_IN_MILLISECONDS;

            Command moveUnitCommand = new MoveUnitCommand
            {
                Payload = (stackView.FirstUnit, stackView.MovementPath[0])
            };
            moveUnitCommand.Execute();

            // if run out of movement points
            if (stackView.MovementPoints <= 0.0f)
            {
                stackView.IsMovingState = false;
                stackView.DeselectStack();
            }

            // if reached final destination
            stackView.RemoveFirstItemFromMovementPath();
            if (stackView.MovementPath.Count == 0)
            {
                stackView.IsMovingState = false;
            }
        }
    }
}