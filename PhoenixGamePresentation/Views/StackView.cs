using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GuiControls;
using Hex;
using Input;
using MonoGameUtilities;
using MonoGameUtilities.ExtensionMethods;
using PhoenixGameLibrary;
using PhoenixGameLibrary.Commands;
using PhoenixGamePresentation.Handlers;
using Utilities;
using Utilities.ExtensionMethods;

namespace PhoenixGamePresentation.Views
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    internal class StackView : IDisposable
    {
        #region State
        private const float BLINK_TIME_IN_MILLISECONDS = 250.0f;
        private const float MOVEMENT_TIME_BETWEEN_CELLS_IN_MILLISECONDS = 250.0f;

        private readonly WorldView _worldView;
        private readonly StackViews _stackViews;
        private readonly Stack _stack;

        private Vector2 _currentPositionOnScreen;
        private List<PointI> _movementPath;
        private List<PointI> _potentialMovementPath;

        private float _blinkCooldownInMilliseconds;
        private bool _blink;

        public Guid Id { get; }
        public bool IsMovingState { get; private set; }
        public float MovementCountdownTime { get; private set; }

        private readonly InputHandler _input;
        private bool _disposedValue;
        #endregion End State

        public HexOffsetCoordinates LocationHex => new HexOffsetCoordinates(Location);

        public bool IsBusy => _stack.IsBusy;
        public UnitStatus Status => _stack.Status;

        public EnumerableList<PointI> MovementPath => new EnumerableList<PointI>(_movementPath);
        public bool IsSelected => _stackViews.Current == this;

        public EnumerableList<string> Actions => _stack.Actions;

        public PointI Location => _stack.Location;
        public float MovementPoints => _stack.MovementPoints;
        public int SightRange => _stack.SightRange;

        public int Count => _stack.Count;

        public StackView(WorldView worldView, StackViews stackViews, Stack stack, InputHandler input)
        {
            Id = Guid.NewGuid();
            _worldView = worldView;
            _stackViews = stackViews;
            _stack = stack;
            _movementPath = new List<PointI>();
            _potentialMovementPath = new List<PointI>();
            _blinkCooldownInMilliseconds = BLINK_TIME_IN_MILLISECONDS;
            _currentPositionOnScreen = HexOffsetCoordinates.ToPixel(stack.Location.X, stack.Location.Y).ToVector2();

            input.SubscribeToEventHandler($"StackView:{Id}", 0, new KeyboardInputAction(Keys.C, KeyboardInputActionType.Released, FocusCameraOnLocation));
            input.SubscribeToEventHandler($"StackView:{Id}", 0, new KeyboardInputAction(Keys.NumPad1, KeyboardInputActionType.Released, CheckForUnitMovementFromKeyboardInitiation));
            input.SubscribeToEventHandler($"StackView:{Id}", 0, new KeyboardInputAction(Keys.NumPad3, KeyboardInputActionType.Released, CheckForUnitMovementFromKeyboardInitiation));
            input.SubscribeToEventHandler($"StackView:{Id}", 0, new KeyboardInputAction(Keys.NumPad4, KeyboardInputActionType.Released, CheckForUnitMovementFromKeyboardInitiation));
            input.SubscribeToEventHandler($"StackView:{Id}", 0, new KeyboardInputAction(Keys.NumPad6, KeyboardInputActionType.Released, CheckForUnitMovementFromKeyboardInitiation));
            input.SubscribeToEventHandler($"StackView:{Id}", 0, new KeyboardInputAction(Keys.NumPad7, KeyboardInputActionType.Released, CheckForUnitMovementFromKeyboardInitiation));
            input.SubscribeToEventHandler($"StackView:{Id}", 0, new KeyboardInputAction(Keys.NumPad9, KeyboardInputActionType.Released, CheckForUnitMovementFromKeyboardInitiation));
            input.SubscribeToEventHandler($"StackView:{Id}", 0, new MouseInputAction(MouseInputActionType.LeftButtonReleased, CheckForUnitMovementFromMouseInitiation));
            input.SubscribeToEventHandler($"StackView:{Id}", 0, new MouseInputAction(MouseInputActionType.RightButtonPressed, DrawPotentialMovementPath));
            input.SubscribeToEventHandler($"StackView:{Id}", 0, new MouseInputAction(MouseInputActionType.RightButtonReleased, SelectStack));
            input.SubscribeToEventHandler($"StackView:{Id}", 1, new MouseInputAction(MouseInputActionType.RightButtonReleased, ResetPotentialMovementPath));
            _input = input;
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

        internal bool HasNoMovementPath()
        {
            return MovementPath.Count == 0;
        }

        internal GetCostToMoveIntoResult GetCostToMoveInto(PointI location)
        {
            return _stack.GetCostToMoveInto(location);
        }

        internal GetCostToMoveIntoResult GetCostToMoveInto(Cell location)
        {
            return _stack.GetCostToMoveInto(location);
        }

        internal void Update(float deltaTime)
        {
            if (_worldView.GameStatus == GameStatus.CityView) return;

            // Causes
            var changeBlinkState = CheckForBlinkStateChange(deltaTime);
            var mustFindNewExploreLocation = ExploreHandler.MustFindNewExploreLocation(this);
            var mustRestartMovementAtStartOfTurn = MovementHandler.CheckForRestartOfMovement(this); // not in moving state, has a path and has movement points
            var mustContinueMovement = MovementHandler.MustContinueMovement(this);
            var mustMoveUnitToNextCell = MovementHandler.MustMoveUnitToNextCell(this);

            if (!IsSelected) return;

            if (changeBlinkState)
            {
                _blink = !_blink;
                _blinkCooldownInMilliseconds = BLINK_TIME_IN_MILLISECONDS;
            }

            if (mustFindNewExploreLocation)
            {
                ExploreHandler.SetMovementPathToNewExploreLocation(this, _worldView.World);
            }

            if (mustRestartMovementAtStartOfTurn)
            {
                RestartUnitMovement();
            }

            if (mustContinueMovement)
            {
                MoveStack(deltaTime);
            }

            if (mustMoveUnitToNextCell)
            {
                MoveStackToCell();
            }
        }

        private void SetAsCurrent()
        {
            _blink = true;
            _blinkCooldownInMilliseconds = BLINK_TIME_IN_MILLISECONDS;
            _movementPath = new List<PointI>();
            _stackViews.SetCurrent(this);
        }

        private bool CheckForBlinkStateChange(float deltaTime)
        {
            if (IsMovingState) return false;

            _blinkCooldownInMilliseconds -= deltaTime;
            if (_blinkCooldownInMilliseconds > 0.0f) return false;

            return true;
        }

        internal void SetMovementPath(List<PointI> path)
        {
            _movementPath = path;
        }

        private void RestartUnitMovement()
        {
            StartUnitMovement();
        }

        private void StartUnitMovement(PointI hexToMoveTo)
        {
            var path = MovementPathDeterminer.DetermineMovementPath(this, Location, hexToMoveTo, _worldView.World.OverlandMap.CellGrid);
            SetMovementPath(path);

            StartUnitMovement();
        }

        private void StartUnitMovement()
        {
            IsMovingState = true;
            _blink = false;
            _blinkCooldownInMilliseconds = BLINK_TIME_IN_MILLISECONDS;
            MovementCountdownTime = MOVEMENT_TIME_BETWEEN_CELLS_IN_MILLISECONDS;
        }

        private void MoveStack(float deltaTime)
        {
            // if stack cannot move into next hex in path
            var cost = _stack.GetCostToMoveInto(MovementPath[0]);
            if (!cost.CanMoveInto && Status != UnitStatus.Explore)
            {
                DeselectStack();
                return;
            }

            if (!cost.CanMoveInto && Status == UnitStatus.Explore)
            {
                // if exploring: pick new path
                SetMovementPath(new List<PointI>());
                ExploreHandler.SetMovementPathToNewExploreLocation(this, _worldView.World);
            }

            MovementCountdownTime -= deltaTime;

            // determine start cell screen position
            var startPosition = HexOffsetCoordinates.ToPixel(Location.X, Location.Y).ToVector2();
            // determine end cell screen position
            var hexToMoveTo = MovementPath[0];
            var endPosition = HexOffsetCoordinates.ToPixel(hexToMoveTo.X, hexToMoveTo.Y).ToVector2();
            // lerp between the two positions
            var newPosition = Vector2.Lerp(startPosition, endPosition, 1.0f - MovementCountdownTime / MOVEMENT_TIME_BETWEEN_CELLS_IN_MILLISECONDS);

            _currentPositionOnScreen = newPosition;
            _worldView.Camera.LookAtPixel(new PointI((int)newPosition.X, (int)newPosition.Y));
        }

        private void MoveStackToCell()
        {
            MovementCountdownTime = MOVEMENT_TIME_BETWEEN_CELLS_IN_MILLISECONDS;

            Command moveUnitCommand = new MoveUnitCommand { Payload = (_stack, MovementPath[0]) };
            moveUnitCommand.Execute();
            RemoveFirstItemFromMovementPath();

            // if run out of movement points
            if (MovementPoints <= 0.0f)
            {
                IsMovingState = false;
                DeselectStack();
            }

            // if reached final destination
            if (MovementPath.Count == 0)
            {
                IsMovingState = false;
            }
        }

        private void RemoveFirstItemFromMovementPath()
        {
            _movementPath.RemoveAt(0);
        }

        private void DeselectStack()
        {
            if (_stackViews.Current == null || Id == _stackViews.Current.Id)
            {
                _stackViews.SelectNext();
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

        internal void DoBuildAction()
        {
            _stack.DoBuildAction();
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
            spriteBatch.Draw(_stackViews.GuiTextures, destinationRectangle, sourceRectangle, Color.White, 0.0f, new Vector2(sourceRectangle.Width * Constants.ONE_HALF, sourceRectangle.Height * 0.5f), SpriteEffects.None, 0.0f);

            // draw unit icon
            destinationRectangle = new Rectangle((int)_currentPositionOnScreen.X, (int)_currentPositionOnScreen.Y, 36, 32);
            var frame = _stackViews.UnitAtlas.Frames[_stack[0].UnitTypeTextureName];
            sourceRectangle = frame.ToRectangle();
            spriteBatch.Draw(_stackViews.UnitTextures, destinationRectangle, sourceRectangle, Color.White, 0.0f, new Vector2(sourceRectangle.Width * Constants.ONE_HALF, sourceRectangle.Height * 0.5f), SpriteEffects.None, 0.0f);
        }

        private void DrawMovementPath(SpriteBatch spriteBatch, List<PointI> movementPath, Color color, float radius, float thickness)
        {
            foreach (var item in movementPath)
            {
                var centerPosition = HexOffsetCoordinates.ToPixel(item.X, item.Y).ToVector2();
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
            var x = topLeftPosition.X + 60.0f * Constants.ONE_HALF;
            var y = topLeftPosition.Y + 60.0f * Constants.ONE_HALF;
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
            spriteBatch.Draw(_stackViews.GuiTextures, destinationRectangle, sourceRectangle, Color.White, 0.0f, new Vector2(sourceRectangle.Width * Constants.ONE_HALF, sourceRectangle.Height * Constants.ONE_HALF), SpriteEffects.FlipVertically, 0.0f);

            // draw unit icon
            destinationRectangle = new Rectangle((int)centerPosition.X, (int)centerPosition.Y, 36, 32);
            var frame = _stackViews.UnitAtlas.Frames[unit.UnitTypeTextureName];
            sourceRectangle = frame.ToRectangle();
            spriteBatch.Draw(_stackViews.UnitTextures, destinationRectangle, sourceRectangle, Color.White, 0.0f, new Vector2(sourceRectangle.Width * Constants.ONE_HALF, sourceRectangle.Height * Constants.ONE_HALF), SpriteEffects.None, 0.0f);
        }

        #region Event Handlers

        private void SelectStack(object sender, MouseEventArgs e)
        {
            if (IsSelected) return;
            if (!MousePointerIsOnHex(LocationHex, e.Mouse.Location)) return;

            SetAsCurrent();
        }

        private bool MousePointerIsOnHex(HexOffsetCoordinates settlementLocation, Point mouseLocation)
        {
            return mouseLocation.IsWithinHex(settlementLocation, _worldView.Camera.Transform);
        }

        private void FocusCameraOnLocation(object sender, KeyboardEventArgs e)
        {
            if (!IsSelected) return;
            if (_worldView.GameStatus == GameStatus.CityView) return;

            _worldView.Camera.LookAtCell(Location);
        }

        private void DrawPotentialMovementPath(object sender, MouseEventArgs e)
        {
            if (!IsSelected) return;
            if (Status == UnitStatus.Explore || IsMovingState) return;

            var path = PotentialMovementHandler.GetPotentialMovementPath(this, _worldView.World.OverlandMap.CellGrid, e.Mouse.Location, _worldView.Camera);
            _potentialMovementPath = path;
        }

        private void ResetPotentialMovementPath(object sender, MouseEventArgs e)
        {
            if (!IsSelected) return;
            _potentialMovementPath = new List<PointI>();
        }

        private void CheckForUnitMovementFromKeyboardInitiation(object sender, KeyboardEventArgs e)
        {
            if (!IsSelected) return;
            if (_worldView.GameStatus == GameStatus.CityView) return;
            if (IsMovingState) return;
            if (MovementPoints.AboutEquals(0.0f)) return;

            Direction direction;
            switch (e.Key)
            {
                case Keys.NumPad1:
                    direction = Direction.SouthWest;
                    break;
                case Keys.NumPad3:
                    direction = Direction.SouthEast;
                    break;
                case Keys.NumPad4:
                    direction = Direction.West;
                    break;
                case Keys.NumPad6:
                    direction = Direction.East;
                    break;
                case Keys.NumPad7:
                    direction = Direction.NorthWest;
                    break;
                case Keys.NumPad9:
                    direction = Direction.NorthEast;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var neighbor = HexOffsetCoordinates.GetNeighbor(Location.X, Location.Y, direction);
            var hexToMoveTo = new PointI(neighbor.Col, neighbor.Row);

            var costToMoveIntoResult = GetCostToMoveInto(hexToMoveTo);

            if (costToMoveIntoResult.CanMoveInto)
            {
                StartUnitMovement(hexToMoveTo);
            }
        }

        private void CheckForUnitMovementFromMouseInitiation(object sender, MouseEventArgs e)
        {
            if (!IsSelected) return;
            if (_worldView.GameStatus == GameStatus.CityView) return;
            if (_worldView.GameStatus == GameStatus.InHudView) return;
            if (IsMovingState) return;
            if (MovementPoints.AboutEquals(0.0f)) return;

            var mustStartMovement = MovementHandler.CheckForUnitMovementFromMouseInitiation(this, _worldView.World.OverlandMap.CellGrid, e.Mouse.Location, _worldView.Camera);

            if (mustStartMovement.startMovement)
            {
                StartUnitMovement(mustStartMovement.hexToMoveTo);
            }
        }

        #endregion

        public override string ToString()
        {
            return DebuggerDisplay;
        } 

        private string DebuggerDisplay => $"{{Id={Id},UnitsInStack={_stack.Count}}}";

        public void Dispose()
        {
            if (!_disposedValue)
            {
                // dispose managed state (managed objects)
                _input.UnsubscribeFromEventHandler($"StackView:{Id}", 0, new KeyboardInputAction(Keys.C, KeyboardInputActionType.Released, FocusCameraOnLocation));
                _input.UnsubscribeFromEventHandler($"StackView:{Id}", 0, new KeyboardInputAction(Keys.NumPad1, KeyboardInputActionType.Released, CheckForUnitMovementFromKeyboardInitiation));
                _input.UnsubscribeFromEventHandler($"StackView:{Id}", 0, new KeyboardInputAction(Keys.NumPad3, KeyboardInputActionType.Released, CheckForUnitMovementFromKeyboardInitiation));
                _input.UnsubscribeFromEventHandler($"StackView:{Id}", 0, new KeyboardInputAction(Keys.NumPad4, KeyboardInputActionType.Released, CheckForUnitMovementFromKeyboardInitiation));
                _input.UnsubscribeFromEventHandler($"StackView:{Id}", 0, new KeyboardInputAction(Keys.NumPad6, KeyboardInputActionType.Released, CheckForUnitMovementFromKeyboardInitiation));
                _input.UnsubscribeFromEventHandler($"StackView:{Id}", 0, new KeyboardInputAction(Keys.NumPad7, KeyboardInputActionType.Released, CheckForUnitMovementFromKeyboardInitiation));
                _input.UnsubscribeFromEventHandler($"StackView:{Id}", 0, new KeyboardInputAction(Keys.NumPad9, KeyboardInputActionType.Released, CheckForUnitMovementFromKeyboardInitiation));
                _input.UnsubscribeFromEventHandler($"StackView:{Id}", 0, new MouseInputAction(MouseInputActionType.LeftButtonReleased, CheckForUnitMovementFromMouseInitiation));
                _input.UnsubscribeFromEventHandler($"StackView:{Id}", 0, new MouseInputAction(MouseInputActionType.RightButtonPressed, DrawPotentialMovementPath));
                _input.UnsubscribeFromEventHandler($"StackView:{Id}", 0, new MouseInputAction(MouseInputActionType.RightButtonReleased, SelectStack));
                _input.UnsubscribeFromEventHandler($"StackView:{Id}", 1, new MouseInputAction(MouseInputActionType.RightButtonReleased, ResetPotentialMovementPath));

                // set large fields to null
                _movementPath = null;
                _potentialMovementPath = null;

                _disposedValue = true;
            }

            GC.SuppressFinalize(this);
        }
    }
}