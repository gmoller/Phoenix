using System;
using Microsoft.Xna.Framework.Input;
using Utilities.ExtensionMethods;

namespace Input
{
    internal static class KeyboardHandler
    {
        private static KeyboardState _currentState;
        private static KeyboardState _previousState;

        private static Func<bool>[] _keyboardActions;

        internal static bool KeyboardActions(int index) => _keyboardActions[index].Invoke();

        internal static void Initialize()
        {
            _currentState = Keyboard.GetState();

            var size = Convert.ToInt32(new InputAction().Max());
            _keyboardActions = new Func<bool>[size + 1];
            _keyboardActions[(int)InputAction.KeyEnterReleased] = IsKeyEnterReleased;
            _keyboardActions[(int)InputAction.KeyCReleased] = IsKeyCReleased;
        }

        internal static void Update()
        {
            _previousState = _currentState;
            _currentState = Keyboard.GetState();
        }

        private static bool IsKeyEnterReleased()
        {
            return IsKeyReleased(Keys.Enter);
        }

        private static bool IsKeyCReleased()
        {
            return IsKeyReleased(Keys.C);
        }

        internal static bool IsKeyDown(Keys key)
        {
            return _currentState.IsKeyDown(key);
        }

        internal static bool IsKeyUp(Keys key)
        {
            return _currentState.IsKeyUp(key);
        }

        internal static bool IsKeyPressed(Keys key)
        {
            return _previousState.IsKeyUp(key) && _currentState.IsKeyDown(key);
        }

        internal static bool IsKeyReleased(Keys key)
        {
            return _previousState.IsKeyDown(key) && _currentState.IsKeyUp(key);
        }
    }
}