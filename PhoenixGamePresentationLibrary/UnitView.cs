﻿using AssetsLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using HexLibrary;
using Input;
using PhoenixGameLibrary;
using PhoenixGameLibrary.Commands;
using PhoenixGameLibrary.GameData;
using Utilities;
using Point = Utilities.Point;

namespace PhoenixGamePresentationLibrary
{
    public class UnitView
    {
        private const float BLINK_TIME_IN_MILLISECONDS = 500.0f;
        private const float MOVEMENT_TIME_BETWEEN_CELLS_IN_MILLISECONDS = 500.0f;

        private readonly WorldView _worldView;
        private readonly Unit _unit;
        private float _blinkCooldownInMilliseconds;
        private bool _blink;

        private bool _isMovingState;
        private Point _hexToMoveTo;
        private float _movementCountdownTime;

        private Vector2 _currentPositionOnScreen;

        private Texture2D _unitTextures;
        private AtlasSpec2 _unitAtlas;
        private Texture2D _guiTextures;
        private AtlasSpec2 _guiAtlas;

        public bool IsSelected { get; private set; }
        public string Name => _unit.Name;
        public string ShortName => _unit.ShortName;

        internal UnitView(WorldView worldView, Unit unit)
        {
            _worldView = worldView;
            _unit = unit;
            _blinkCooldownInMilliseconds = BLINK_TIME_IN_MILLISECONDS;
            _currentPositionOnScreen = HexOffsetCoordinates.OffsetCoordinatesToPixel(unit.Location.X, unit.Location.Y);
        }

        internal void LoadContent(ContentManager content)
        {
            _unitTextures = AssetsManager.Instance.GetTexture("Units");
            _unitAtlas = AssetsManager.Instance.GetAtlas("Units");
            _guiTextures = AssetsManager.Instance.GetTexture("GUI_Textures_1");
            _guiAtlas = AssetsManager.Instance.GetAtlas("GUI_Textures_1");
        }

        internal void SelectUnit()
        {
            IsSelected = true;
            _worldView.Camera.LookAtCell(_unit.Location);
        }

        internal void Update(InputHandler input, float deltaTime)
        {
            _blink = DetermineBlinkState(deltaTime);

            // unit movement
            var startUnitMovement = CheckForUnitMovementInitiation(input);
            if (startUnitMovement) StartUnitMovement();
            if (UnitIsMoving())
            {
                MoveUnit(deltaTime);
                var unitHasReachedDestination = CheckIfUnitHasReachedDestination();
                if (unitHasReachedDestination) MoveUnitToCell();
            }

            var selectUnit = CheckForUnitSelection(input);
            if (selectUnit) SelectUnit();
            var deselectUit = CheckForUnitDeselection(_unit);
            if (deselectUit) DeselectUnit();
        }

        private bool DetermineBlinkState(float deltaTime)
        {
            if (IsSelected && !UnitIsMoving())
            {
                _blinkCooldownInMilliseconds -= deltaTime;
                if (_blinkCooldownInMilliseconds > 0.0f) return _blink;

                // flip blink state
                _blink = !_blink;
                _blinkCooldownInMilliseconds = BLINK_TIME_IN_MILLISECONDS;
            }
            else
            {
                _blink = false;
            }

            return _blink;
        }

        private bool CheckForUnitMovementInitiation(InputHandler input)
        {
            if (!IsSelected || !input.IsLeftMouseButtonReleased || _isMovingState) return false;

            var currentHex = _unit.Location;
            var hexToMoveTo = DeviceManager.Instance.WorldHexPointedAtByMouseCursor;

            if (IsWithinOneHexOf(currentHex, hexToMoveTo))
            {
                var cellToMoveTo = Globals.Instance.World.OverlandMap.CellGrid.GetCell(hexToMoveTo.X, hexToMoveTo.Y);
                var movementCost = Globals.Instance.TerrainTypes[cellToMoveTo.TerrainTypeId].MovementCosts[_unit.MovementTypeName];

                // TODO: assumes all units are walking: checking movement type
                if (movementCost.Cost > 0.0f && _unit.MovementPoints > 0.0f)
                {
                    return true;
                }
            }

            return false;
        }

