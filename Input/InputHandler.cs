using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

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

        public InputHandler()
        {
            _keyboard = new KeyboardHandler();
            Mouse = new MouseHandler();

            _keyboardEventHandlers = new Dictionary<string, Dictionary<string, KeyboardInputAction>>();
            _mouseEventHandlers = new Dictionary<string, Dictionary<string, MouseInputAction>>();
        }

        public Point MousePosition => Mouse.Location;
        public bool IsLeftMouseButtonReleased => Mouse.IsLeftButtonReleased();
        internal bool MouseIsWithinScreen => Mouse.MouseIsWithinScreen;

        public void Update(float deltaTime)
        {
            //if (!MouseIsWithinScreen) return;

            _keyboard.Update(_keyboardEventHandlers, deltaTime);
            Mouse.Update(_mouseEventHandlers, deltaTime);
        }

        // for testing
        public void SetMousePosition(Point pos)
        {
            Mouse.SetMousePosition(pos);
        }

        public void SubscribeToEventHandler(string source, int id, object sender, Keys key, KeyboardInputActionType inputActionType, Action<object, KeyboardEventArgs> action)
        {
            var keyboardInputAction = new KeyboardInputAction(sender, key, inputActionType, action);

            var firstKey = $"Keyboard.{key}.{inputActionType}";
            var secondKey = $"{source}.{id}";
            if (!_keyboardEventHandlers.ContainsKey(firstKey))
            {
                _keyboardEventHandlers.Add(firstKey, new Dictionary<string, KeyboardInputAction>());
            }

            _keyboardEventHandlers[firstKey].Add(secondKey, keyboardInputAction);
        }

        public void SubscribeToEventHandler(string source, int id, object sender, MouseInputActionType inputActionType, Action<object, MouseEventArgs> action)
        {
            var mouseInputAction = new MouseInputAction(sender, inputActionType, action);

            var firstKey = $"Mouse.{inputActionType}";
            var secondKey = $"{source}.{id}";
            if (!_mouseEventHandlers.ContainsKey(firstKey))
            {
                _mouseEventHandlers.Add(firstKey, new Dictionary<string, MouseInputAction>());
            }

            _mouseEventHandlers[firstKey].Add(secondKey, mouseInputAction);
        }

        public void UnsubscribeFromEventHandler(string source, int id, Keys key, KeyboardInputActionType inputActionType)
        {
            var firstKey = $"Keyboard.{key}.{inputActionType}";
            var secondKey = $"{source}.{id}";
            var eventHandlers = _keyboardEventHandlers[firstKey];
            eventHandlers.Remove(secondKey);
        }

        public void UnsubscribeFromEventHandler(string source, int id, MouseInputActionType inputActionType)
        {
            var firstKey = $"Mouse.{inputActionType}";
            var secondKey = $"{source}.{id}";
            var eventHandlers = _mouseEventHandlers[firstKey];
            eventHandlers.Remove(secondKey);
        }

        public void UnsubscribeAllFromEventHandler(string source)
        {
            foreach (var keyboardEventHandler in _keyboardEventHandlers)
            {
                //var eventHandlers = _keyboardEventHandlers[keyboardEventHandler.Key];
                foreach (var key in keyboardEventHandler.Value.Keys)
                {
                    if (key.StartsWith($"{source}."))
                    {
                        keyboardEventHandler.Value.Remove(key);
                    }
                }
            }

            foreach (var mouseEventHandler in _mouseEventHandlers)
            {
                //var eventHandlers = _mouseEventHandlers[mouseEventHandler.Key];
                foreach (var key in mouseEventHandler.Value.Keys)
                {
                    if (key.StartsWith($"{source}."))
                    {
                        mouseEventHandler.Value.Remove(key);
                    }
                }
            }
        }
    }
}