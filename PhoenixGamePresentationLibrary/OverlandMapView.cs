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
        private OverlandMap _overlandMap;

        internal OverlandMapView(OverlandMap overlandMap)
        {
            _overlandMap = overlandMap;
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
                    //DrawHexBorder(cell);
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
            float layerDepth = cell.Index / 10000.0f;
            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color, 0.0f, PhoenixGameLibrary.Constants.HEX_ORIGIN, SpriteEffects.None, layerDepth);
        }
    }
}