using Input;
using Microsoft.Xna.Framework;

namespace PhoenixGamePresentation.Events
{
    internal static class MoveCameraEvent
    {
        internal static void HandleEvent(object sender, MouseEventArgs e)
        {
            var camera = (Camera)sender;

            if (camera.WorldView.GameStatus != GameStatus.OverlandMap) return;

            var panCameraDistance = IsMouseIsAtTopOfScreen(e.Mouse.Location) ? new Vector2(0.0f, -1.0f) * e.DeltaTime : Vector2.Zero;
            panCameraDistance += MouseIsAtBottomOfScreen(e.Mouse.Location) ? new Vector2(0.0f, 1.0f) * e.DeltaTime : Vector2.Zero;
            panCameraDistance += MouseIsAtLeftOfScreen(e.Mouse.Location) ? new Vector2(-1.0f, 0.0f) * e.DeltaTime : Vector2.Zero;
            panCameraDistance += MouseIsAtRightOfScreen(e.Mouse.Location) ? new Vector2(1.0f, 0.0f) * e.DeltaTime : Vector2.Zero;

            camera.MoveCamera(panCameraDistance);

            // TODO: adjust speed depending on zoom level
        }

        private static bool IsMouseIsAtTopOfScreen(Point mousePosition)
        {
            return mousePosition.Y < 20.0f && mousePosition.Y >= 0.0f;
        }

        private static bool MouseIsAtBottomOfScreen(Point mousePosition)
        {
            return mousePosition.Y > 1080 - 20.0f && mousePosition.Y <= 1080.0f;
        }

        private static bool MouseIsAtLeftOfScreen(Point mousePosition)
        {
            return mousePosition.X < 20.0f && mousePosition.X >= 0.0f; ;
        }

        private static bool MouseIsAtRightOfScreen(Point mousePosition)
        {
            return mousePosition.X > 1670.0f - 20.0f && mousePosition.X <= 1670.0f;
        }
    }
}