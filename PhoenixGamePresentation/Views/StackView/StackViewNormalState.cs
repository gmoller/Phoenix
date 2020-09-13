using Microsoft.Xna.Framework.Graphics;

namespace PhoenixGamePresentation.Views.StackView
{
    internal class StackViewNormalState : StackViewState
    {
        internal StackViewNormalState(StackView stackView)
        {
            StackView = stackView;
            StackView.StackViews.SetNotCurrent(StackView);
        }

        internal override (bool changeState, StackViewState stateToChangeTo) Update(StackViewUpdateActions updateActions, WorldView worldView, float deltaTime)
        {
            if (updateActions.HasFlag(StackViewUpdateActions.SelectStackDirect))
            {
                return (true, new StackViewSelectedState(StackView));
            }

            return (false, null);
        }

        internal override void DrawUnit(SpriteBatch spriteBatch, Camera camera)
        {
            var location = camera.WorldHexToWorldPixel(StackView.Stack.LocationHex);

            DrawUnit(spriteBatch, location);
        }
    }
}