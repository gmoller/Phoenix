using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace PhoenixGamePresentation.Views.StackView
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    internal class StackViewNormalState : StackViewState
    {
        internal StackViewNormalState(StackView stackView)
        {
            StackView = stackView;
            StackView.SetNotCurrent();
            //StackView.SetStatusToNone();
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

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => "Normal";
    }
}