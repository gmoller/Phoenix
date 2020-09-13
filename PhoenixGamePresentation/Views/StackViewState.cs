using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Hex;
using MonoGameUtilities;
using MonoGameUtilities.ExtensionMethods;
using PhoenixGamePresentation.Views.StackView;
using Utilities;

namespace PhoenixGamePresentation.Views
{
    internal abstract class StackViewState
    {
        protected StackView.StackView StackView { get; set; }
        internal abstract (bool changeState, StackViewState stateToChangeTo) Update(StackViewUpdateActions updateActions, WorldView worldView, float deltaTime);
        internal abstract void DrawUnit(SpriteBatch spriteBatch, Camera camera);

        protected void DrawUnit(SpriteBatch spriteBatch, Vector2 location)
        {
            // draw background
            var destinationRectangle = new Rectangle((int)location.X, (int)location.Y, 60, 60);
            var sourceRectangle = StackView.StackViews.SquareGreenFrame.ToRectangle();
            spriteBatch.Draw(StackView.StackViews.GuiTextures, destinationRectangle, sourceRectangle, Color.White, 0.0f, new Vector2(sourceRectangle.Width * Constants.ONE_HALF, sourceRectangle.Height * 0.5f), SpriteEffects.None, 0.0f);

            // draw unit icon
            destinationRectangle = new Rectangle((int)location.X, (int)location.Y, 36, 32);
            var frame = StackView.StackViews.UnitAtlas.Frames[StackView.Stack[0].UnitTypeTextureName];
            sourceRectangle = frame.ToRectangle();
            spriteBatch.Draw(StackView.StackViews.UnitTextures, destinationRectangle, sourceRectangle, Color.White, 0.0f, new Vector2(sourceRectangle.Width * Constants.ONE_HALF, sourceRectangle.Height * 0.5f), SpriteEffects.None, 0.0f);
        }

        protected void DrawMovementPath(SpriteBatch spriteBatch, List<PointI> movementPath, Color color, float radius, float thickness)
        {
            foreach (var item in movementPath)
            {
                var centerPosition = HexOffsetCoordinates.ToPixel(item).ToVector2();
                spriteBatch.DrawCircle(centerPosition, radius, 10, color, thickness);
            }
        }
    }
}