using Zen.Input;

namespace PhoenixGamePresentation.Events
{
    internal static class DragCameraEvent
    {
        internal static void HandleEvent(object sender, MouseEventArgs e)
        {
            var camera = (Camera)sender;

            var panCameraDistance = e.Mouse.Movement.ToVector2();

            camera.MoveCamera(panCameraDistance);

            // TODO: adjust speed depending on zoom level
        }
    }
}