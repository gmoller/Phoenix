using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using HexLibrary;
using Input;
using Microsoft.Xna.Framework;
using PhoenixGameLibrary;
using Utilities;
using Point = Utilities.Point;

namespace PhoenixGamePresentationLibrary
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class UnitsStackView
    {
        private const float BLINK_TIME_IN_MILLISECONDS = 500.0f;

        private readonly WorldView _worldView;
        private readonly UnitsStack _unitsStack;

        private float _blinkCooldownInMilliseconds;
        private bool _blink;

        private Texture2D _guiTextures;
        private AtlasFrame _slotFrame; 
        private Texture2D _unitTextures;
        private AtlasSpec2 _unitAtlas;

        public List<Point> MovementPath { get; set; }
        public List<Point> PotentialMovementPath { get; set; }
        public bool IsSelected { get; set; }
        public bool IsMovingState { get; set; }
        public float MovementCountdownTime { get; set; }
        public Vector2 CurrentPositionOnScreen { get; set; }

        public Point Location => _unitsStack.Location;
        public float MovementPoints => _unitsStack.MovementPoints;
        public List<string> MovementTypes => _unitsStack.MovementTypes;
        public Unit FirstUnit => _unitsStack[0];

        public UnitsStackView(WorldView worldView, UnitsStack unitsStack)
        {
            _worldView = worldView;
            _unitsStack = unitsStack;
            MovementPath = new List<Point>();
            _blinkCooldownInMilliseconds = BLINK_TIME_IN_MILLISECONDS;
            CurrentPositionOnScreen = HexOffsetCoordinates.ToPixel(unitsStack.Location.X, unitsStack.Location.Y);
        }

        internal void LoadContent(ContentManager content)
        {
            var atlas = AssetsManager.Instance.GetAtlas("Squares");
            _guiTextures = AssetsManager.Instance.GetTexture("Squares");
            _slotFrame = atlas.Frames["SquareGreen"];

            _unitAtlas = AssetsManager.Instance.GetAtlas("Units");
            _unitTextures = AssetsManager.Instance.GetTexture("Units");
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
            var deselectUit = CheckForStackDeselection(_unitsStack);
            if (deselectUit) DeselectStack();
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
            IsSelected = true;
            _blink = true;
            _worldView.Camera.LookAtCell(_unitsStack.Location);
        }

        private bool CheckForStackDeselection(UnitsStack unitsStack)
        {
            if (IsSelected) return unitsStack.MovementPoints <= 0.0f;

            return false;
        }

        private void DeselectStack()
        {
            IsSelected = false;
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
            var sourceRectangle = _slotFrame.ToRectangle();
            spriteBatch.Draw(_guiTextures, destinationRectangle, sourceRectangle, Color.White, 0.0f, new Vector2(sourceRectangle.Width * 0.5f, sourceRectangle.Height * 0.5f), SpriteEffects.None, 0.0f);

            // draw unit icon
            var frame = _unitAtlas.Frames[FirstUnit.UnitTypeTextureName];
            sourceRectangle = frame.ToRectangle();
            destinationRectangle = new Rectangle((int)position.X, (int)position.Y, 36, 32);
            spriteBatch.Draw(_unitTextures, destinationRectangle, sourceRectangle, Color.White, 0.0f, new Vector2(sourceRectangle.Width * 0.5f, sourceRectangle.Height * 0.5f), SpriteEffects.None, 0.0f);
        }

        private void DrawMovementPath(SpriteBatch spriteBatch, List<Point> movementPath, Color color, float radius, float thickness)
        {
            foreach (var item in movementPath)
            {
                var centerPosition = HexOffsetCoordinates.ToPixel(item.X, item.Y);
                spriteBatch.DrawCircle(centerPosition, radius, 10, color, thickness);
            }
        }

        internal void DrawBadge(SpriteBatch spriteBatch, Vector2 topLeftPosition)
        {
            var firstUnit = FirstUnit;

            var destinationRectangle = new Rectangle((int)topLeftPosition.X, (int)topLeftPosition.Y, 70, 70);
            var sourceRectangle = _slotFrame.ToRectangle();
            spriteBatch.Draw(_guiTextures, destinationRectangle, sourceRectangle, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);

            destinationRectangle = new Rectangle((int)topLeftPosition.X + 10, (int)topLeftPosition.Y + 10, 50, 50);
            var frame = _unitAtlas.Frames[firstUnit.UnitTypeTextureName];
            sourceRectangle = frame.ToRectangle();
            spriteBatch.Draw(_unitTextures, destinationRectangle, sourceRectangle, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Count={_unitsStack.Count}}}";
    }
}