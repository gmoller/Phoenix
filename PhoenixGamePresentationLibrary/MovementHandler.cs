using System.Collections.Generic;
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
        private const float MOVEMENT_TIME_BETWEEN_CELLS_IN_MILLISECONDS = 500.0f;

        internal void HandleMovement(InputHandler input, UnitsStackView unitsStackView, float deltaTime)
        {
            var restartMovement = CheckForRestartOfMovement(unitsStackView);
            if (restartMovement)
            {
                RestartUnitMovement(unitsStackView);
            }

            var startUnitMovement = CheckForUnitMovementFromKeyboardInitiation(input, unitsStackView);
            if (startUnitMovement.startMovement)
            {
                StartUnitMovement(unitsStackView, startUnitMovement.hexToMoveTo);
            }
            else
            {
                startUnitMovement = CheckForUnitMovementFromMouseInitiation(input, unitsStackView);
                if (startUnitMovement.startMovement)
                {
                    StartUnitMovement(unitsStackView, startUnitMovement.hexToMoveTo);
                }
            }

            if (UnitIsMoving(unitsStackView))
            {
                MoveUnit(unitsStackView, deltaTime);
                var unitHasReachedNextCell = CheckIfUnitHasReachedNextCell(unitsStackView);
                if (unitHasReachedNextCell) MoveUnitToCell(unitsStackView);
            }
        }

        private bool CheckForRestartOfMovement(UnitsStackView unitsStackView)
        {
            if (unitsStackView.IsMovingState == false && unitsStackView.MovementPath.Count > 0 && unitsStackView.MovementPoints > 0.0f)
            {
                return true;
            }

            return false;
        }

        private void RestartUnitMovement(UnitsStackView unitsStackView)
        {
            unitsStackView.IsMovingState = true;
            unitsStackView.MovementCountdownTime = MOVEMENT_TIME_BETWEEN_CELLS_IN_MILLISECONDS;
        }

        private (bool startMovement, Point hexToMoveTo) CheckForUnitMovementFromKeyboardInitiation(InputHandler input, UnitsStackView unitsStackView)
        {
            if (!unitsStackView.IsSelected || unitsStackView.IsMovingState || !input.AreAnyNumPadKeysDown) return (false, new Point(0, 0));

            var direction = DetermineDirection(input);
            if (direction == Direction.None) return (false, new Point(0, 0));

            var neighbor = HexOffsetCoordinates.GetNeighbor(unitsStackView.Location.X, unitsStackView.Location.Y, direction);
            var hexToMoveTo = new Point(neighbor.Col, neighbor.Row);

            var canMoveInto = unitsStackView.FirstUnit.CanMoveInto(hexToMoveTo); // TODO: fix this, make work on stack

            return canMoveInto ? (true, hexToMoveTo) : (false, new Point(0, 0));
        }

        private (bool startMovement, Point hexToMoveTo) CheckForUnitMovementFromMouseInitiation(InputHandler input, UnitsStackView unitsStackView)
        {
            if (!unitsStackView.IsSelected || unitsStackView.IsMovingState || !input.IsLeftMouseButtonReleased || input.Eaten) return (false, new Point(0, 0));

            // unit is selected, left mouse button released and unit is not already moving
            var hexToMoveTo = DeviceManager.Instance.WorldHexPointedAtByMouseCursor;
            var cellToMoveTo = Globals.Instance.World.OverlandMap.CellGrid.GetCell(hexToMoveTo.X, hexToMoveTo.Y);
            if (cellToMoveTo.SeenState == SeenState.Never) return (false, new Point(0, 0));

            var canMoveInto = unitsStackView.FirstUnit.CanMoveInto(cellToMoveTo);

            return canMoveInto ? (true, hexToMoveTo) : (false, new Point(0, 0));
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

        private void StartUnitMovement(UnitsStackView unitsStackView, Point hexToMoveTo)
        {
            unitsStackView.MovementPath = MovementPathDeterminer.DetermineMovementPath(unitsStackView.FirstUnit, unitsStackView.Location, hexToMoveTo);

            unitsStackView.IsMovingState = true;
            unitsStackView.MovementCountdownTime = MOVEMENT_TIME_BETWEEN_CELLS_IN_MILLISECONDS;
        }

        private bool UnitIsMoving(UnitsStackView unitsStackView)
        {
            return unitsStackView.IsMovingState;
        }

        private void MoveUnit(UnitsStackView unitsStackView, float deltaTime)
        {
            unitsStackView.MovementCountdownTime -= deltaTime;

            // determine start cell screen position
            var startPosition = HexOffsetCoordinates.ToPixel(unitsStackView.Location.X, unitsStackView.Location.Y);
            // determine end cell screen position
            var hexToMoveTo = unitsStackView.MovementPath[0];
            var endPosition = HexOffsetCoordinates.ToPixel(hexToMoveTo.X, hexToMoveTo.Y);
            // lerp between the two positions
            var newPosition = Vector2.Lerp(startPosition, endPosition, 1.0f - unitsStackView.MovementCountdownTime / MOVEMENT_TIME_BETWEEN_CELLS_IN_MILLISECONDS);

            unitsStackView.CurrentPositionOnScreen = newPosition;
        }

        private bool CheckIfUnitHasReachedNextCell(UnitsStackView unitsStackView)
        {
            return unitsStackView.MovementCountdownTime <= 0;
        }

        private void MoveUnitToCell(UnitsStackView unitsStackView)
        {
            unitsStackView.MovementCountdownTime = MOVEMENT_TIME_BETWEEN_CELLS_IN_MILLISECONDS;

            Command moveUnitCommand = new MoveUnitCommand
            {
                Payload = (unitsStackView.FirstUnit, unitsStackView.MovementPath[0])
            };
            moveUnitCommand.Execute();

            // if run out of movement points
            if (unitsStackView.MovementPoints <= 0.0f)
            {
                unitsStackView.IsMovingState = false;
            }

            // if reached final destination
            if (unitsStackView.MovementPath.Count == 0)
            {
                unitsStackView.IsMovingState = false;
                unitsStackView.MovementPath = new List<Point>();
            }
            else
            {
                unitsStackView.MovementPath.RemoveAt(0);
                if (unitsStackView.MovementPath.Count == 0)
                {
                    unitsStackView.IsMovingState = false;
                    unitsStackView.MovementPath = new List<Point>();
                }
            }
        }
    }
}