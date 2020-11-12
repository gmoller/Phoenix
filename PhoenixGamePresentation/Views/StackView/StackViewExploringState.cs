using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhoenixGameLibrary;
using PhoenixGameLibrary.Commands;
using PhoenixGamePresentation.Handlers;
using Zen.Hexagons;
using Zen.Utilities.ExtensionMethods;

namespace PhoenixGamePresentation.Views.StackView
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    internal class StackViewExploringState : StackViewState
    {
        private const float MOVEMENT_TIME_BETWEEN_CELLS_IN_MILLISECONDS = 250.0f;

        #region State
        private Vector2 LocationInWorld { get; set; }
        private float MovementCountdownTime { get; set; }
        #endregion End State

        internal StackViewExploringState(StackView stackView, WorldView worldView)
        {
            StackView = stackView;
            MovementCountdownTime = MOVEMENT_TIME_BETWEEN_CELLS_IN_MILLISECONDS;
            SetMovementPathToNewExploreLocation(worldView);
        }

        internal override void Update(WorldView worldView, float deltaTime)
        {
            var cantMove = MovementCountdownTimeHasExpired() ? MoveStackToNextCell() : MoveStackBetweenCells(deltaTime);
            var canMove = !cantMove;
            worldView.Camera.LookAtPixel(LocationInWorld);

            if (canMove) return;

            if (StackView.StackHasMovementPoints) // either reached final destination or may not move into cell
            {
                SetMovementPathToNewExploreLocation(worldView);
            }
            else // run out of movement points
            {
                StackView.Unselect();
                StackView.SelectNext();
            }
        }

        private void SetMovementPathToNewExploreLocation(WorldView worldView)
        {
            // find closest unexplored cell
            var cellGrid = worldView.CellGrid;
            var cell = cellGrid.GetClosestUnexploredCell(StackView.LocationHex);

            if (cell == Cell.Empty)
            {
                // all locations explored
                StackView.SetStatusToNone();
                StackView.Unselect();
            }

            // find best path to unexplored cell
            var path = MovementPathDeterminer.DetermineMovementPath(StackView.HexLibrary, StackView.Stack, StackView.LocationHex, cell.ToPoint, cellGrid);

            if (path.Count == 0)
            {
                // could not find a path to location
                StackView.SetStatusToNone();
                StackView.Unselect();
            }

            path = path.RemoveLast(StackView.SightRange);
            StackView.MovementPath = path;
        }

        private bool MoveStackToNextCell()
        {
            MovementCountdownTime = MOVEMENT_TIME_BETWEEN_CELLS_IN_MILLISECONDS;

            Command moveUnitCommand = new MoveUnitCommand { Payload = (StackView.Stack, StackView.MovementPath[0]) };
            moveUnitCommand.Execute();
            RemoveFirstItemFromMovementPath();

            // if run out of movement points
            if (StackView.StackHasNoMovementPoints)
            {
                return true;
            }

            // if reached final destination
            if (StackView.HasNoMovementPath)
            {
                return true;
            }

            return false;
        }

        private bool MoveStackBetweenCells(float deltaTime)
        {
            // if stack cannot move into next hex in path
            var cost = StackView.Stack.GetCostToMoveInto(StackView.MovementPath[0]);
            if (!cost.CanMoveInto)
            {
                return true;
            }

            MovementCountdownTime -= deltaTime;

            // determine start cell screen position
            var pixel = StackView.HexLibrary.FromOffsetCoordinatesToPixel(new HexOffsetCoordinates(StackView.LocationHex.X, StackView.LocationHex.Y));
            var startPosition = new Vector2(pixel.X, pixel.Y);
            // determine end cell screen position
            var hexToMoveTo = StackView.MovementPath[0];
            pixel = StackView.HexLibrary.FromOffsetCoordinatesToPixel(new HexOffsetCoordinates(hexToMoveTo.X, hexToMoveTo.Y));
            var endPosition = new Vector2(pixel.X, pixel.Y);
            // lerp between the two positions
            var newPosition = Vector2.Lerp(startPosition, endPosition, 1.0f - MovementCountdownTime / MOVEMENT_TIME_BETWEEN_CELLS_IN_MILLISECONDS);

            LocationInWorld = newPosition;

            return false;
        }

        internal override void DrawUnit(SpriteBatch spriteBatch, Camera camera)
        {
            DrawUnitIcon(spriteBatch, LocationInWorld);
            DrawMovementPath(spriteBatch, StackView.MovementPath, Color.Black, 5.0f, 5.0f);
        }

        private void RemoveFirstItemFromMovementPath()
        {
            StackView.MovementPath.RemoveAt(0);
        }

        private bool MovementCountdownTimeHasExpired()
        {
            return MovementCountdownTime <= 0.0f;
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => "Exploring";
    }
}