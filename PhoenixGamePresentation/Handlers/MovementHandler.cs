using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PhoenixGameLibrary;
using PhoenixGamePresentation.Views.StackView;
using Zen.Hexagons;
using Zen.Utilities;

namespace PhoenixGamePresentation.Handlers
{
    internal static class MovementHandler
    {
        public static HexLibrary HexLibrary { get; set; }

        internal static (bool startMovement, PointI hexToMoveTo) CheckForUnitMovementFromMouseInitiation(Stack stack, object args)
        {
            var (cellGrid, mouseLocation, camera) = (ValueTuple<CellGrid, Point, Camera>)args;

            if (!camera.GetViewport.Contains(mouseLocation)) return (false, new PointI(0, 0));

            var screenPixelToWorldHex = camera.ScreenPixelToWorldHex(mouseLocation);
            var hexToMoveTo = new PointI(screenPixelToWorldHex.Col, screenPixelToWorldHex.Row);
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
                Keys.NumPad1 => Direction.SouthWest,
                Keys.NumPad2 => Direction.South,
                Keys.NumPad3 => Direction.SouthEast,
                Keys.NumPad4 => Direction.West,
                Keys.NumPad6 => Direction.East,
                Keys.NumPad7 => Direction.NorthWest,
                Keys.NumPad8 => Direction.North,
                Keys.NumPad9 => Direction.NorthEast,
                _ => throw new ArgumentOutOfRangeException()
            };

            var neighbor = HexLibrary.GetNeighbor(new HexOffsetCoordinates(stack.LocationHex.X, stack.LocationHex.Y), direction);
            var hexToMoveTo = new PointI(neighbor.Col, neighbor.Row);

            if (stack.LocationHex == hexToMoveTo)
            {
                return (false, new PointI(0, 0));
            }

            var costToMoveIntoResult = stack.GetCostToMoveInto(hexToMoveTo);

            return costToMoveIntoResult.CanMoveInto ? (true, hexToMoveTo) : (false, new PointI(0, 0));
        }

        internal static (bool startMovement, PointI hexToMoveTo) CheckForUnitMovement(Func<Stack, object, (bool startMovement, PointI hexToMoveTo)> func, Stack stack, object args)
        {
            if (!(stack.MovementPoints > 0.0f)) return (false, PointI.Empty);

            var mustStartMovement = func(stack, args);

            return mustStartMovement;
        }

        internal static void StartMovement(StackView stackView, PointI hexToMoveTo)
        {
            var path = MovementPathDeterminer.DetermineMovementPath(HexLibrary, stackView.Stack, stackView.LocationHex, hexToMoveTo, stackView.CellGrid);
            stackView.MovementPath = path;
            stackView.Move();
        }
    }
}