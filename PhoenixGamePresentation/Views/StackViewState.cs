﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zen.Hexagons;
using Zen.MonoGameUtilities;
using Zen.Utilities;

namespace PhoenixGamePresentation.Views
{
    internal abstract class StackViewState
    {
        protected StackView.StackView StackView { get; set; }
        internal abstract void Update(WorldView worldView, float deltaTime);
        internal abstract void DrawUnit(SpriteBatch spriteBatch, Camera camera);

        protected void DrawUnitBackground(SpriteBatch spriteBatch)
        {
            var sourceRectangle = StackView.StackViews.SquareGreenFrame.ToRectangle();
            var destinationRectangle = StackView.WorldFrame;
            spriteBatch.Draw(StackView.StackViews.GuiTextures, destinationRectangle, sourceRectangle, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
        }

        protected void DrawUnitIcon(SpriteBatch spriteBatch, Vector2 location)
        {
            var frame = StackView.StackViews.UnitAtlas.Frames[StackView.Stack[0].UnitTypeTextureName];
            var sourceRectangle = frame.ToRectangle();
            var destinationRectangle = new Rectangle((int)location.X, (int)location.Y, sourceRectangle.Width, sourceRectangle.Height);
            spriteBatch.Draw(StackView.StackViews.UnitTextures, destinationRectangle, sourceRectangle, Color.White, 0.0f, new Vector2(sourceRectangle.Width * Constants.ONE_HALF, sourceRectangle.Height * 0.5f), SpriteEffects.None, 0.0f);
        }

        protected void DrawMovementPath(SpriteBatch spriteBatch, List<PointI> movementPath, Color color, float radius, float thickness)
        {
            foreach (var item in movementPath)
            {
                var pixel = StackView.HexLibrary.FromOffsetCoordinatesToPixel(new HexOffsetCoordinates(item.X, item.Y));
                var centerPosition = new Vector2(pixel.X, pixel.Y);
                spriteBatch.DrawCircle(centerPosition, radius, 10, color, thickness);
            }
        }
    }
}