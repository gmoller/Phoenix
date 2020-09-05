using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Input
{
    public class InputHandler
    {
        #region State
        private Dictionary<string, Dictionary<string, KeyboardInputAction>> _keyboardEventHandlers;
        private Dictionary<string, Dictionary<string, MouseInputAction>> _mouseEventHandlers;
        #endregion End State

        public Point MousePosition => MouseHandler.MousePosition;
        public bool IsRightMouseButtonDown => MouseHandler.IsRightButtonDown();
        public bool IsLeftMouseButtonReleased => MouseHandler.IsLeftButtonReleased();

        public bool MouseIsWithinScreen => MousePosition.X >= 0.0f &&
                                           MousePosition.X <= 1920.0f &&
                                           MousePosition.Y >= 0.0f &&
                                           MousePosition.Y <= 1080.0f;

        public void Initialize()
        {
            KeyboardHandler.Initialize();
            MouseHandler.Initialize();

            _keyboardEventHandlers = new Dictionary<string, Dictionary<string, KeyboardInputAction>>();
            _mouseEventHandlers = new Dictionary<string, Dictionary<string, MouseInputAction>>();
        }

        public void AddCommandHandler(string source, int id, KeyboardInputAction keyboardInputAction)
        {
            var firstKey = keyboardInputAction.DictionaryKey;
            var secondKey = $"{source}.{id}";
            if (!_keyboardEventHandlers.ContainsKey(firstKey))
            {
                _keyboardEventHandlers.Add(firstKey, new Dictionary<string, KeyboardInputAction>());
            }

            _keyboardEventHandlers[firstKey].Add(secondKey, keyboardInputAction);
        }

        public void AddCommandHandler(string source, int id, MouseInputAction mouseInputAction)
        {
            var firstKey = mouseInputAction.DictionaryKey;
            var secondKey = $"{source}.{id}";
            if (!_mouseEventHandlers.ContainsKey(firstKey))
            {
                _mouseEventHandlers.Add(firstKey, new Dictionary<string, MouseInputAction>());
            }

            _mouseEventHandlers[firstKey].Add(secondKey, mouseInputAction);
        }

        public void RemoveCommandHandler(string source, int id, KeyboardInputAction keyboardInputAction)
        {
            var firstKey = keyboardInputAction.DictionaryKey;
            var secondKey = $"{source}.{id}";
            var eventHandlers = _keyboardEventHandlers[firstKey];
            eventHandlers.Remove(secondKey);
        }

        public void RemoveCommandHandler(string source, int id, MouseInputAction mouseInputAction)
        {
            var firstKey = mouseInputAction.DictionaryKey;
            var secondKey = $"{source}.{id}";
            var eventHandlers = _mouseEventHandlers[firstKey];
            eventHandlers.Remove(secondKey);
        }

        public void Update(float deltaTime)
        {
            //if (!MouseIsWithinScreen) return;

            KeyboardHandler.Update(_keyboardEventHandlers, deltaTime);
            MouseHandler.Update(_mouseEventHandlers, deltaTime);
        }
    }
}