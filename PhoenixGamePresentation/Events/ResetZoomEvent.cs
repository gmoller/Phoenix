using Input;

namespace PhoenixGamePresentation.Events
{
    internal static class ResetZoomEvent
    {
        internal static void HandleEvent(object sender, KeyboardEventArgs e)
        {
            var camera = (Camera)sender;

            if (camera.WorldView.GameStatus != GameStatus.OverlandMap) return;

            camera.Zoom = 1.0f;
        }
    }
}