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
            var thisStacksLocationHex = StackView.Stack.LocationHex;

            var drawUnit = false;
            var currentlySelectedStack = StackView.StackViews.Current;
            if (currentlySelectedStack != null) // if another stack is selected
            {
                var currentlySelectedStacksLocationHex = currentlySelectedStack.LocationHex;
                if (currentlySelectedStacksLocationHex == thisStacksLocationHex) // and it's in the same hex as this one
                {
                    if (currentlySelectedStack.IsMovingState) // and selected stack is moving
                    {
                        drawUnit = true;
                    }
                    else
                    {
                        // don't draw if there's a selected stack on same location and it's not moving
                    }
                }
                else
                {
                    drawUnit = true;
                }
            }
            else
            {
                drawUnit = true;
            }

            if (drawUnit)
            {
                var locationInWorld = camera.WorldHexToWorldPixel(thisStacksLocationHex);
                DrawUnit(spriteBatch, locationInWorld);
            }
        }
    }
}