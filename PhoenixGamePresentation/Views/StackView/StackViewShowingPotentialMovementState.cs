using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhoenixGameLibrary;
using PhoenixGamePresentation.Handlers;
using Zen.Utilities;

namespace PhoenixGamePresentation.Views.StackView
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    internal class StackViewShowingPotentialMovementState : StackViewState
    {
        internal StackViewShowingPotentialMovementState(StackView stackView)
        {
            StackView = stackView;
        }

        internal override void Update(WorldView worldView, float deltaTime)
        {
            var path = GetPotentialMovementPath(StackView.Stack, StackView.CellGrid, StackView.MousePosition, StackView.Camera);
            StackView.PotentialMovementPath = path;
        }

        internal override void DrawUnit(SpriteBatch spriteBatch, Camera camera)
        {
            var locationInWorld = camera.WorldHexToWorldPixel(StackView.Stack.LocationHex);
            DrawUnitBackground(spriteBatch, locationInWorld);
            DrawUnitIcon(spriteBatch, locationInWorld);
            DrawMovementPath(spriteBatch, StackView.PotentialMovementPath, Color.White, 3.0f, 1.0f);
        }

        private List<PointI> GetPotentialMovementPath(Stack stack, CellGrid cellGrid, Point mouseLocation, Camera camera)
        {
            var (potentialMovement, hexToMoveTo) = CheckForPotentialUnitMovement(stack, cellGrid, mouseLocation, camera);
            if (!potentialMovement) return new List<PointI>();

            var path = MovementPathDeterminer.DetermineMovementPath(StackView.HexLibrary, stack, stack.LocationHex, hexToMoveTo, cellGrid);

            return path;

        }

        private (bool potentialMovement, PointI hexToMoveTo) CheckForPotentialUnitMovement(Stack stack, CellGrid cellGrid, Point mouseLocation, Camera camera)
        {
            var hexToMoveTo = camera.ScreenPixelToWorldHex(mouseLocation);
            var cellToMoveTo = cellGrid.GetCell(hexToMoveTo.Col, hexToMoveTo.Row);
            if (cellToMoveTo.SeenState == SeenState.NeverSeen) return (false, new PointI(0, 0));
            var costToMoveIntoResult = stack.GetCostToMoveInto(cellToMoveTo);

            return (costToMoveIntoResult.CanMoveInto, new PointI(hexToMoveTo.Col, hexToMoveTo.Row));
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => "ShowingPotentialMovementState";
    }
}