using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Input
{
    internal static class KeyboardHandler
    {
        #region State
        private static KeyboardState _currentState;
        private static KeyboardState _previousState;
        private static readonly Dictionary<KeyboardInputActionType, Func<Keys, bool>> Switch = new Dictionary<KeyboardInputActionType, Func<Keys, bool>>
        {
            { KeyboardInputActionType.Up, IsKeyUp },
            { KeyboardInputActionType.Down, IsKeyDown },
            { KeyboardInputActionType.Pressed, IsKeyPressed },
            { KeyboardInputActionType.Released, IsKeyReleased }
        };
        #endregion End State

        internal static void Initialize()
        {
            _currentState = Keyboard.GetState();
        }

        internal static void Update(Dictionary<string, Dictionary<string, KeyboardInputAction>> keyboardEventHandlers, float deltaTime)
        {
            _previousState = _currentState;
            _currentState = Keyboard.GetState();

            HandleKeyboard(keyboardEventHandlers, deltaTime);
        }

        /// <summary>
        /// Is the key currently not being pressed?
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <returns>True if key is currently not being pressed.</returns>
        private static bool IsKeyUp(Keys key)
        {
            return _currentState.IsKeyUp(key);
        }

        /// <summary>
        /// Is the key currently being pressed?
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <returns>True if key is currently being pressed.</returns>
        private static bool IsKeyDown(Keys key)
        {
            return _currentState.IsKeyDown(key);
        }

        /// <summary>
        /// Has the key just been pressed?
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <returns>True if key has just been pressed (between this frame and last frame).</returns>
        private static bool IsKeyPressed(Keys key)
        {
            return _previousState.IsKeyUp(key) && _currentState.IsKeyDown(key);
        }

        /// <summary>
        /// Has the key just been released?
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <returns>True if key has just been released (between this frame and last frame).</returns>
        private static bool IsKeyReleased(Keys key)
        {
            return _previousState.IsKeyDown(key) && _currentState.IsKeyUp(key);
        }

        private static void HandleKeyboard(Dictionary<string, Dictionary<string, KeyboardInputAction>> keyboardEventHandlers, float deltaTime)
        {
            foreach (var item in keyboardEventHandlers.Values)
            {
                foreach (var keyboardInputAction in item.Values)
                {
                    var func = Switch[keyboardInputAction.InputActionType];
                    var invoke = func.Invoke(keyboardInputAction.Key);

                    if (invoke)
                    {
                        keyboardInputAction.Invoke(deltaTime);
                    }
                }
            }
        }
    }
}