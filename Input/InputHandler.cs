using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Utilities;

namespace Input
{
    public class InputHandler
    {
        #region State
        private Dictionary<int, Dictionary<string, Action<object, EventArgs>>> _eventHandlers;
        private InputAction[] _mouseInputActionsToCheck;
        private InputAction[] _keyboardInputActionsToCheck;
        #endregion

        public Point MousePosition => MouseHandler.MousePosition;
        public Point MouseMovement => MouseHandler.MouseMovement;
        public bool MouseWheelUp => MouseHandler.MouseWheelUp();
        public bool MouseWheelDown => MouseHandler.MouseWheelDown();
        public bool IsRightMouseButtonDown => MouseHandler.IsRightButtonDown();
        public bool IsLeftMouseButtonReleased => MouseHandler.IsLeftButtonReleased();

        public bool HasMouseMoved => MouseHandler.HasMouseMoved();
        public bool MouseIsAtTopOfScreen => MousePosition.Y < 20.0f && MousePosition.Y >= 0.0f;
        public bool MouseIsAtBottomOfScreen => MousePosition.Y > (1080 - 20.0f) && MousePosition.Y <= 1080.0f;
        public bool MouseIsAtLeftOfScreen => MousePosition.X < 20.0f && MousePosition.X >= 0.0f;
        public bool MouseIsAtRightOfScreen => MousePosition.X > (1670.0f - 20.0f) && MousePosition.X <= 1670.0f;

        public bool MouseIsWithinScreen => MousePosition.X >= 0.0f &&
                                           MousePosition.X <= 1920.0f &&
                                           MousePosition.Y >= 0.0f &&
                                           MousePosition.Y <= 1080.0f;

        public bool AreAnyNumPadKeysDown =>
            KeyboardHandler.IsKeyDown(Keys.NumPad4) ||
            KeyboardHandler.IsKeyDown(Keys.NumPad6) ||
            KeyboardHandler.IsKeyDown(Keys.NumPad7) ||
            KeyboardHandler.IsKeyDown(Keys.NumPad9) ||
            KeyboardHandler.IsKeyDown(Keys.NumPad1) ||
            KeyboardHandler.IsKeyDown(Keys.NumPad3);

        public void Initialize()
        {
            KeyboardHandler.Initialize();
            MouseHandler.Initialize();

            _eventHandlers = new Dictionary<int, Dictionary<string, Action<object, EventArgs>>>();

            _mouseInputActionsToCheck = new[]
            {
                InputAction.LeftMouseButtonDown,
                InputAction.LeftMouseButtonPressed,
                InputAction.LeftMouseButtonReleased,
                InputAction.RightMouseButtonDown,
                InputAction.RightMouseButtonPressed,
                InputAction.RightMouseButtonReleased
            };

            _keyboardInputActionsToCheck = new[]
            {
                InputAction.KeyEnterReleased,
                InputAction.KeyCReleased
            };
        }

        public void AddCommandHandler(string source, int id, InputAction inputAction, Action<object, EventArgs> commandHandler)
        {
            var firstKey = (int)inputAction;
            var secondKey = $"{source}.{id}";
            if (_eventHandlers.ContainsKey(firstKey))
            {
                _eventHandlers[firstKey].Add(secondKey, commandHandler);
            }
            else
            {
                _eventHandlers.Add(firstKey, new Dictionary<string, Action<object, EventArgs>>());
                _eventHandlers[firstKey].Add(secondKey, commandHandler);
            }
        }

        public void RemoveCommandHandler(string source, int id, InputAction inputAction)
        {
            var firstKey = (int)inputAction;
            var secondKey = $"{source}.{id}";
            var eventHandlers = _eventHandlers[firstKey];
            eventHandlers.Remove(secondKey);
        }

        public void Update(float deltaTime)
        {
            KeyboardHandler.Update();
            MouseHandler.Update();

            if (!MouseIsWithinScreen) return;

            foreach (var inputActionToCheck in _mouseInputActionsToCheck)
            {
                var index = (int)inputActionToCheck;
                if (MouseHandler.MouseActions(index))
                {
                    if (_eventHandlers.ContainsKey(index))
                    {
                        var eventHandlers = _eventHandlers[index];
                        foreach (var eventHandler in eventHandlers.Values)
                        {
                            eventHandler.Invoke(this, new MouseEventArgs(new PointI(MousePosition.X, MousePosition.Y)));
                        }
                    }
                }
            }

            foreach (var inputActionToCheck in _keyboardInputActionsToCheck)
            {
                var index = (int)inputActionToCheck;
                if (KeyboardHandler.KeyboardActions(index))
                {
                    if (_eventHandlers.ContainsKey(index))
                    {
                        var eventHandlers = _eventHandlers[index];
                        foreach (var eventHandler in eventHandlers.Values)
                        {
                            eventHandler.Invoke(this, new KeyboardEventArgs(Keys.C));
                        }
                    }
                }
            }
        }

        /// <summary>
        ///  Is the key currently not being pressed?
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsKeyUp(Keys key)
        {
            return KeyboardHandler.IsKeyUp(key);
        }

        /// <summary>
        /// Is the key currently being pressed?
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsKeyDown(Keys key)
        {
            return KeyboardHandler.IsKeyDown(key);
        }

        /// <summary>
        /// Has the key just been pressed?
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsKeyPressed(Keys key)
        {
            return KeyboardHandler.IsKeyPressed(key);
        }

        /// <summary>
        /// Has the key just been released?
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsKeyReleased(Keys key)
        {
            return KeyboardHandler.IsKeyReleased(key);
        }
    }
}