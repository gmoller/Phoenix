using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using HexLibrary;
using PhoenixGameLibrary.GameData;
using Utilities;

namespace PhoenixGameLibrary
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public struct Cell
    {
        private readonly int _index;
        private readonly int _terrainTypeId;
        private readonly GameData.Texture _texture;

        public int Column => _index % Constants.WORLD_MAP_COLUMNS;
        public int Row => _index / Constants.WORLD_MAP_COLUMNS;

        public Cell(int col, int row, TerrainType terrainType, Camera camera)
        {
            _index = (row * Constants.WORLD_MAP_COLUMNS) + col;
            _terrainTypeId = terrainType.Id;
            _texture = terrainType.PossibleTextures[RandomNumberGenerator.Instance.GetRandomInt(0, 3)];
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
        }

        public void Draw(Camera camera, float layerDepth, TerrainTypes terrainTypes)
        {
            var centerPosition = HexOffsetCoordinates.OffsetCoordinatesToPixel(Column, Row);
            //centerPosition -= new Vector2(camera.Width * 0.5f, camera.Height * 0.5f);

            if (camera.VisibleArea.Contains(centerPosition))
            {
                var spriteBatch = DeviceManager.Instance.GetCurrentSpriteBatch();

                var terrainType = terrainTypes[_terrainTypeId];
                var texture = AssetsManager.Instance.GetTexture(_texture.TexturePalette);
                var spec = AssetsManager.Instance.GetAtlas(_texture.TexturePalette);
                var frame = spec.Frames[_texture.TextureId];
                var sourceRectangle = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);

                //spriteBatch.Draw(texture, centerPosition, sourceRectangle, Color.White, 0.0f, Constants.HEX_ORIGIN, Constants.HEX_SCALE, SpriteEffects.None, layerDepth);

                var destinationRectangle = new Rectangle((int)centerPosition.X, (int)centerPosition.Y, 111, 192);
                spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White, 0.0f, Constants.HEX_ORIGIN, SpriteEffects.None, layerDepth);
            }
        }

        public void DrawHexBorder(Camera camera)
        {
            var centerPosition = HexOffsetCoordinates.OffsetCoordinatesToPixel(Column, Row);
            //centerPosition -= new Vector2(camera.Width * 0.5f, camera.Height * 0.5f);

            if (camera.VisibleArea.Contains(centerPosition))
            {
                var spriteBatch = DeviceManager.Instance.GetCurrentSpriteBatch();

                var color = Color.PeachPuff;
                var point0 = GetHexCorner(5);
                var point1 = GetHexCorner(0);
                var point2 = GetHexCorner(1);
                var point3 = GetHexCorner(2);
                var point4 = GetHexCorner(3);
                var point5 = GetHexCorner(4);

                spriteBatch.DrawLine(centerPosition + point0, centerPosition + point1, color);
                spriteBatch.DrawLine(centerPosition + point1, centerPosition + point2, color);
                spriteBatch.DrawLine(centerPosition + point2, centerPosition + point3, color);
                spriteBatch.DrawLine(centerPosition + point3, centerPosition + point4, color);
                spriteBatch.DrawLine(centerPosition + point4, centerPosition + point5, color);
                spriteBatch.DrawLine(centerPosition + point5, centerPosition + point0, color);
            }
        }

        private Vector2 GetHexCorner(int i)
        {
            float degrees = 60 * i - 30;
            float radians = MathHelper.ToRadians(degrees);

            var v = new Vector2((float)(HexLibrary.Constants.HEX_SIZE * Math.Cos(radians)), (float)(HexLibrary.Constants.HEX_SIZE * Math.Sin(radians)));

            return v;
        }

        private string DebuggerDisplay => $"{{Col={Column},Row={Row},TerrainTypeId={_terrainTypeId}}}";
    }
}