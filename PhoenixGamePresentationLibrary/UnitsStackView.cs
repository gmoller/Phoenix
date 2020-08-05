using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using HexLibrary;
using Input;
using Microsoft.Xna.Framework;
using PhoenixGameLibrary;
using Utilities;
using Point = Utilities.Point;

namespace PhoenixGamePresentationLibrary
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    internal class UnitsStackView
    {
        private const float BLINK_TIME_IN_MILLISECONDS = 500.0f;

        private readonly WorldView _worldView;
        private readonly UnitsStacksView _unitsStacksView;
        private readonly UnitsStack _unitsStack;

        private float _blinkCooldownInMilliseconds;
        private bool _blink;

        public List<Point> MovementPath { get; set; }
        public List<Point> PotentialMovementPath { get; set; }
        public bool IsSelected => _unitsStacksView.Selected == this;
        public bool IsMovingState { get; set; }
        public float MovementCountdownTime { get; set; }
        public Vector2 CurrentPositionOnScreen { get; set; }

        public Point Location => _unitsStack.Location;
        public float MovementPoints => _unitsStack.MovementPoints;
        public List<string> MovementTypes => _unitsStack.MovementTypes;
        public Unit FirstUnit => _unitsStack[0];

        public int Count => _unitsStack.Count;

        public UnitsStackView(WorldView worldView, UnitsStacksView unitsStacksView, UnitsStack unitsStack)
        {
            _worldView = worldView;
            _unitsStacksView = unitsStacksView;
            _unitsStack = unitsStack;
            MovementPath = new List<Point>();
            _blinkCooldownInMilliseconds = BLINK_TIME_IN_MILLISECONDS;
            CurrentPositionOnScreen = HexOffsetCoordinates.ToPixel(unitsStack.Location.X, unitsStack.Location.Y);
        }

        internal void Update(InputHandler input, float deltaTime)
        {
            // check for blink
            _blink = DetermineBlinkState(_blink, deltaTime);

            // handle potential movement
            var potentialMovementHandler = new PotentialMovementHandler();
            potentialMovementHandler.HandleMovement(input, this, _worldView.World);

            // handle movement
            var movementHandler = new MovementHandler();
            movementHandler.HandleMovement(input, this, deltaTime);

            // handle selection/deselection
            var selectUnit = CheckForUnitSelection(input);
            if (selectUnit) SelectStack();
            var deselectUnit = CheckForStackDeselection(_unitsStack);
            if (deselectUnit) DeselectStack();
        }

        private bool DetermineBlinkState(bool blink, float deltaTime)
        {
            if (IsSelected && !IsMovingState)
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

        private bool CheckForUnitSelection(InputHandler input)
        {
            if (IsSelected) return false;
            if (input.IsRightMouseButtonReleased) return CursorIsOnThisStack();

            return false;
        }

        private bool CursorIsOnThisStack()
        {
            var hexPoint = DeviceManager.Instance.WorldHexPointedAtByMouseCursor;

            return _unitsStack.Location == hexPoint;
        }

        internal void SelectStack()
        {
            _blink = true;
        }

        private bool CheckForStackDeselection(UnitsStack unitsStack)
        {
            if (IsSelected) return unitsStack.MovementPoints <= 0.0f;

            return false;
        }

        private void DeselectStack()
        {
            _unitsStacksView.SelectNext();
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
                DrawUnit(spriteBatch);
            }

            DrawMovementPath(spriteBatch, MovementPath, Color.Black, 5.0f, 5.0f);
            DrawMovementPath(spriteBatch, PotentialMovementPath, Color.White, 3.0f, 1.0f);
        }

        private void DrawUnit(SpriteBatch spriteBatch)
        {
            // draw background
            var position = CurrentPositionOnScreen;
            var destinationRectangle = new Rectangle((int)position.X, (int)position.Y, 60, 60); ;
            var sourceRectangle = _unitsStacksView.SquareGreenFrame.ToRectangle();
            spriteBatch.Draw(_unitsStacksView.GuiTextures, destinationRectangle, sourceRectangle, Color.White, 0.0f, new Vector2(sourceRectangle.Width * 0.5f, sourceRectangle.Height * 0.5f), SpriteEffects.None, 0.0f);

            // draw unit icon
            destinationRectangle = new Rectangle((int)position.X, (int)position.Y, 36, 32);
            var frame = _unitsStacksView.UnitAtlas.Frames[FirstUnit.UnitTypeTextureName];
            sourceRectangle = frame.ToRectangle();
            spriteBatch.Draw(_unitsStacksView.UnitTextures, destinationRectangle, sourceRectangle, Color.White, 0.0f, new Vector2(sourceRectangle.Width * 0.5f, sourceRectangle.Height * 0.5f), SpriteEffects.None, 0.0f);
        }

        private void DrawMovementPath(SpriteBatch spriteBatch, List<Point> movementPath, Color color, float radius, float thickness)
        {
            foreach (var item in movementPath)
            {
                var centerPosition = HexOffsetCoordinates.ToPixel(item.X, item.Y);
                spriteBatch.DrawCircle(centerPosition, radius, 10, color, thickness);
            }
        }

        internal List<UnitsStackView> GetUnitStacksSharingSameLocation()
        {
            var unitsStacksView = new List<UnitsStackView>();
            foreach (var unitsStackView in _unitsStacksView)
            {
                if (unitsStackView.Location == Location && unitsStackView != this) // same location and not itself
                {
                    unitsStacksView.Add(unitsStackView);
                }
            }

            return unitsStacksView;
        }

        internal void DrawBadges(SpriteBatch spriteBatch, Vector2 topLeftPosition, int index = 0, bool isSelected = true)
        {
            foreach (var unit in _unitsStack)
            {
                var xOffset = 75.0f * (index % 3);
                var yOffset = 75.0f * (index / 3); // math.Floor
                DrawBadge(spriteBatch, new Vector2(topLeftPosition.X + 60.0f * 0.5f + xOffset, topLeftPosition.Y + 60.0f * 0.5f + yOffset), unit, isSelected);
                index++;
            }
        }

        private void DrawBadge(SpriteBatch spriteBatch, Vector2 centerPosition, Unit unit, bool isSelected)
        {
            // draw background
            var destinationRectangle = new Rectangle((int)centerPosition.X, (int)centerPosition.Y, 60, 60);
            var sourceRectangle = isSelected ? _unitsStacksView.SquareGreenFrame.ToRectangle() : _unitsStacksView.SquareGrayFrame.ToRectangle();
            spriteBatch.Draw(_unitsStacksView.GuiTextures, destinationRectangle, sourceRectangle, Color.White, 0.0f, new Vector2(sourceRectangle.Width * 0.5f, sourceRectangle.Height * 0.5f), SpriteEffects.None, 0.0f);

            // draw unit icon
            destinationRectangle = new Rectangle((int)centerPosition.X, (int)centerPosition.Y, 36, 32);
            var frame = _unitsStacksView.UnitAtlas.Frames[unit.UnitTypeTextureName];
            sourceRectangle = frame.ToRectangle();
            spriteBatch.Draw(_unitsStacksView.UnitTextures, destinationRectangle, sourceRectangle, Color.White, 0.0f, new Vector2(sourceRectangle.Width * 0.5f, sourceRectangle.Height * 0.5f), SpriteEffects.None, 0.0f);
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Count={_unitsStack.Count}}}";
    }
}