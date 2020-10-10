using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Hex;
using PhoenixGameLibrary;
using PhoenixGamePresentation.Views.StackView;
using Utilities;

namespace PhoenixGamePresentation.Handlers
{
    internal static class MovementHandler
    {
        private static readonly HexLibrary HexLibrary = new HexLibrary(HexType.PointyTopped, OffsetCoordinatesType.Odd);

        internal static (bool startMovement, PointI hexToMoveTo) CheckForUnitMovementFromMouseInitiation(Stack stack, object args)
        {
            var (cellGrid, mouseLocation, camera) = (ValueTuple<CellGrid, Point, Camera>)args;

            if (!camera.GetViewport.Contains(mouseLocation)) return (false, new PointI(0, 0));

            var hexToMoveTo = camera.ScreenPixelToWorldHex(mouseLocation).ToPointI();
            if (hexToMoveTo.Equals(stack.LocationHex)) return (false, new PointI(0, 0));
            var cellToMoveTo = cellGrid.GetCell(hexToMoveTo);
            if (cellToMoveTo.SeenState == SeenState.NeverSeen) return (false, new PointI(0, 0));

            var costToMoveIntoResult = stack.GetCostToMoveInto(cellToMoveTo);

            return costToMoveIntoResult.CanMoveInto ? (true, hexToMoveTo) : (false, new PointI(0, 0));
        }

        internal static (bool startMovement, PointI hexToMoveTo) CheckForUnitMovementFromKeyboardInitiation(Stack stack, object args)
        {
            var key = (Keys) args;

            var direction = key switch
            {
                Keys.NumPad1 => DirectionPointySideUp.SouthWest,
                Keys.NumPad3 => DirectionPointySideUp.SouthEast,
                Keys.NumPad4 => DirectionPointySideUp.West,
                Keys.NumPad6 => DirectionPointySideUp.East,
                Keys.NumPad7 => DirectionPointySideUp.NorthWest,
                Keys.NumPad9 => DirectionPointySideUp.NorthEast,
                _ => throw new ArgumentOutOfRangeException()
            };

            var neighbor = HexLibrary.GetNeighbor(new HexOffsetCoordinates(stack.LocationHex), (int)direction);
            var hexToMoveTo = neighbor.ToPointI();

            var costToMoveIntoResult = stack.GetCostToMoveInto(hexToMoveTo);

            return costToMoveIntoResult.CanMoveInto ? (true, hexToMoveTo) : (false, new PointI(0, 0));
        }

        internal static void CheckForUnitMovement(Func<Stack, object, (bool startMovement, PointI hexToMoveTo)> func, Stack stack, object args, StackView stackView)
        {
            if (!(stack.MovementPoints > 0.0f)) return;

            var mustStartMovement = func(stack, args);

            if (mustStartMovement.startMovement)
            {
                StartMovement(stackView, mustStartMovement.hexToMoveTo);
            }
        }

        private static void StartMovement(StackView stackView, PointI hexToMoveTo)
        {
            var path = MovementPathDeterminer.DetermineMovementPath(stackView.Stack, stackView.LocationHex, hexToMoveTo, stackView.CellGrid);
            stackView.MovementPath = path;
            stackView.Move();
        }
    }
}