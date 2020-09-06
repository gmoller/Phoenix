using Input;
using PhoenixGamePresentation.Views;

namespace PhoenixGamePresentation.Events
{
    internal static class CheckIfMouseIsOverHudViewEvent
    {
        internal static void HandleEvent(object sender, MouseEventArgs e)
        {
            var hudView = (HudView)sender;

            // ReSharper disable once PossiblyImpureMethodCallOnReadonlyVariable
            var mouseOverHudView = hudView.AreaContains(e.Mouse.Location) || hudView.HudViewFrame.ChildControls["btnEndTurn"].MouseOver;
            hudView.HudViewFrame.Enabled = mouseOverHudView;

            if (hudView.WorldView.GameStatus != GameStatus.CityView)
            {
                hudView.WorldView.GameStatus = mouseOverHudView ? GameStatus.InHudView : GameStatus.OverlandMap;
            }
        }
    }
}