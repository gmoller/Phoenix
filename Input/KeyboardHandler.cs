using Microsoft.Xna.Framework.Input;

namespace Input
{
    internal static class KeyboardHandler
    {
        private static KeyboardState _currentState;
        private static KeyboardState _previousState;

        internal static void Initialize()
        {
            _currentState = Keyboard.GetState();
        }

        internal static void Update()
        {
            _previousState = _currentState;
            _currentState = Keyboard.GetState();
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