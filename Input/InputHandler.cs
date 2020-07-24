using Microsoft.Xna.Framework.Input;
using Utilities;

namespace Input
{
    public class InputHandler
    {
        public Microsoft.Xna.Framework.Point MousePosition => MouseHandler.MousePosition;
        public Microsoft.Xna.Framework.Point MouseMovement => MouseHandler.MouseMovement;
        public bool MouseWheelUp => MouseHandler.MouseWheelUp();
        public bool MouseWheelDown => MouseHandler.MouseWheelDown();
        public bool IsLeftMouseButtonDown => MouseHandler.IsLeftButtonDown();
        public bool IsLeftMouseButtonReleased => MouseHandler.IsLeftButtonReleased();
        public bool IsRightMouseButtonReleased => MouseHandler.IsRightButtonReleased();

        public bool HasMouseMoved => MouseHandler.HasMouseMoved();
        public bool MouseIsAtTopOfScreen => MouseHandler.MousePosition.Y < DeviceManager.Instance.MapViewport.Y + 5.0f;
        public bool MouseIsAtBottomOfScreen => MouseHandler.MousePosition.Y > DeviceManager.Instance.MapViewport.Height - 5.0f;
        public bool MouseIsAtLeftOfScreen => MouseHandler.MousePosition.X < DeviceManager.Instance.MapViewport.X + 5.0f;
        public bool MouseIsAtRightOfScreen => MouseHandler.MousePosition.X > DeviceManager.Instance.MapViewport.Width - 5.0f;

        public bool AreAnyNumPadKeysDown =>
            KeyboardHandler.IsKeyDown(Keys.NumPad4) ||
            KeyboardHandler.IsKeyDown(Keys.NumPad6) ||
            KeyboardHandler.IsKeyDown(Keys.NumPad7) ||
            KeyboardHandler.IsKeyDown(Keys.NumPad9) ||
            KeyboardHandler.IsKeyDown(Keys.NumPad1) ||
            KeyboardHandler.IsKeyDown(Keys.NumPad3);

        public bool Eaten { get; set; }

        public void Initialize()
        {
            KeyboardHandler.Initialize();
            MouseHandler.Initialize();
        }

        public void Update(float deltaTime)
        {
            KeyboardHandler.Update();
            MouseHandler.Update();
        }

        public bool IsKeyDown(Keys key)
        {
            return KeyboardHandler.IsKeyDown(key);
        }
    }
}