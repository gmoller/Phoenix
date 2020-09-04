﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Input
{
    internal static class MouseHandler
    {
        #region State
        private static MouseState _currentState;
        private static MouseState _previousState;
        #endregion End State

        internal static void Initialize()
        {
            _currentState = Mouse.GetState();
        }

        internal static void Update()
        {
            _previousState = _currentState;
            _currentState = Mouse.GetState();
        }

        internal static Point MousePosition => _currentState.Position;

        internal static Point MouseMovement => _currentState.Position - _previousState.Position;

        internal static bool IsLeftButtonDown()
        {
            return _currentState.LeftButton == ButtonState.Pressed;
        }

        internal static bool IsLeftButtonPressed()
        {
            return _previousState.LeftButton == ButtonState.Released && _currentState.LeftButton == ButtonState.Pressed;
        }

        internal static bool IsLeftButtonReleased()
        {
            return _previousState.LeftButton == ButtonState.Pressed && _currentState.LeftButton == ButtonState.Released;
        }

        internal static bool IsMiddleButtonDown()
        {
            return _currentState.MiddleButton == ButtonState.Pressed;
        }

        internal static bool IsMiddleButtonPressed()
        {
            return _previousState.MiddleButton == ButtonState.Released && _currentState.MiddleButton == ButtonState.Pressed;
        }

        internal static bool IsMiddleButtonReleased()
        {
            return _previousState.MiddleButton == ButtonState.Pressed && _currentState.MiddleButton == ButtonState.Released;
        }

        internal static bool IsRightButtonDown()
        {
            return _currentState.RightButton == ButtonState.Pressed;
        }

        internal static bool IsRightButtonPressed()
        {
            return _previousState.RightButton == ButtonState.Released && _currentState.RightButton == ButtonState.Pressed;
        }

        internal static bool IsRightButtonReleased()
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