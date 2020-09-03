using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Utilities.ExtensionMethods;

namespace Input
{
    internal static class MouseHandler
    {
        private static MouseState _currentState;
        private static MouseState _previousState;

        private static Func<bool>[] _mouseActions;

        internal static bool MouseActions(int index) => _mouseActions[index].Invoke();

        internal static void Initialize()
        {
            _currentState = Mouse.GetState();

            var size = Convert.ToInt32(new InputAction().Max());
            _mouseActions = new Func<bool>[size + 1];
            _mouseActions[(int)InputAction.LeftMouseButtonDown] = IsLeftButtonDown;
            _mouseActions[(int)InputAction.LeftMouseButtonPressed] = IsLeftButtonPressed;
            _mouseActions[(int)InputAction.LeftMouseButtonReleased] = IsLeftButtonReleased;

            _mouseActions[(int)InputAction.MiddleMouseButtonDown] = IsMiddleButtonDown;
            _mouseActions[(int)InputAction.MiddleMouseButtonPressed] = IsMiddleButtonPressed;
            _mouseActions[(int)InputAction.MiddleMouseButtonReleased] = IsMiddleButtonReleased;

            _mouseActions[(int)InputAction.RightMouseButtonDown] = IsRightButtonDown;
            _mouseActions[(int)InputAction.RightMouseButtonPressed] = IsRightButtonPressed;
            _mouseActions[(int)InputAction.RightMouseButtonReleased] = IsRightButtonReleased;
        }

        internal static void Update()
        {
            _previousState = _currentState;
            _currentState = Mouse.GetState();
        }

        internal static Point MousePosition => _currentState.Position;

        internal static Point MouseMovement => _currentState.Position - _previousState.Position;

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

        internal static bool MouseWheelUp()
        {
            return _currentState.ScrollWheelValue > _previousState.ScrollWheelValue;
        }

        internal static bool MouseWheelDown()
        {
            return _currentState.ScrollWheelValue < _previousState.ScrollWheelValue;
        }

        internal static bool HasMouseMoved()
        {
            return _previousState.Position != _currentState.Position;
        }
    }
}