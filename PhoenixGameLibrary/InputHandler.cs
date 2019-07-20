using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Input;
using Utilities;

namespace PhoenixGameLibrary
{
    public class InputHandler
    {
        public Point MousePostion => MouseHandler.MousePosition;
        public Point MouseMovement => MouseHandler.MouseMovement;
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
        public bool Exit { get; private set; }

        public void Initialize()
        {
            KeyboardHandler.Initialize();
            MouseHandler.Initialize();
        }

        public void Update(GameTime gameTime)
        {
            KeyboardHandler.Update();
            MouseHandler.Update();

            if (KeyboardHandler.IsKeyDown(Keys.Escape))
            {
                Exit = true;
            }
        }
    }
}