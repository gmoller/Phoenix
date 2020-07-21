using AssetsLibrary;
using HexLibrary;
using Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        private float _movementCountdownTime;

        private Vector2 _currentPositionOnScreen;

        private Texture2D _guiTextures;
        private AtlasSpec2 _guiAtlas;
        private Texture2D _unitTextures;
        private AtlasSpec2 _unitAtlas;

        public bool IsSelected { get; private set; }
        public string Name => _unit.Name;
        public string ShortName => _unit.ShortName;

        internal UnitView(WorldView worldView, Unit unit)
        {
            _worldView = worldView;
            _unit = unit;
            _blinkCooldownInMilliseconds = BLINK_TIME_IN_MILLISECONDS;
            _currentPositionOnScreen = HexOffsetCoordinates.ToPixel(unit.Location.X, unit.Location.Y);
        }

        internal void LoadContent(ContentManager content)
        {
            _guiTextures = AssetsManager.Instance.GetTexture("GUI_Textures_1");
            _guiAtlas = AssetsManager.Instance.GetAtlas("GUI_Textures_1");
            _unitTextures = AssetsManager.Instance.GetTexture("Units");
            _unitAtlas = AssetsManager.Instance.GetAtlas("Units");
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
            var restartMovement = CheckForRestartOfMovement();
            if (restartMovement)
            {
                RestartUnitMovement();
            }

            var startUnitMovement = CheckForUnitMovementFromKeyboardInitiation(input);
            if (startUnitMovement.startMovement)
            {
                StartUnitMovement(startUnitMovement.hexToMoveTo);
            }
            else
            {
                startUnitMovement = CheckForUnitMovementFromMouseInitiation(input);
                if (startUnitMovement.startMovement)
                {
                    StartUnitMovement(startUnitMovement.hexToMoveTo);
                }
            }

            if (UnitIsMoving())
            {
                MoveUnit(deltaTime);
                var unitHasReachedNextCell = CheckIfUnitHasReachedNextCell();
                if (unitHasReachedNextCell) MoveUnitToCell();
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

        private bool CheckForRestartOfMovement()
        {
            if (_isMovingState == false && _unit.CurrentPositionInMovementPath > 0 && _unit.MovementPoints > 0.0f)
            {
                return true;
            }

            return false;
        }

        private (bool startMovement, Point hexToMoveTo) CheckForUnitMovementFromKeyboardInitiation(InputHandler input)
        {
            if (!IsSelected || _isMovingState || !input.AreAnyNumPadKeysDown) return (false, new Point(0, 0));

            var direction = DetermineDirection(input);
            if (direction == Direction.None) return (false, new Point(0, 0));

            var neighbor = HexOffsetCoordinates.GetNeighbor(_unit.Location.X, _unit.Location.Y, direction);
            var hexToMoveTo = new Point(neighbor.Col, neighbor.Row);

            var cellToMoveTo = Globals.Instance.World.OverlandMap.CellGrid.GetCell(hexToMoveTo.X, hexToMoveTo.Y);
            var movementCost = Globals.Instance.TerrainTypes[cellToMoveTo.TerrainTypeId].MovementCosts[_unit.MovementTypeName];

            // TODO: assumes all units are walking: checking movement type
            if (movementCost.Cost > 0.0f && _unit.MovementPoints > 0.0f)
            {
                return (true, hexToMoveTo);
            }

            return (false, new Point(0, 0));
        }

        private Direction DetermineDirection(InputHandler input)
        {
            if (input.IsKeyDown(Keys.NumPad4))
            {
                return Direction.West;
            }

            if (input.IsKeyDown(Keys.NumPad6))
            {
                return Direction.East;
            }

            if (input.IsKeyDown(Keys.NumPad7))
            {
                return Direction.NorthWest;
            }

            if (input.IsKeyDown(Keys.NumPad9))
            {
                return Direction.NorthEast;
            }

            if (input.IsKeyDown(Keys.NumPad1))
            {
                return Direction.SouthWest;
            }

            if (input.IsKeyDown(Keys.NumPad3))
            {
                return Direction.SouthEast;
            }

            return Direction.None;
        }

        private (bool startMovement, Point hexToMoveTo) CheckForUnitMovementFromMouseInitiation(InputHandler input)
        {
            if (!IsSelected || _isMovingState || !input.IsLeftMouseButtonReleased) return (false, new Point(0, 0));

            // unit is selected, left mouse button released and unit is not already moving
            var hexToMoveTo = DeviceManager.Instance.WorldHexPointedAtByMouseCursor;
            var cellToMoveTo = Globals.Instance.World.OverlandMap.CellGrid.GetCell(hexToMoveTo.X, hexToMoveTo.Y);
            if (cellToMoveTo.SeenState == SeenState.Never) return (false, new Point(0, 0));
            var movementCost = Globals.Instance.TerrainTypes[cellToMoveTo.TerrainTypeId].MovementCosts[_unit.MovementTypeName];

            // TODO: assumes all units are walking: checking movement type
            if (movementCost.Cost > 0.0f)
            {
                return (true, hexToMoveTo);
            }

            return (false, new Point(0, 0));
        }

        private void RestartUnitMovement()
        {
            _isMovingState = true;
            _movementCountdownTime = MOVEMENT_TIME_BETWEEN_CELLS_IN_MILLISECONDS;
        }

        private void StartUnitMovement(Point hexToMoveTo)
        {
            _unit.MovementPath = DetermineMovementPath(hexToMoveTo);
            _unit.CurrentPositionInMovementPath = 1;

            _isMovingState = true;
            _movementCountdownTime = MOVEMENT_TIME_BETWEEN_CELLS_IN_MILLISECONDS;
        }

        private Point[] DetermineMovementPath(Point hexToMoveTo)
        {
            var hexes = HexOffsetCoordinates.GetLine(_unit.Location.X, _unit.Location.Y, hexToMoveTo.X, hexToMoveTo.Y);
            var result = new Point[hexes.Count];
            var cnt = 0;
            foreach (var hex in hexes)
            {
                result[cnt++] = new Point(hex.Col, hex.Row);
            }

            return result;
        }

        private bool UnitIsMoving()
        {
            return _isMovingState;
        }

        private void MoveUnit(float deltaTime)
        {
            _movementCountdownTime -= deltaTime;

            // determine start cell screen position
            var startPosition = HexOffsetCoordinates.ToPixel(_unit.Location.X, _unit.Location.Y);
            // determine end cell screen position
            var hexToMoveTo = _unit.MovementPath[_unit.CurrentPositionInMovementPath];
            var endPosition = HexOffsetCoordinates.ToPixel(hexToMoveTo.X, hexToMoveTo.Y);
            // lerp between the two positions
            var newPosition = Vector2.Lerp(startPosition, endPosition, 1.0f - _movementCountdownTime / MOVEMENT_TIME_BETWEEN_CELLS_IN_MILLISECONDS);

            _currentPositionOnScreen = newPosition;
        }

        private bool CheckIfUnitHasReachedNextCell()
        {
            return _movementCountdownTime <= 0;
        }

        private void MoveUnitToCell()
        {
            _movementCountdownTime = MOVEMENT_TIME_BETWEEN_CELLS_IN_MILLISECONDS;

            Command moveUnitCommand = new MoveUnitCommand();
            moveUnitCommand.Payload = (_unit, _unit.MovementPath[_unit.CurrentPositionInMovementPath]);
            moveUnitCommand.Execute();

            // if run out of movement points
            if (_unit.MovementPoints <= 0.0f)
            {
                _isMovingState = false;
            }

            // if reached final destination
            if (_unit.CurrentPositionInMovementPath >= _unit.MovementPath.Length - 1)
            {
                _isMovingState = false;
                _unit.MovementPath = null;
                _unit.CurrentPositionInMovementPath = 0;
            }
            else
            {
                _unit.CurrentPositionInMovementPath++;
            }
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