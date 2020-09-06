using System.Collections.Generic;
using Input;
using PhoenixGamePresentation.Views;
using Utilities;

namespace PhoenixGamePresentation.Events
{
    internal static class ResetPotentialMovementPathEvent
    {
        internal static void HandleEvent(object sender, MouseEventArgs e)
        {
            var stackView = (StackView)sender;

            if (!stackView.IsSelected) return;

            stackView.PotentialMovementPath = new List<PointI>();
        }
    }
}