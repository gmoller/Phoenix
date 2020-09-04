using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Input
{
    public class InputHandler
    {
        #region State
        private Dictionary<string, Dictionary<string, KeyboardInputAction>> _keyboardEventHandlers;
        private Dictionary<string, Dictionary<string, MouseInputAction>> _mouseEventHandlers;
        #endregion

        public Point MousePosition => MouseHandler.MousePosition;
        public Point MouseMovement => MouseHandler.MouseMovement;
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
            KeyboardHandler.Update();
            MouseHandler.Update();

            if (!MouseIsWithinScreen) return;

            HandleKeyboard(_keyboardEventHandlers);
            HandleMouse(_mouseEventHandlers, deltaTime);
        }

        private void HandleKeyboard(Dictionary<string, Dictionary<string, KeyboardInputAction>> keyboardEventHandlers)
        {
            //TODO: replace switch with dictionary
            foreach (var item in keyboardEventHandlers.Values)
            {
                foreach (var keyboardInputAction in item.Values)
                {
                    var invoke = false;
                    switch (keyboardInputAction.InputActionType)
                    {
                        case KeyboardInputActionType.Up:
                            if (KeyboardHandler.IsKeyUp(keyboardInputAction.Key))
                            {
                                invoke = true;
                            }
                            break;
                        case KeyboardInputActionType.Down:
                            if (KeyboardHandler.IsKeyDown(keyboardInputAction.Key))
                            {
                                invoke = true;
                            }
                            break;
                        case KeyboardInputActionType.Pressed:
                            if (KeyboardHandler.IsKeyPressed(keyboardInputAction.Key))
                            {
                                invoke = true;
                            }
                            break;
                        case KeyboardInputActionType.Released:
                            if (KeyboardHandler.IsKeyReleased(keyboardInputAction.Key))
                            {
                                invoke = true;
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    if (invoke)
                    {
                        keyboardInputAction.Invoke();
                    }
                }
            }
        }

        private void HandleMouse(Dictionary<string, Dictionary<string, MouseInputAction>> mouseEventHandlers, float deltaTime)
        {
            //TODO: replace switch with dictionary
            foreach (var item in mouseEventHandlers.Values)
            {
                foreach (var mouseInputAction in item.Values)
                {
                    var invoke = false;
                    switch (mouseInputAction.InputActionType)
                    {
                        case MouseInputActionType.Moved:
                            switch (mouseInputAction.MouseWidget)
                            {
                                case MouseButtons.None:
                                    invoke = true;
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                            break;
                        case MouseInputActionType.ButtonDown:
                            switch (mouseInputAction.MouseWidget)
                            {
                                case MouseButtons.LeftButton:
                                    if (MouseHandler.IsLeftButtonDown())
                                    {
                                        invoke = true;
                                    }
                                    break;
                                case MouseButtons.MiddleButton:
                                    if (MouseHandler.IsMiddleButtonDown())
                                    {
                                        invoke = true;
                                    }
                                    break;
                                case MouseButtons.RightButton:
                                    if (MouseHandler.IsRightButtonDown())
                                    {
                                        invoke = true;
                                    }
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                            break;
                        case MouseInputActionType.ButtonPressed:
                            switch (mouseInputAction.MouseWidget)
                            {
                                case MouseButtons.LeftButton:
                                    if (MouseHandler.IsLeftButtonPressed())
                                    {
                                        invoke = true;
                                    }
                                    break;
                                case MouseButtons.MiddleButton:
                                    if (MouseHandler.IsMiddleButtonPressed())
                                    {
                                        invoke = true;
                                    }
                                    break;
                                case MouseButtons.RightButton:
                                    if (MouseHandler.IsRightButtonPressed())
                                    {
                                        invoke = true;
                                    }
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                            break;
                        case MouseInputActionType.ButtonReleased:
                            switch (mouseInputAction.MouseWidget)
                            {
                                case MouseButtons.LeftButton:
                                    if (MouseHandler.IsLeftButtonReleased())
                                    {
                                        invoke = true;
                                    }
                                    break;
                                case MouseButtons.MiddleButton:
                                    if (MouseHandler.IsMiddleButtonReleased())
                                    {
                                        invoke = true;
                                    }
                                    break;
                                case MouseButtons.RightButton:
                                    if (MouseHandler.IsRightButtonReleased())
                                    {
                                        invoke = true;
                                    }
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                            break;
                        case MouseInputActionType.WheelUp:
                            switch (mouseInputAction.MouseWidget)
                            {
                                case MouseButtons.Wheel:
                                    if (MouseHandler.MouseWheelUp())
                                    {
                                        invoke = true;
                                    }
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                            break;
                        case MouseInputActionType.WheelDown:
                            switch (mouseInputAction.MouseWidget)
                            {
                                case MouseButtons.Wheel:
                                    if (MouseHandler.MouseWheelDown())
                                    {
                                        invoke = true;
                                    }
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                            break;
                        case MouseInputActionType.ButtonDrag:
                            switch (mouseInputAction.MouseWidget)
                            {
                                case MouseButtons.LeftButton:
                                    if (MouseHandler.IsLeftButtonDown() && MouseHandler.HasMouseMoved())
                                    {
                                        invoke = true;
                                    }
                                    break;
                                case MouseButtons.MiddleButton:
                                    if (MouseHandler.IsMiddleButtonDown() && MouseHandler.HasMouseMoved())
                                    {
                                        invoke = true;
                                    }
                                    break;
                                case MouseButtons.RightButton:
                                    if (MouseHandler.IsRightButtonDown() && MouseHandler.HasMouseMoved())
                                    {
                                        invoke = true;
                                    }
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    if (invoke)
                    {
                        mouseInputAction.Invoke(this, deltaTime);
                    }
                }
            }
        }
    }
}