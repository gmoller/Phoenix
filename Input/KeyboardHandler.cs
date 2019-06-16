using Microsoft.Xna.Framework.Input;

namespace Input
{
    public static class KeyboardHandler
    {
        private static KeyboardState _currentState;
        private static KeyboardState _previousState;

        public static void Initialize()
        {
            _currentState = Keyboard.GetState();
        }

        public static void Update()
        {
            _previousState = _currentState;
            _currentState = Keyboard.GetState();
        }

        public static bool IsKeyDown(Keys key)
        {
            return _currentState.IsKeyDown(key);
        }

        public static bool IsKeyUp(Keys key)
        {
            return _currentState.IsKeyUp(key);
        }

        public static bool IsKeyPressed(Keys key)
        {
            return _previousState.IsKeyUp(key) && _currentState.IsKeyDown(key);
        }

        public static bool IsKeyReleased(Keys key)
        {
            return _previousState.IsKeyDown(key) && _currentState.IsKeyUp(key);
        }
    }
}