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

        internal UnitView(WorldView worldView, Unit unit)
        {
            _worldView = worldView;
            _unit = unit;
            _blinkCooldownInMilliseconds = BLINK_TIME_IN_MILLISECONDS;
            _currentPositionOnScreen = HexOffsetCoordinates.OffsetCoordinatesToPixel(unit.Location.X, unit.Location.Y);
        }

        internal void LoadContent(ContentManager content)
        {
        }

        internal void Update(InputHandler input, float deltaTime)
        {
            _blink = DetermineBlinkState(_unit, deltaTime);

            // unit movement
            var startUnitMovement = CheckForUnitMovementInitiation(input);
            if (startUnitMovement) StartUnitMovement();
            var unitIsMoving = CheckIfUnitIsMoving();
            if (unitIsMoving)
            {
                MoveUnit(deltaTime);
                var unitHasReachedDestination = CheckIfUnitHasReachedDestination();
                if (unitHasReachedDestination) MoveUnitToCell();
            }

            var selectUnit = CheckForUnitSelection(input);
            if (selectUnit) SelectUnit();
        }

        private bool DetermineBlinkState(Unit unit, float deltaTime)
        {
            if (unit.IsSelected && !CheckIfUnitIsMoving())
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
            if (_unit.IsSelected && input.IsLeftMouseButtonReleased && _isMovingState == false)
            {
                var currentHex = _unit.Location;
                var hexToMoveTo = GetHexPoint();

                if (IsWithinOneHexOf(currentHex, hexToMoveTo))
                {
                    var cellToMoveTo = Globals.Instance.World.OverlandMap.CellGrid.GetCell(hexToMoveTo.X, hexToMoveTo.Y);
                    var movementCost = Globals.Instance.TerrainTypes[cellToMoveTo.TerrainTypeId].MovementCosts[_unit.MovementTypeName];

                    if (_unit.MovementPoints >= movementCost.Moves)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void StartUnitMovement()
        {
            _isMovingState = true;
            _hexToMoveTo = GetHexPoint();
            _movementCountdownTime = MOVEMENT_TIME_BETWEEN_CELLS_IN_MILLISECONDS;
        }

        private bool CheckIfUnitIsMoving()
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
            Globals.Instance.MessageQueue.Enqueue(moveUnitCommand);
        }

        private bool CheckForUnitSelection(InputHandler input)
        {
            return !_unit.IsSelected && input.IsRightMouseButtonReleased && CursorIsOnThisUnit();
        }

        private void SelectUnit()
        {
            // TODO: show in hudview
            Command selectUnitCommand = new SelectUnitCommand();
            selectUnitCommand.Payload = _unit;
            Globals.Instance.MessageQueue.Enqueue(selectUnitCommand);

            var hexPoint = GetHexPoint();
            var worldPixelLocation = HexOffsetCoordinates.OffsetCoordinatesToPixel(hexPoint.X, hexPoint.Y);
            _worldView.Camera.LookAt(worldPixelLocation);
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
            var hexPoint = GetHexPoint();

            return _unit.Location == hexPoint;
        }

        private Point GetHexPoint()
        {
            var hex = DeviceManager.Instance.WorldHex;
            var hexPoint = new Point(hex.X, hex.Y);

            return hexPoint;
        }

        internal void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            if (!_unit.IsSelected)
            {
                DrawUnit(spriteBatch, texture);
            }
            else if (!_blink)
            {
                DrawUnit(spriteBatch, texture);
            }
        }

        private void DrawUnit(SpriteBatch spriteBatch, Texture2D texture)
        {
            //var position = HexOffsetCoordinates.OffsetCoordinatesToPixel(_unit.Location.X, _unit.Location.Y);
            var destinationRectangle = new Rectangle((int)_currentPositionOnScreen.X, (int)_currentPositionOnScreen.Y, 50, 50);
            var sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);

            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.Navy, 0.0f, new Vector2(texture.Width / 2.0f, texture.Height / 2.0f), SpriteEffects.None, 0.0f);
        }
    }
}