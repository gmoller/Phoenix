using Microsoft.Xna.Framework.Graphics;
using PhoenixGameLibrary;
using PhoenixGamePresentation.Handlers;
using Utilities.ExtensionMethods;

namespace PhoenixGamePresentation.Views.StackView
{
    internal class StackViewExploringState : StackViewState
    {
        internal StackViewExploringState(StackView stackView)
        {
            StackView = stackView;
        }

        internal override (bool changeState, StackViewState stateToChangeTo) Update(StackViewUpdateActions updateActions, WorldView worldView, float deltaTime)
        {
            var changeState = SetMovementPathToNewExploreLocation(worldView);

            return changeState;
        }

        private (bool changeState, StackViewState stateToChangeTo) SetMovementPathToNewExploreLocation(WorldView worldView)
        {
            // find closest unexplored cell
            var cellGrid = worldView.CellGrid;
            var cell = cellGrid.GetClosestUnexploredCell(StackView.LocationHex);

            if (cell == Cell.Empty)
            {
                return (true, new StackViewNormalState(StackView)); // all locations explored
            }

            // find best path to unexplored cell
            var path = MovementPathDeterminer.DetermineMovementPath(StackView.Stack, StackView.LocationHex, cell.ToPoint, cellGrid);

            if (path.Count == 0)
            {
                return (true, new StackViewNormalState(StackView)); // could not find a path to location
            }
            
            path = path.RemoveLast(StackView.SightRange);

            return (true, new StackViewMovingState(path, StackView));
        }

        internal override void DrawUnit(SpriteBatch spriteBatch, Camera camera)
        {
            var locationInWorld = camera.WorldHexToWorldPixel(StackView.Stack.LocationHex);
            DrawUnit(spriteBatch, locationInWorld);
        }
    }
}