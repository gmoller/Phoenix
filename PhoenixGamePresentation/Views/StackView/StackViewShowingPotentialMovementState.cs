using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhoenixGamePresentation.Handlers;

namespace PhoenixGamePresentation.Views.StackView
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    internal class StackViewShowingPotentialMovementState : StackViewState
    {
        internal StackViewShowingPotentialMovementState(StackView stackView)
        {
            StackView = stackView;
        }

        internal override void Update(WorldView worldView, float deltaTime)
        {
            var path = PotentialMovementHandler.GetPotentialMovementPath(StackView.Stack, StackView.CellGrid, StackView.MousePosition, StackView.Camera);
            StackView.PotentialMovementPath = path;
        }

        internal override void DrawUnit(SpriteBatch spriteBatch, Camera camera)
        {
            var locationInWorld = camera.WorldHexToWorldPixel(StackView.Stack.LocationHex);
            DrawUnit(spriteBatch, locationInWorld);

            DrawMovementPath(spriteBatch, StackView.PotentialMovementPath, Color.White, 3.0f, 1.0f);
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => "ShowingPotentialMovementState";
    }
}