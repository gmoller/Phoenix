using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Hex;
using MonoGameUtilities.ExtensionMethods;
using PhoenixGameLibrary.Commands;

namespace PhoenixGamePresentation.Views.StackView
{
    internal class StackViewMovingState : StackViewState
    {
        private const float MOVEMENT_TIME_BETWEEN_CELLS_IN_MILLISECONDS = 250.0f;

        #region State
        private Vector2 CurrentPositionOnScreen { get; set; }
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
            worldView.Camera.LookAtPixel(CurrentPositionOnScreen);

            if (canMove) return;

            if (StackView.StackHasMovementPoints)
            {
                StackView.Select();
            }
            else
            {
                StackView.Unselect();
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
            var startPosition = HexOffsetCoordinates.ToPixel(StackView.LocationHex).ToVector2();
            // determine end cell screen position
            var hexToMoveTo = StackView.MovementPath[0];
            var endPosition = HexOffsetCoordinates.ToPixel(hexToMoveTo).ToVector2();
            // lerp between the two positions
            var newPosition = Vector2.Lerp(startPosition, endPosition, 1.0f - MovementCountdownTime / MOVEMENT_TIME_BETWEEN_CELLS_IN_MILLISECONDS);

            CurrentPositionOnScreen = newPosition;

            return false;
        }

        internal override void DrawUnit(SpriteBatch spriteBatch, Camera camera)
        {
            DrawUnit(spriteBatch, CurrentPositionOnScreen);

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
    }
}