using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhoenixGameLibrary.Commands;
using Zen.Hexagons;

namespace PhoenixGamePresentation.Views.StackView
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    internal class StackViewMovingState : StackViewState
    {
        private const float MOVEMENT_TIME_BETWEEN_CELLS_IN_MILLISECONDS = 250.0f;

        #region State
        private Vector2 LocationInWorld { get; set; }
        private float MovementCountdownTime { get; set; }
        #endregion End State

        internal StackViewMovingState(StackView stackView)
        {
            StackView = stackView;
            MovementCountdownTime = MOVEMENT_TIME_BETWEEN_CELLS_IN_MILLISECONDS;
        }

        internal override void Update(WorldView worldView, float deltaTime)
        {
            var cantMove = MovementCountdownTimeHasExpired() ? MoveStackToNextCell() : MoveStackBetweenCells(deltaTime);
            var canMove = !cantMove;
            worldView.Camera.LookAtPixel(LocationInWorld);

            if (canMove) return;

            if (StackView.StackHasMovementPoints)
            {
                StackView.Select();
            }
            else
            {
                StackView.Unselect();
                StackView.SelectNext();
            }
        }

        private bool MoveStackToNextCell()
        {
            MovementCountdownTime = MOVEMENT_TIME_BETWEEN_CELLS_IN_MILLISECONDS;

            //StackView.Stack.MoveTo(StackView.MovementPath[0]);
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

        private string DebuggerDisplay => "Moving";
    }
}