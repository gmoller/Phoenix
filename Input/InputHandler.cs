using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Input
{
    public class InputHandler
    {
        #region State
        public event EventHandler<EventArgs> RightMouseButtonReleased;
        public event EventHandler<EventArgs> EnterKeyReleased;
        public event EventHandler<EventArgs> CKeyReleased;
        #endregion

        public Point MousePosition => MouseHandler.MousePosition;
        public Point MouseMovement => MouseHandler.MouseMovement;
        public bool MouseWheelUp => MouseHandler.MouseWheelUp();
        public bool MouseWheelDown => MouseHandler.MouseWheelDown();
        public bool IsLeftMouseButtonDown => MouseHandler.IsLeftButtonDown();
        public bool IsMiddleMouseButtonDown => MouseHandler.IsLeftButtonDown();
        public bool IsRightMouseButtonDown => MouseHandler.IsRightButtonDown();
        public bool IsLeftMouseButtonReleased => MouseHandler.IsLeftButtonReleased();
        public bool IsMiddleMouseButtonReleased => MouseHandler.IsLeftButtonReleased();
        public bool IsRightMouseButtonReleased => MouseHandler.IsRightButtonReleased();

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
        }

        public void Update(float deltaTime)
        {
            KeyboardHandler.Update();
            MouseHandler.Update();

            if (IsRightMouseButtonReleased)
            {
                OnRightMouseButtonReleased(new EventArgs());
            }

            if (IsKeyReleased(Keys.Enter))
            {
                OnEnterKeyReleased(new EventArgs());
            }

            if (IsKeyReleased(Keys.C))
            {
                OnCKeyReleased(new EventArgs());
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

        private void OnRightMouseButtonReleased(EventArgs e)
        {
            RightMouseButtonReleased?.Invoke(this, e);
        }

        private void OnEnterKeyReleased(EventArgs e)
        {
            EnterKeyReleased?.Invoke(this, e);
        }

        private void OnCKeyReleased(EventArgs e)
        {
            CKeyReleased?.Invoke(this, e);
        }
    }
}