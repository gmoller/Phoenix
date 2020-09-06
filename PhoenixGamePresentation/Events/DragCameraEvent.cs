using Input;

namespace PhoenixGamePresentation.Events
{
    internal static class DragCameraEvent
    {
        internal static void HandleEvent(object sender, MouseEventArgs e)
        {
            var camera = (Camera)sender;

            if (camera.WorldView.GameStatus != GameStatus.OverlandMap) return;

            var mouseEventArgs = (MouseEventArgs)e;

            var panCameraDistance = mouseEventArgs.Mouse.Movement.ToVector2();

            camera.MoveCamera(panCameraDistance);

            // TODO: adjust speed depending on zoom level
        }
    }
}