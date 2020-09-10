using PhoenixGamePresentation.Views;

namespace PhoenixGamePresentation.Conditions
{
    internal static class MoveStackToNextCellCondition
    {
        internal static bool EvaluateCondition(object sender, float deltaTime)
        {
            var stackView = (StackView)sender;

            var moveUnit = stackView.IsMovingState && stackView.MovementCountdownTime <= 0;

            return moveUnit;
        }
    }
}