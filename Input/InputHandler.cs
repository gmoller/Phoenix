using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Input
{
    public class InputHandler
    {
        #region State
        private readonly KeyboardHandler _keyboard;
        public MouseHandler Mouse { get; }
        private readonly Dictionary<string, Dictionary<string, KeyboardInputAction>> _keyboardEventHandlers;
        private readonly Dictionary<string, Dictionary<string, MouseInputAction>> _mouseEventHandlers;
        #endregion End State

        public Point MousePosition => Mouse.Location;
        public bool IsLeftMouseButtonReleased => Mouse.IsLeftButtonReleased();

        internal bool MouseIsWithinScreen => Mouse.MouseIsWithinScreen;

        public InputHandler()
        {
            _keyboard = new KeyboardHandler();
            Mouse = new MouseHandler();

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

            _keyboard.Update(_keyboardEventHandlers, deltaTime);
            Mouse.Update(_mouseEventHandlers, deltaTime);
        }
    }
}