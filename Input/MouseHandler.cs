using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Input
{
    public static class MouseHandler
    {
        private static MouseState _currentState;
        private static MouseState _previousState;

        public static void Initialize()
        {
            _currentState = Mouse.GetState();
        }

        public static void Update()
        {
            _previousState = _currentState;
            _currentState = Mouse.GetState();
        }

        public static Point MousePosition => _currentState. Position;

        public static bool IsLeftButtonPressed()
        {
            return _previousState.LeftButton == ButtonState.Released && _currentState.LeftButton == ButtonState.Pressed;
        }

        public static bool IsLeftButtonReleased()
        {
            return _previousState.LeftButton == ButtonState.Pressed && _currentState.LeftButton == ButtonState.Released;
        }

        public static bool IsRightButtonPressed()
        {
            return _previousState.RightButton == ButtonState.Released && _currentState.RightButton == ButtonState.Pressed;
        }

        public static bool IsRightButtonReleased()
        {
            return _previousState.RightButton == ButtonState.Pressed && _currentState.RightButton == ButtonState.Released;
        }
    }
}