        private void StartUnitMovement()
        {
            _isMovingState = true;
            _hexToMoveTo = DeviceManager.Instance.WorldHexPointedAtByMouseCursor;
            _movementCountdownTime = MOVEMENT_TIME_BETWEEN_CELLS_IN_MILLISECONDS;
        }

        private bool UnitIsMoving()
        {
            return _isMovingState;
        }

        private void MoveUnit(float deltaTime)
        {
            _movementCountdownTime -= deltaTime;

            // determine start cell screen position
            var startPosition = HexOffsetCoordinates.OffsetCoordinatesToPixel(_unit.Location.X, _unit.Location.Y);
            // determine end cell screen position
            var endPosition = HexOffsetCoordinates.OffsetCoordinatesToPixel(_hexToMoveTo.X, _hexToMoveTo.Y);
            // lerp between the two positions
            var newPosition = Vector2.Lerp(startPosition, endPosition, 1.0f - _movementCountdownTime / MOVEMENT_TIME_BETWEEN_CELLS_IN_MILLISECONDS);

            _currentPositionOnScreen = newPosition;
        }

        private bool CheckIfUnitHasReachedDestination()
        {
            return _movementCountdownTime <= 0;
        }

        private void MoveUnitToCell()
        {
            _isMovingState = false;

            Command moveUnitCommand = new MoveUnitCommand();
            moveUnitCommand.Payload = (_unit, _hexToMoveTo);
            moveUnitCommand.Execute();
        }

        private bool CheckForUnitSelection(InputHandler input)
        {
            if (IsSelected) return false;
            if (input.IsRightMouseButtonReleased) return CursorIsOnThisUnit();

            return false;
        }

        private bool CheckForUnitDeselection(Unit unit)
        {
            if (IsSelected) return unit.MovementPoints <= 0.0f;

            return false;
        }

        private void DeselectUnit()
        {
            IsSelected = false;
        }

        private bool IsWithinOneHexOf(Point currentHex, Point hexToMoveTo)
        {
            var neighbors = HexOffsetCoordinates.GetAllNeighbors(currentHex.X, currentHex.Y);

            foreach (var neighbor in neighbors)
            {
                if (neighbor.Col == hexToMoveTo.X && neighbor.Row == hexToMoveTo.Y)
                {
                    return true;
                }
            }

            return false;
        }

        private bool CursorIsOnThisUnit()
        {
            var hexPoint = DeviceManager.Instance.WorldHexPointedAtByMouseCursor;

            return _unit.Location == hexPoint;
        }

        internal void DrawBadge(SpriteBatch spriteBatch, Vector2 topLeftPosition)
        {
            var destinationRectangle = new Rectangle((int)topLeftPosition.X, (int)topLeftPosition.Y, 70, 70);
            var frame = _guiAtlas.Frames["sp_frame"];
            var sourceRectangle = frame.ToRectangle();
            spriteBatch.Draw(_guiTextures, destinationRectangle, sourceRectangle, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);

            destinationRectangle = new Rectangle((int)topLeftPosition.X + 10, (int)topLeftPosition.Y + 10, 50, 50);
            if (IsSelected)
            {
                spriteBatch.FillRectangle(destinationRectangle, Color.Green, 0.0f);
            }
            frame = _unitAtlas.Frames[_unit.UnitTypeTextureName];
            sourceRectangle = frame.ToRectangle();
            spriteBatch.Draw(_unitTextures, destinationRectangle, sourceRectangle, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
        }

        internal void Draw(SpriteBatch spriteBatch)
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
        }

        private void DrawUnit(SpriteBatch spriteBatch)
        {
            var destinationRectangle = new Rectangle((int)_currentPositionOnScreen.X, (int)_currentPositionOnScreen.Y, 50, 50);
            var frame = _unitAtlas.Frames[_unit.UnitTypeTextureName];
            var sourceRectangle = frame.ToRectangle();
            spriteBatch.Draw(_unitTextures, destinationRectangle, sourceRectangle, Color.White, 0.0f, new Vector2(frame.Width / 2.0f, frame.Height / 2.0f), SpriteEffects.None, 0.0f);
        }
    }
}