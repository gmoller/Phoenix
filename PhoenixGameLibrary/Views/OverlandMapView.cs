using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using GameLogic;
using HexLibrary;
using Utilities;

namespace PhoenixGameLibrary.Views
{
    public class OverlandMapView
    {
        private OverlandMap _overlandMap;

        public bool IsEnabled { get; set; }

        public OverlandMapView(OverlandMap overlandMap)
        {
            _overlandMap = overlandMap;
        }

        public void LoadContent(ContentManager content)
        {
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
        }

        public void Draw()
        {
            DrawCellGrid(_overlandMap.CellGrid);
        }

        private void DrawCellGrid(CellGrid cellGrid)
        {
            var spriteBatch = DeviceManager.Instance.GetCurrentSpriteBatch();

            var camera = Globals.Instance.World.Camera;

            for (int r = 0; r < cellGrid.NumberOfRows; ++r)
            {
                for (int q = 0; q < cellGrid.NumberOfColumns; ++q)
                {
                    var cell = cellGrid.GetCell(q, r);

                    var centerPosition = HexOffsetCoordinates.OffsetCoordinatesToPixel(cell.Column, cell.Row);
                    if (camera.VisibleArea.Contains(centerPosition))
                    {
                        DrawCell(cell);
                    }
                }
            }

            //spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, null, null, null, null, camera.Transform);

            //for (int r = 0; r < cellGrid.NumberOfRows; ++r)
            //{
            //    for (int q = 0; q < cellGrid.NumberOfColumns; ++q)
            //    {
            //        var cell = cellGrid.GetCell(q, r);
            //        DrawHexBorder(cell);
            //    }
            //}

            //spriteBatch.End();
        }

        private void DrawCell(Cell cell)
        {
            var centerPosition = HexOffsetCoordinates.OffsetCoordinatesToPixel(cell.Column, cell.Row);

            var spriteBatch = DeviceManager.Instance.GetCurrentSpriteBatch();

            var terrainType = Globals.Instance.TerrainTypes[cell.TerrainTypeId];
            var texture = AssetsManager.Instance.GetTexture(cell.Texture.TexturePalette);
            var spec = AssetsManager.Instance.GetAtlas(cell.Texture.TexturePalette);
            var frame = spec.Frames[cell.Texture.TextureId];
            var sourceRectangle = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);

            //spriteBatch.Draw(texture, centerPosition, sourceRectangle, Color.White, 0.0f, Constants.HEX_ORIGIN, Constants.HEX_SCALE, SpriteEffects.None, layerDepth);

            var destinationRectangle = new Rectangle((int)centerPosition.X, (int)centerPosition.Y, 111, 192);
            float layerDepth = cell.Index / 10000.0f;
            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White, 0.0f, Constants.HEX_ORIGIN, SpriteEffects.None, layerDepth);
        }

        private void DrawHexBorder(Cell cell)
        {
            var centerPosition = HexOffsetCoordinates.OffsetCoordinatesToPixel(cell.Column, cell.Row);

            var spriteBatch = DeviceManager.Instance.GetCurrentSpriteBatch();

            var color = Color.PeachPuff;
            var point0 = GetHexCorner(5);
            var point1 = GetHexCorner(0);
            var point2 = GetHexCorner(1);
            var point3 = GetHexCorner(2);
            var point4 = GetHexCorner(3);
            var point5 = GetHexCorner(4);

            spriteBatch.DrawLine(centerPosition + point0, centerPosition + point1, color, 0.0f);
            spriteBatch.DrawLine(centerPosition + point1, centerPosition + point2, color, 0.0f);
            spriteBatch.DrawLine(centerPosition + point2, centerPosition + point3, color, 0.0f);
            spriteBatch.DrawLine(centerPosition + point3, centerPosition + point4, color, 0.0f);
            spriteBatch.DrawLine(centerPosition + point4, centerPosition + point5, color, 0.0f);
            spriteBatch.DrawLine(centerPosition + point5, centerPosition + point0, color, 0.0f);
        }

        private Vector2 GetHexCorner(int i)
        {
            float degrees = 60 * i - 30;
            float radians = MathHelper.ToRadians(degrees);

            var v = new Vector2((float)(HexLibrary.Constants.HEX_SIZE * Math.Cos(radians)), (float)(HexLibrary.Constants.HEX_SIZE * Math.Sin(radians)));

            return v;
        }
    }
}