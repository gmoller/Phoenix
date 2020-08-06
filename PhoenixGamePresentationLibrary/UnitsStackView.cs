using System;
using System.Collections.Generic;
using System.Diagnostics;
using GuiControls;
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
        private readonly UnitsStackViews _unitsStackViews;
        private readonly UnitsStack _unitsStack;

        private List<Point> _movementPath;
        private List<Point> _potentialMovementPath;
        private List<IControl> _actionButtons;

        private float _blinkCooldownInMilliseconds;
        private bool _blink;

        public EnumerableList<Point> MovementPath => new EnumerableList<Point>(_movementPath);
        public EnumerableList<Point> PotentialMovementPath => new EnumerableList<Point>(_potentialMovementPath);
        public bool IsSelected => _unitsStackViews.Selected == this;
        public bool IsMovingState { get; set; }
        public float MovementCountdownTime { get; set; }
        public Vector2 CurrentPositionOnScreen { get; set; }
        public EnumerableList<IControl> ActionButtons => new EnumerableList<IControl>(_actionButtons);

        public Point Location => _unitsStack.Location;
        public float MovementPoints => _unitsStack.MovementPoints;
        public EnumerableList<string> Actions => _unitsStack.Actions;
        public Unit FirstUnit => _unitsStack[0];

        public int Count => _unitsStack.Count;

        public UnitsStackView(WorldView worldView, UnitsStackViews unitsStacksView, UnitsStack unitsStack)
        {
            _worldView = worldView;
            _unitsStackViews = unitsStacksView;
            _unitsStack = unitsStack;
            _movementPath = new List<Point>();
            _potentialMovementPath = new List<Point>();
            _blinkCooldownInMilliseconds = BLINK_TIME_IN_MILLISECONDS;
            CurrentPositionOnScreen = HexOffsetCoordinates.ToPixel(unitsStack.Location.X, unitsStack.Location.Y);
            _actionButtons = new List<IControl>();
        }

        internal List<IControl> GetMovementTypeImages()
        {
            // TODO: update will not be called on these
            var imgMovementTypes = new List<IControl>();
            foreach (var movementType in _unitsStack.MovementTypes)
            {
                var img = _worldView.MovementTypeImages[movementType];
                imgMovementTypes.Add(img);
            }

            return imgMovementTypes;
        }

        internal void SetButtons()
        {
            var actionButtons = new List<IControl>();
            foreach (var action in Actions)
            {
                var btn = _worldView.ActionButtons[action];
                btn.Click += (o, args) => BtnClick(o, new ButtonClickEventArgs(action));
                actionButtons.Add(btn);
            }

            _actionButtons = actionButtons;
        }

        private void BtnClick(object sender, ButtonClickEventArgs e)
        {
            switch (e.Action)
            {
                case "Done":
                    break;
                case "Patrol":
                    break;
                case "Wait":
                    _unitsStackViews.SelectNext();
                    break;
                case "BuildOutpost":
                    break;
                default:
                    throw new Exception($"Action [{e.Action}] is not implemented.");
            }
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

            foreach (var button in ActionButtons)
            {
                button.Update(input, deltaTime);
            }
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
            _unitsStackViews.SelectNext();
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

            DrawMovementPath(spriteBatch, _movementPath, Color.Black, 5.0f, 5.0f);
            DrawMovementPath(spriteBatch, _potentialMovementPath, Color.White, 3.0f, 1.0f);
        }

        private void DrawUnit(SpriteBatch spriteBatch)
        {
            // draw background
            var position = CurrentPositionOnScreen;
            var destinationRectangle = new Rectangle((int)position.X, (int)position.Y, 60, 60); ;
            var sourceRectangle = _unitsStackViews.SquareGreenFrame.ToRectangle();
            spriteBatch.Draw(_unitsStackViews.GuiTextures, destinationRectangle, sourceRectangle, Color.White, 0.0f, new Vector2(sourceRectangle.Width * 0.5f, sourceRectangle.Height * 0.5f), SpriteEffects.None, 0.0f);

            // draw unit icon
            destinationRectangle = new Rectangle((int)position.X, (int)position.Y, 36, 32);
            var frame = _unitsStackViews.UnitAtlas.Frames[FirstUnit.UnitTypeTextureName];
            sourceRectangle = frame.ToRectangle();
            spriteBatch.Draw(_unitsStackViews.UnitTextures, destinationRectangle, sourceRectangle, Color.White, 0.0f, new Vector2(sourceRectangle.Width * 0.5f, sourceRectangle.Height * 0.5f), SpriteEffects.None, 0.0f);
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
            foreach (var unitsStackView in _unitsStackViews)
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
            var x = topLeftPosition.X + 60.0f * 0.5f;
            var y = topLeftPosition.Y + 60.0f * 0.5f;
            foreach (var unit in _unitsStack)
            {
                var xOffset = 75.0f * (index % 3);
                var yOffset = 75.0f * (index / 3); // math.Floor
                DrawBadge(spriteBatch, new Vector2(x + xOffset, y + yOffset), unit, isSelected);
                index++;
            }
        }

        private void DrawBadge(SpriteBatch spriteBatch, Vector2 centerPosition, Unit unit, bool isSelected)
        {
            // draw background
            var destinationRectangle = new Rectangle((int)centerPosition.X, (int)centerPosition.Y, 60, 60);
            var sourceRectangle = isSelected ? _unitsStackViews.SquareGreenFrame.ToRectangle() : _unitsStackViews.SquareGrayFrame.ToRectangle();
            spriteBatch.Draw(_unitsStackViews.GuiTextures, destinationRectangle, sourceRectangle, Color.White, 0.0f, new Vector2(sourceRectangle.Width * 0.5f, sourceRectangle.Height * 0.5f), SpriteEffects.FlipVertically, 0.0f);

            // draw unit icon
            destinationRectangle = new Rectangle((int)centerPosition.X, (int)centerPosition.Y, 36, 32);
            var frame = _unitsStackViews.UnitAtlas.Frames[unit.UnitTypeTextureName];
            sourceRectangle = frame.ToRectangle();
            spriteBatch.Draw(_unitsStackViews.UnitTextures, destinationRectangle, sourceRectangle, Color.White, 0.0f, new Vector2(sourceRectangle.Width * 0.5f, sourceRectangle.Height * 0.5f), SpriteEffects.None, 0.0f);
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Count={_unitsStack.Count}}}";

        public void ResetPotentialMovementPath()
        {
            _potentialMovementPath = new List<Point>();
        }

        internal void SetPotentialMovementPath(List<Point> path)
        {
            _potentialMovementPath = path;
        }

        public void ResetMovementPath()
        {
            _movementPath = new List<Point>();
        }

        internal void SetMovementPath(List<Point> path)
        {
            _movementPath = path;
        }

        internal void RemoveFirstItemFromMovementPath()
        {
            _movementPath.RemoveAt(0);
        }
    }
}