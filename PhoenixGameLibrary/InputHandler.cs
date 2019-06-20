using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Input;

namespace PhoenixGameLibrary
{
    public class InputHandler
    {
        public bool Exit { get; private set; }
        public bool CameraZoomIn { get; private set; }
        public bool CameraZoomOut { get; private set; }
        public Point PanCameraDistance { get; private set; }

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

            CameraZoomIn = MouseHandler.MouseWheelUp();
            CameraZoomOut = MouseHandler.MouseWheelDown();

            PanCameraDistance = MouseHandler.IsLeftButtonDown() && MouseHandler.HasMouseMoved() ? MouseHandler.MouseMovement : Point.Zero;
        }
    }
}