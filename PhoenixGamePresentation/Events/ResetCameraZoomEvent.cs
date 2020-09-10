using Input;

namespace PhoenixGamePresentation.Events
{
    internal static class ResetCameraZoomEvent
    {
        internal static void HandleEvent(object sender, KeyboardEventArgs e)
        {
            var camera = (Camera)sender;

            camera.Zoom = 1.0f;
        }
    }
}