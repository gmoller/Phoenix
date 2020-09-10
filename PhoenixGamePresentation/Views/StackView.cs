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
using PhoenixGamePresentation.Actions;
using PhoenixGamePresentation.Conditions;
using PhoenixGamePresentation.Events;
using PhoenixGamePresentation.Handlers;
using Utilities;

namespace PhoenixGamePresentation.Views
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    internal class StackView : IDisposable
    {
        #region State
        private const float BLINK_TIME_IN_MILLISECONDS = 250.0f;
        private const float MOVEMENT_TIME_BETWEEN_CELLS_IN_MILLISECONDS = 250.0f;

        internal WorldView WorldView { get; }
        private readonly StackViews _stackViews;
        private readonly Stack _stack;

        private Vector2 _currentPositionOnScreen;
        private List<PointI> _movementPath;
        internal List<PointI> PotentialMovementPath { get; set; }

        private float _blinkCooldownInMilliseconds;
        private bool _blink;

        public Guid Id { get; }
        public bool IsMovingState { get; private set; }
        public float MovementCountdownTime { get; private set; }

        private readonly IfThenElseProcessor _ifThenElseProcessor;
        private InputHandler Input { get; } // readonly
        private bool IsDisposed { get; set; }
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
            WorldView = worldView;
            _stackViews = stackViews;
            _stack = stack;
            _movementPath = new List<PointI>();
            PotentialMovementPath = new List<PointI>();
            _blinkCooldownInMilliseconds = BLINK_TIME_IN_MILLISECONDS;
            _currentPositionOnScreen = HexOffsetCoordinates.ToPixel(stack.Location.X, stack.Location.Y).ToVector2();

            Input = input;
            Input.BeginRegistration(GameStatus.OverlandMap.ToString(), $"StackView:{Id}");
            Input.Register(0, this, Keys.C, KeyboardInputActionType.Released, FocusCameraOnLocationEvent.HandleEvent);
            Input.Register(1, this, Keys.NumPad1, KeyboardInputActionType.Released, CheckForUnitMovementFromKeyboardInitiationEvent.HandleEvent);
            Input.Register(2, this, Keys.NumPad3, KeyboardInputActionType.Released, CheckForUnitMovementFromKeyboardInitiationEvent.HandleEvent);
            Input.Register(3, this, Keys.NumPad4, KeyboardInputActionType.Released, CheckForUnitMovementFromKeyboardInitiationEvent.HandleEvent);
            Input.Register(4, this, Keys.NumPad6, KeyboardInputActionType.Released, CheckForUnitMovementFromKeyboardInitiationEvent.HandleEvent);
            Input.Register(5, this, Keys.NumPad7, KeyboardInputActionType.Released, CheckForUnitMovementFromKeyboardInitiationEvent.HandleEvent);
            Input.Register(6, this, Keys.NumPad9, KeyboardInputActionType.Released, CheckForUnitMovementFromKeyboardInitiationEvent.HandleEvent);
            Input.Register(7, this, MouseInputActionType.LeftButtonReleased, CheckForUnitMovementFromMouseInitiationEvent.HandleEvent);
            Input.Register(8, this, MouseInputActionType.RightButtonPressed, DrawPotentialMovementPathEvent.HandleEvent);
            Input.Register(9, this, MouseInputActionType.RightButtonReleased, SelectStackEvent.HandleEvent);
            Input.Register(10, this, MouseInputActionType.RightButtonReleased, ResetPotentialMovementPathEvent.HandleEvent);
            Input.EndRegistration();
            //Input.BeginRegistration(GameStatus.CityView.ToString(), $"StackView:{Id}");
            //Input.Register(8, this, MouseInputActionType.RightButtonPressed, DrawPotentialMovementPathEvent.HandleEvent);
            //Input.Register(10, this, MouseInputActionType.RightButtonReleased, ResetPotentialMovementPathEvent.HandleEvent);
            //Input.EndRegistration();

            Input.Subscribe(GameStatus.OverlandMap.ToString(), $"StackView:{Id}");

            WorldView.SubscribeToStatusChanges($"StackView:{Id}", WorldView.HandleStatusChange);

            _ifThenElseProcessor = new IfThenElseProcessor();
            _ifThenElseProcessor.Add($"StackView:{Id}", 0, this, CheckForBlinkStateChange, (sender, e) => { _blink = !_blink; _blinkCooldownInMilliseconds = BLINK_TIME_IN_MILLISECONDS; });
            _ifThenElseProcessor.Add($"StackView:{Id}", 1, this, MoveStackToNextCellCondition.EvaluateCondition, MoveStackToCellAction.DoAction);
            _ifThenElseProcessor.Add($"StackView:{Id}", 2, this, ExploreHandler.MustFindNewExploreLocation, ExploreHandler.SetMovementPathToNewExploreLocation);
            _ifThenElseProcessor.Add($"StackView:{Id}", 3, this, MovementHandler.CheckForRestartOfMovement, (sender, e) => { RestartUnitMovement(); });
            _ifThenElseProcessor.Add($"StackView:{Id}", 4, this, MovementHandler.MustContinueMovement, (sender, e) => { MoveStack(e.DeltaTime); });
        }

        internal List<IControl> GetMovementTypeImages()
        {
            // TODO: update will not be called on these
            var imgMovementTypes = new List<IControl>();
            foreach (var movementType in _stack.MovementTypes)
            {
                var img = WorldView.GetMovementTypeImages[movementType];
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
            //if (WorldView.GameStatus == GameStatus.CityView) return;
            if (!IsSelected) return;

            _ifThenElseProcessor.Update(deltaTime);
        }

        internal void SetAsCurrent()
        {
            _blink = true;
            _blinkCooldownInMilliseconds = BLINK_TIME_IN_MILLISECONDS;
            _movementPath = new List<PointI>();
            _stackViews.SetCurrent(this);
        }

        private bool CheckForBlinkStateChange(object sender, float deltaTime)
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

        internal void StartUnitMovement(PointI hexToMoveTo)
        {
            var path = MovementPathDeterminer.DetermineMovementPath(this, Location, hexToMoveTo, WorldView.CellGrid);
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
                ExploreHandler.SetMovementPathToNewExploreLocation(this, new ActionArgs(deltaTime));
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
            WorldView.Camera.LookAtPixel(new PointI((int)newPosition.X, (int)newPosition.Y));
        }

        internal void MoveStackToCell()
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
            DrawMovementPath(spriteBatch, PotentialMovementPath, Color.White, 3.0f, 1.0f);
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

        public override string ToString()
        {
            return DebuggerDisplay;
        } 

        private string DebuggerDisplay => $"{{Id={Id},UnitsInStack={_stack.Count}}}";

        public void Dispose()
        {
            if (!IsDisposed)
            {
                // dispose managed state (managed objects)
                Input.UnsubscribeAllFromEventHandler($"StackView:{Id}");

                // set large fields to null
                _movementPath = null;
                PotentialMovementPath = null;

                IsDisposed = true;
            }
        }
    }
}