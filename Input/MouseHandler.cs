using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Input
{
    internal static class MouseHandler
    {
        #region State
        private static MouseState _currentState;
        private static MouseState _previousState;
        private static readonly Dictionary<MouseInputActionType, Func<bool>> Switch = new Dictionary<MouseInputActionType, Func<bool>>
        {
            { MouseInputActionType.Moved, () => MouseMovement != Point.Zero },
            { MouseInputActionType.LeftButtonDown, IsLeftButtonDown },
            { MouseInputActionType.MiddleButtonDown, IsMiddleButtonDown },
            { MouseInputActionType.RightButtonDown, IsRightButtonDown },
            { MouseInputActionType.LeftButtonPressed, IsLeftButtonPressed },
            { MouseInputActionType.MiddleButtonPressed, IsMiddleButtonPressed },
            { MouseInputActionType.RightButtonPressed, IsRightButtonPressed },
            { MouseInputActionType.LeftButtonReleased, IsLeftButtonReleased },
            { MouseInputActionType.MiddleButtonReleased, IsMiddleButtonReleased },
            { MouseInputActionType.RightButtonReleased, IsRightButtonReleased },
            { MouseInputActionType.WheelUp, MouseWheelUp },
            { MouseInputActionType.WheelDown, MouseWheelDown },
            { MouseInputActionType.LeftButtonDrag, () => IsLeftButtonDown() && HasMouseMoved() },
            { MouseInputActionType.MiddleButtonDrag, () => IsMiddleButtonDown() && HasMouseMoved() },
            { MouseInputActionType.RightButtonDrag, () => IsRightButtonDown() && HasMouseMoved() }
        };
        #endregion End State

        internal static void Initialize()
        {
            _currentState = Mouse.GetState();
        }

        internal static void Update(Dictionary<string, Dictionary<string, MouseInputAction>> mouseEventHandlers, float deltaTime)
        {
            _previousState = _currentState;
            _currentState = Mouse.GetState();

            HandleMouse(mouseEventHandlers, deltaTime);
        }

        internal static Point MousePosition => _currentState.Position;

        private static Point MouseMovement => _currentState.Position - _previousState.Position;

        private static bool IsLeftButtonDown()
        {
            return _currentState.LeftButton == ButtonState.Pressed;
        }

        private static bool IsLeftButtonPressed()
        {
            return _previousState.LeftButton == ButtonState.Released && _currentState.LeftButton == ButtonState.Pressed;
        }

        internal static bool IsLeftButtonReleased()
        {
            return _previousState.LeftButton == ButtonState.Pressed && _currentState.LeftButton == ButtonState.Released;
        }

        private static bool IsMiddleButtonDown()
        {
            return _currentState.MiddleButton == ButtonState.Pressed;
        }

        private static bool IsMiddleButtonPressed()
        {
            return _previousState.MiddleButton == ButtonState.Released && _currentState.MiddleButton == ButtonState.Pressed;
        }

        private static bool IsMiddleButtonReleased()
        {
            return _previousState.MiddleButton == ButtonState.Pressed && _currentState.MiddleButton == ButtonState.Released;
        }

        internal static bool IsRightButtonDown()
        {
            return _currentState.RightButton == ButtonState.Pressed;
        }

        private static bool IsRightButtonPressed()
        {
            return _previousState.RightButton == ButtonState.Released && _currentState.RightButton == ButtonState.Pressed;
        }

        private static bool IsRightButtonReleased()
        {
            return _previousState.RightButton == ButtonState.Pressed && _currentState.RightButton == ButtonState.Released;
        }

        private static bool MouseWheelUp()
        {
            return _currentState.ScrollWheelValue > _previousState.ScrollWheelValue;
        }

        private static bool MouseWheelDown()
        {
            return _currentState.ScrollWheelValue < _previousState.ScrollWheelValue;
        }

        private static bool HasMouseMoved()
        {
            return _previousState.Position != _currentState.Position;
        }

        private static void HandleMouse(Dictionary<string, Dictionary<string, MouseInputAction>> mouseEventHandlers, float deltaTime)
        {
            foreach (var item in mouseEventHandlers.Values)
            {
                foreach (var mouseInputAction in item.Values)
                {
                    var func = Switch[mouseInputAction.InputActionType];
                    var invoke = func.Invoke();

                    if (invoke)
                    {
                        mouseInputAction.Invoke(MousePosition, MouseMovement, deltaTime);
                    }
                }
            }
        }
    }
}