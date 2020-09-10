using PhoenixGamePresentation.Views;

namespace PhoenixGamePresentation.Actions
{
    internal static class MoveStackToCellAction
    {
        internal static void DoAction(object sender, ActionArgs e)
        {
            var stackView = (StackView)sender;

            stackView.MoveStackToCell();
        }
    }
}