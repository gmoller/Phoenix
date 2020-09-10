using System;
using Hex;
using Input;
using Microsoft.Xna.Framework.Input;
using PhoenixGamePresentation.Views;
using Utilities;
using Utilities.ExtensionMethods;

namespace PhoenixGamePresentation.Events
{
    internal static class CheckForUnitMovementFromKeyboardInitiationEvent
    {
        internal static void HandleEvent(object sender, KeyboardEventArgs e)
        {
            var stackView = (StackView)sender;

            if (!stackView.IsSelected) return;
            if (stackView.IsMovingState) return;
            if (stackView.MovementPoints.AboutEquals(0.0f)) return;

            var direction = e.Key switch
            {
                Keys.NumPad1 => Direction.SouthWest,
                Keys.NumPad3 => Direction.SouthEast,
                Keys.NumPad4 => Direction.West,
                Keys.NumPad6 => Direction.East,
                Keys.NumPad7 => Direction.NorthWest,
                Keys.NumPad9 => Direction.NorthEast,
                _ => throw new ArgumentOutOfRangeException()
            };

            var neighbor = HexOffsetCoordinates.GetNeighbor(stackView.LocationHex, direction);
            var hexToMoveTo = new PointI(neighbor.Col, neighbor.Row);

            var costToMoveIntoResult = stackView.GetCostToMoveInto(hexToMoveTo);

            if (costToMoveIntoResult.CanMoveInto)
            {
                stackView.StartUnitMovement(hexToMoveTo);
            }
        }
    }
}