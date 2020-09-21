using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using PhoenixGameLibrary;
using PhoenixGamePresentation.Handlers;
using Utilities.ExtensionMethods;

namespace PhoenixGamePresentation.Views.StackView
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    internal class StackViewExploringState : StackViewState
    {
        internal StackViewExploringState(StackView stackView)
        {
            StackView = stackView;
        }

        internal override void Update(WorldView worldView, float deltaTime)
        {
            SetMovementPathToNewExploreLocation(worldView);
        }

        private void SetMovementPathToNewExploreLocation(WorldView worldView)
        {
            // find closest unexplored cell
            var cellGrid = worldView.CellGrid;
            var cell = cellGrid.GetClosestUnexploredCell(StackView.LocationHex);

            if (cell == Cell.Empty)
            {
                // all locations explored
                StackView.Unselect();
            }

            // find best path to unexplored cell
            var path = MovementPathDeterminer.DetermineMovementPath(StackView.Stack, StackView.LocationHex, cell.ToPoint, cellGrid);

            if (path.Count == 0)
            {
                // could not find a path to location
                StackView.Unselect();
            }

            path = path.RemoveLast(StackView.SightRange);
            StackView.MovementPath = path;
            StackView.Move();
        }

        internal override void DrawUnit(SpriteBatch spriteBatch, Camera camera)
        {
            var locationInWorld = camera.WorldHexToWorldPixel(StackView.Stack.LocationHex);
            DrawUnit(spriteBatch, locationInWorld);
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => "Exploring";
    }
}