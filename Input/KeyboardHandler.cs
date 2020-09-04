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

        /// <summary>
        /// Is the key currently not being pressed?
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <returns>True if key is currently not being pressed.</returns>
        internal static bool IsKeyUp(Keys key)
        {
            return _currentState.IsKeyUp(key);
        }

        /// <summary>
        /// Is the key currently being pressed?
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <returns>True if key is currently being pressed.</returns>
        internal static bool IsKeyDown(Keys key)
        {
            return _currentState.IsKeyDown(key);
        }

        /// <summary>
        /// Has the key just been pressed?
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <returns>True if key has just been pressed (between this frame and last frame).</returns>
        internal static bool IsKeyPressed(Keys key)
        {
            return _previousState.IsKeyUp(key) && _currentState.IsKeyDown(key);
        }

        /// <summary>
        /// Has the key just been released?
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <returns>True if key has just been released (between this frame and last frame).</returns>
        internal static bool IsKeyReleased(Keys key)
        {
            return _previousState.IsKeyDown(key) && _currentState.IsKeyUp(key);
        }
    }
}