using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using GameLogic;
using HexLibrary;
using PhoenixGameLibrary;
using Utilities;

namespace PhoenixGamePresentationLibrary
{
    internal class OverlandMapView
    {
        private readonly OverlandMap _overlandMap;

        internal OverlandMapView(OverlandMap overlandMap)
        {
            _overlandMap = overlandMap;
        }

        internal void Update(InputHandler input)
        {
            if (input.IsRightMouseButtonReleased)
            {
                var hex = DeviceManager.Instance.WorldHex;
                var cell = _overlandMap.CellGrid.GetCell(hex.X, hex.Y);
                if (cell.HasSettlement)
                {
                    Globals.Instance.MessageQueue.Enqueue("OpenSettlement"); // TODO: send through settlementId
                }
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            DrawCellGrid(spriteBatch, _overlandMap.CellGrid);
        }

        private void DrawCellGrid(SpriteBatch spriteBatch, CellGrid cellGrid)
        {
            var camera = Globals.Instance.World.Camera;

            var center = camera.ScreenToWorld(new Vector2(DeviceManager.Instance.GraphicsDevice.Viewport.Width / 2, DeviceManager.Instance.GraphicsDevice.Viewport.Height / 2));
            var centerHex = HexOffsetCoordinates.OffsetCoordinatesFromPixel((int)center.X, (int)center.Y);

            var columnsToLeft = 10; // TODO: remove hardcoding, use size of hex and cater for zoom (how many hexes fit on the screen)
            var columnsToRight = 10;
            var rowsUp = 10;
            var rowsDown = 10;

            var fromColumn = centerHex.Col - columnsToLeft;
            if (fromColumn < 0) fromColumn = 0;
            var toColumn = centerHex.Col + columnsToRight;
            if (toColumn > PhoenixGameLibrary.Constants.WORLD_MAP_COLUMNS) toColumn = PhoenixGameLibrary.Constants.WORLD_MAP_COLUMNS;

            var fromRow = centerHex.Row - rowsUp;
            if (fromRow < 0) fromRow = 0;
            var toRow = centerHex.Row + rowsDown;
            if (toRow > PhoenixGameLibrary.Constants.WORLD_MAP_ROWS) toRow = PhoenixGameLibrary.Constants.WORLD_MAP_ROWS;

            for (int r = fromRow; r < toRow; ++r)
            {
                for (int q = fromColumn; q < toColumn; ++q)
                {
                    var cell = cellGrid.GetCell(q, r);
                    if (cell.BelongsToSettlement >= 0) // TODO: only display if IsCityView
                    {
                        DrawCell(spriteBatch, cell, Color.LightSlateGray);
                    }
                    else
                    {
                        DrawCell(spriteBatch, cell, Color.White);
                    }
                    if (cell.HasSettlement)
                    {
                        DrawSettlement(spriteBatch, cell);
                    }
                    DrawHexBorder(cell);
                }
            }
        }

        private void DrawCell(SpriteBatch spriteBatch, Cell cell, Color color)
        {
            var centerPosition = HexOffsetCoordinates.OffsetCoordinatesToPixel(cell.Column, cell.Row);

            var terrainType = Globals.Instance.TerrainTypes[cell.TerrainTypeId];
            var texture = AssetsManager.Instance.GetTexture(cell.Texture.TexturePalette);
            var spec = AssetsManager.Instance.GetAtlas(cell.Texture.TexturePalette);
            var frame = spec.Frames[cell.Texture.TextureId];
            var sourceRectangle = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);

            var destinationRectangle = new Rectangle((int)centerPosition.X, (int)centerPosition.Y, 111, 192);
            var layerDepth = cell.Index / 10000.0f;
            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color, 0.0f, PhoenixGameLibrary.Constants.HEX_ORIGIN, SpriteEffects.None, layerDepth);
        }

        private void DrawSettlement(SpriteBatch spriteBatch, Cell cell)
        {
            var texture = AssetsManager.Instance.GetTexture("VillageSmall00");
            var position = HexOffsetCoordinates.OffsetCoordinatesToPixel(cell.Column, cell.Row);
            var destinationRectangle = new Rectangle((int)position.X, (int)position.Y, (int)(HexLibrary.Constants.HEX_ACTUAL_WIDTH * 0.5f), (int)(HexLibrary.Constants.HEX_ACTUAL_HEIGHT * 0.75f));
            var sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            var layerDepth = cell.Index / 10000.0f + 0.00001f;
            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White, 0.0f, PhoenixGameLibrary.Constants.HEX_ORIGIN, SpriteEffects.None, layerDepth);
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

            spriteBatch.DrawLine(centerPosition + point0, centerPosition + point1, color, 1.0f);
            spriteBatch.DrawLine(centerPosition + point1, centerPosition + point2, color, 1.0f);
            spriteBatch.DrawLine(centerPosition + point2, centerPosition + point3, color, 1.0f);
            spriteBatch.DrawLine(centerPosition + point3, centerPosition + point4, color, 1.0f);
            spriteBatch.DrawLine(centerPosition + point4, centerPosition + point5, color, 1.0f);
            spriteBatch.DrawLine(centerPosition + point5, centerPosition + point0, color, 1.0f);
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