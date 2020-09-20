using Microsoft.Xna.Framework.Graphics;

namespace PhoenixGamePresentation.Views.StackView
{
    internal class StackViewPatrollingState : StackViewState
    {
        internal StackViewPatrollingState(StackView stackView)
        {
            StackView = stackView;
        }

        internal override void Update(WorldView worldView, float deltaTime)
        {
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