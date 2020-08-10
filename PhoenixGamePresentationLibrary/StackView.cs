using System;
using System.Collections.Generic;
using System.Diagnostics;
using GuiControls;
using Microsoft.Xna.Framework.Graphics;
using HexLibrary;
using Input;
using Microsoft.Xna.Framework;
using PhoenixGameLibrary;
using PhoenixGameLibrary.Commands;
using Utilities;
using Point = Utilities.Point;

namespace PhoenixGamePresentationLibrary
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    internal class StackView
    {
        private const float BLINK_TIME_IN_MILLISECONDS = 250.0f;
        private const float MOVEMENT_TIME_BETWEEN_CELLS_IN_MILLISECONDS = 250.0f;

        private readonly WorldView _worldView;
        private readonly StackViews _stackViews;
        private readonly Stack _stack;

        private Vector2 _currentPositionOnScreen;
        private Point _destination;
        private List<Point> _movementPath;
        private List<Point> _potentialMovementPath;
        private List<IControl> _actionButtons;

        private float _blinkCooldownInMilliseconds;
        private bool _blink;

        public Guid Id { get; }
        public bool IsMovingState { get; private set; }
        public float MovementCountdownTime { get; private set; }

        public bool IsBusy => _stack.IsBusy;
        public UnitStatus Status => _stack.Status;

        public EnumerableList<Point> MovementPath => new EnumerableList<Point>(_movementPath);
        public bool IsSelected => _stackViews.Current == this;
        public EnumerableList<IControl> ActionButtons => new EnumerableList<IControl>(_actionButtons);

        public Point Location => _stack.Location;
        public float MovementPoints => _stack.MovementPoints;
        public Unit FirstUnit => _stack[0];

        public int Count => _stack.Count;

        public StackView(WorldView worldView, StackViews stackViews, Stack stack)
        {
            Id = Guid.NewGuid();
            _worldView = worldView;
            _stackViews = stackViews;
            _stack = stack;
            _movementPath = new List<Point>();
            _potentialMovementPath = new List<Point>();
            _blinkCooldownInMilliseconds = BLINK_TIME_IN_MILLISECONDS;
            _currentPositionOnScreen = HexOffsetCoordinates.ToPixel(stack.Location.X, stack.Location.Y);
            _actionButtons = new List<IControl>();
        }

        internal List<IControl> GetMovementTypeImages()
        {
            // TODO: update will not be called on these
            var imgMovementTypes = new List<IControl>();
            foreach (var movementType in _stack.MovementTypes)
            {
                var img = _worldView.MovementTypeImages[movementType];
                imgMovementTypes.Add(img);
            }

            return imgMovementTypes;
        }

        internal void SetButtons()
        {
            var actionButtons = new List<IControl>();
            foreach (var action in _stack.Actions)
            {
                var btn = _worldView.ActionButtons[action];
                actionButtons.Add(btn);
            }

            _actionButtons = actionButtons;
        }

        internal void Update(InputHandler input, float deltaTime)
        {
            SelectionHandler.HandleSelection(input, this, SelectStack);

            if (!IsSelected) return;

            _blink = DetermineBlinkState(_blink, deltaTime);
             
            ExploreHandler.HandleExplore(this, SetMovementPath);
            PotentialMovementHandler.HandlePotentialMovement(input, this, _worldView.World, SetPotentialMovementPath);
            MovementHandler.HandleMovement(input, this, deltaTime, RestartUnitMovement, StartUnitMovement, MoveUnit, MoveUnitToCell);

            ActionButtons.Update(input, deltaTime);
        }

        private void SelectStack()
        {
            _blink = true;
            _stackViews.SetCurrent(this);
        }

        private bool DetermineBlinkState(bool blink, float deltaTime)
        {
            if (!IsMovingState)
            {
                _blinkCooldownInMilliseconds -= deltaTime;
                if (_blinkCooldownInMilliseconds > 0.0f) return blink;

                // flip blink state
                blink = !blink;
                _blinkCooldownInMilliseconds = BLINK_TIME_IN_MILLISECONDS;
            }
            else
            {
                return false;
            }

            return blink;
        }

        private void SetMovementPath(List<Point> path)
        {
            _movementPath = path;
        }

        private void SetPotentialMovementPath(List<Point> path)
        {
            _potentialMovementPath = path;
        }

        private void RestartUnitMovement()
        {
            IsMovingState = true;
            MovementCountdownTime = MOVEMENT_TIME_BETWEEN_CELLS_IN_MILLISECONDS;
        }

        private void StartUnitMovement(Point hexToMoveTo)
        {
            _destination = hexToMoveTo;

            var path = MovementPathDeterminer.DetermineMovementPath(FirstUnit, Location, hexToMoveTo);
            SetMovementPath(path);

            IsMovingState = true;
            MovementCountdownTime = MOVEMENT_TIME_BETWEEN_CELLS_IN_MILLISECONDS;
        }

        private void MoveUnit(float deltaTime)
        {
            MovementCountdownTime -= deltaTime;

            // determine start cell screen position
            var startPosition = HexOffsetCoordinates.ToPixel(Location.X, Location.Y);
            // determine end cell screen position
            var hexToMoveTo = MovementPath[0];
            var endPosition = HexOffsetCoordinates.ToPixel(hexToMoveTo.X, hexToMoveTo.Y);
            // lerp between the two positions
            var newPosition = Vector2.Lerp(startPosition, endPosition, 1.0f - MovementCountdownTime / MOVEMENT_TIME_BETWEEN_CELLS_IN_MILLISECONDS);

            _currentPositionOnScreen = newPosition;
            _worldView.Camera.LookAtPixel(newPosition);
        }

        private void MoveUnitToCell()
        {
            MovementCountdownTime = MOVEMENT_TIME_BETWEEN_CELLS_IN_MILLISECONDS;

            Command moveUnitCommand = new MoveUnitCommand { Payload = (FirstUnit, MovementPath[0]) };
            moveUnitCommand.Execute();

            //if (Location == _destination)
            //{
            //    var path = MovementPathDeterminer.DetermineMovementPath(FirstUnit, Location, Destination);
            //    SetMovementPath(path);
            //}

            // if run out of movement points
            if (MovementPoints <= 0.0f)
            {
                IsMovingState = false;
                DeselectStack();
            }

            // if reached final destination
            RemoveFirstItemFromMovementPath();
            if (MovementPath.Count == 0)
            {
                IsMovingState = false;
            }
        }

        internal void DoPatrolAction()
        {
            _stack.DoPatrolAction();
        }

        internal void DoFortifyAction()
        {
            _stack.DoFortifyAction();
        }

        internal void DoExploreAction()
        {
            _stack.DoExploreAction();
        }

        internal void SetStatusToNone()
        {
            _stack.SetStatusToNone();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsSelected)
            {
                if (!_blink)
                {
                    DrawUnit(spriteBatch);
                }
            }
            else
            {
                if (_stackViews.Current != null) // if another stack is selected
                {
                    var selectedStacksPosition = _stackViews.Current.Location;
                    var thisStacksPosition = Location;
                    if (selectedStacksPosition == thisStacksPosition) // and it's in the same hex as this one
                    {
                        if (_stackViews.Current.IsMovingState) // and selected stack is moving
                        {
                            DrawUnit(spriteBatch);
                        }
                        else
                        {
                            // don't draw if there's a selected stack on same location and it's not moving
                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        DrawUnit(spriteBatch);
                    }
                }
                else
                {
                    DrawUnit(spriteBatch);
                }
            }

            DrawMovementPath(spriteBatch, _movementPath, Color.Black, 5.0f, 5.0f);
            DrawMovementPath(spriteBatch, _potentialMovementPath, Color.White, 3.0f, 1.0f);
        }

        private void DrawUnit(SpriteBatch spriteBatch)
        {
            // draw background
            var destinationRectangle = new Rectangle((int)_currentPositionOnScreen.X, (int)_currentPositionOnScreen.Y, 60, 60);
            var sourceRectangle = _stackViews.SquareGreenFrame.ToRectangle();
            spriteBatch.Draw(_stackViews.GuiTextures, destinationRectangle, sourceRectangle, Color.White, 0.0f, new Vector2(sourceRectangle.Width * 0.5f, sourceRectangle.Height * 0.5f), SpriteEffects.None, 0.0f);

            // draw unit icon
            destinationRectangle = new Rectangle((int)_currentPositionOnScreen.X, (int)_currentPositionOnScreen.Y, 36, 32);
            var frame = _stackViews.UnitAtlas.Frames[FirstUnit.UnitTypeTextureName];
            sourceRectangle = frame.ToRectangle();
            spriteBatch.Draw(_stackViews.UnitTextures, destinationRectangle, sourceRectangle, Color.White, 0.0f, new Vector2(sourceRectangle.Width * 0.5f, sourceRectangle.Height * 0.5f), SpriteEffects.None, 0.0f);
        }

        private void DrawMovementPath(SpriteBatch spriteBatch, List<Point> movementPath, Color color, float radius, float thickness)
        {
            foreach (var item in movementPath)
            {
                var centerPosition = HexOffsetCoordinates.ToPixel(item.X, item.Y);
                spriteBatch.DrawCircle(centerPosition, radius, 10, color, thickness);
            }
        }

        internal List<StackView> GetStackViewsSharingSameLocation()
        {
            var stackViews = new List<StackView>();
            foreach (var stackView in _stackViews)
            {
                if (stackView.Location == Location) // same location
                {
                    stackViews.Add(stackView);
                }
            }

            return stackViews;
        }

        internal void DrawBadges(SpriteBatch spriteBatch, Vector2 topLeftPosition, int index = 0, bool isSelected = true)
        {
            var x = topLeftPosition.X + 60.0f * 0.5f;
            var y = topLeftPosition.Y + 60.0f * 0.5f;
            foreach (var unit in _stack)
            {
                var indexMod3 = index % 3;
                var indexDividedBy3 = index / 3; // Floor
                var xOffset = 75.0f * indexMod3;
                var yOffset = 75.0f * indexDividedBy3;
                DrawBadge(spriteBatch, new Vector2(x + xOffset, y + yOffset), unit, isSelected);
                index++;
            }
        }

        private void DrawBadge(SpriteBatch spriteBatch, Vector2 centerPosition, Unit unit, bool isSelected)
        {
            // draw background
            var destinationRectangle = new Rectangle((int)centerPosition.X, (int)centerPosition.Y, 60, 60);
            var sourceRectangle = isSelected ? _stackViews.SquareGreenFrame.ToRectangle() : _stackViews.SquareGrayFrame.ToRectangle();
            spriteBatch.Draw(_stackViews.GuiTextures, destinationRectangle, sourceRectangle, Color.White, 0.0f, new Vector2(sourceRectangle.Width * 0.5f, sourceRectangle.Height * 0.5f), SpriteEffects.FlipVertically, 0.0f);

            // draw unit icon
            destinationRectangle = new Rectangle((int)centerPosition.X, (int)centerPosition.Y, 36, 32);
            var frame = _stackViews.UnitAtlas.Frames[unit.UnitTypeTextureName];
            sourceRectangle = frame.ToRectangle();
            spriteBatch.Draw(_stackViews.UnitTextures, destinationRectangle, sourceRectangle, Color.White, 0.0f, new Vector2(sourceRectangle.Width * 0.5f, sourceRectangle.Height * 0.5f), SpriteEffects.None, 0.0f);
        }

        private void DeselectStack()
        {
            if (_stackViews.Current == null || Id == _stackViews.Current.Id)
            {
                _stackViews.SelectNext();
            }
        }

        private void RemoveFirstItemFromMovementPath()
        {
            _movementPath.RemoveAt(0);
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Id={Id},UnitsInStack={_stack.Count}}}";
    }

    public static class EnumerableListExtensions
    {
        public static void Update(this EnumerableList<IControl> list, InputHandler input, float deltaTime)
        {
            foreach (var item in list)
            {
                item.Update(input, deltaTime);
            }
        }
    }
}