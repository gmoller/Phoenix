using Microsoft.Xna.Framework;
using Zen.Input;

namespace PhoenixGamePresentation.Events
{
    internal static class MoveCameraEvent
    {
        internal static void HandleEvent(object sender, MouseEventArgs e)
        {
            var camera = (Camera)sender;

            var panCameraDistance = e.Mouse.IsMouseIsAtTopOfScreen() ? new Vector2(0.0f, -1.0f) * e.DeltaTime : Vector2.Zero;
            panCameraDistance += e.Mouse.MouseIsAtBottomOfScreen() ? new Vector2(0.0f, 1.0f) * e.DeltaTime : Vector2.Zero;
            panCameraDistance += e.Mouse.MouseIsAtLeftOfScreen() ? new Vector2(-1.0f, 0.0f) * e.DeltaTime : Vector2.Zero;
            panCameraDistance += e.Mouse.MouseIsAtRightOfScreen() ? new Vector2(1.0f, 0.0f) * e.DeltaTime : Vector2.Zero;

            camera.MoveCamera(panCameraDistance);

            // TODO: adjust speed depending on zoom level
        }
    }
}