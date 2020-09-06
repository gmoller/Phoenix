using Input;

namespace PhoenixGamePresentation.Events
{
    internal static class IncreaseCameraZoomEvent
    {
        internal static void HandleEvent(object sender, MouseEventArgs e)
        {
            var camera = (Camera)sender;

            if (camera.WorldView.GameStatus != GameStatus.OverlandMap) return;

            camera.Zoom += 0.05f;
        }
    }
}