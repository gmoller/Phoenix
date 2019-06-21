using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using Utilities;
using PhoenixGameLibrary.GameData;

namespace PhoenixGameLibrary
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public struct Hex
    {
        private readonly int _terrainTypeId;
        private readonly GameData.Texture _texture;
        private readonly Vector2 _centerPosition;

        public Hex(int colQ, int rowR, TerrainType terrainType, Camera camera)
        {
            _terrainTypeId = terrainType.Id;
            _texture = terrainType.PossibleTextures[RandomNumberGenerator.Instance.GetRandomInt(0, 3)];

            _centerPosition = new Vector2();
            _centerPosition = CalculateWorldPosition(colQ, rowR) - new Vector2(camera.Width * 0.5f, camera.Height * 0.5f);
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera, float layerDepth, TerrainTypes terrainTypes)
        {
            var rect = new Rectangle(
                camera.VisibleArea.X - (int)Constants.HEX_WIDTH, 
                camera.VisibleArea.Y - (int)Constants.HEX_HEIGHT, 
                camera.VisibleArea.Width + (int)Constants.HEX_WIDTH * 3, 
                camera.VisibleArea.Height + (int)Constants.HEX_HEIGHT * 2);

            if (rect.Contains(_centerPosition))
            {
                var terrainType = terrainTypes[_terrainTypeId];
                var texture = AssetsManager.Instance.GetTexture(_texture.TexturePalette);
                var spec = AssetsManager.Instance.GetAtlas(_texture.TexturePalette);
                var frame = spec.Frames[_texture.TextureId];
                var sourceRectangle = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
                spriteBatch.Draw(texture, _centerPosition, sourceRectangle, Color.White, 0.0f, Constants.HEX_ORIGIN, Constants.HEX_SCALE, SpriteEffects.None, layerDepth);
            }
        }

        public void DrawHexBorder(SpriteBatch spriteBatch, int colQ, int rowR, Camera camera)
        {
            var rect = new Rectangle(
                camera.VisibleArea.X - (int)Constants.HEX_WIDTH, 
                camera.VisibleArea.Y - (int)Constants.HEX_HEIGHT, 
                camera.VisibleArea.Width + (int)Constants.HEX_WIDTH * 2, 
                camera.VisibleArea.Height + (int)Constants.HEX_HEIGHT * 2);

            if (rect.Contains(_centerPosition))
            {
                var color = Color.PeachPuff;
                var centerPosition = CalculateWorldPosition(colQ, rowR) - new Vector2(camera.Width * 0.5f, camera.Height * 0.5f);
                var point0 = new Vector2(0.0f, 0.0f - Constants.HEX_HALF_HEIGHT);
                var point1 = new Vector2(0.0f + Constants.HEX_HALF_WIDTH, 0.0f - Constants.HEX_ONE_QUARTER_HEIGHT);
                var point2 = new Vector2(0.0f + Constants.HEX_HALF_WIDTH, 0.0f + Constants.HEX_ONE_QUARTER_HEIGHT);
                var point3 = new Vector2(0.0f, 0.0f + Constants.HEX_HALF_HEIGHT);
                var point4 = new Vector2(0.0f - Constants.HEX_HALF_WIDTH, 0.0f + Constants.HEX_ONE_QUARTER_HEIGHT);
                var point5 = new Vector2(0.0f - Constants.HEX_HALF_WIDTH, 0.0f - Constants.HEX_ONE_QUARTER_HEIGHT);

                spriteBatch.DrawLine(centerPosition + point0, centerPosition + point1, color);
                spriteBatch.DrawLine(centerPosition + point1, centerPosition + point2, color);
                spriteBatch.DrawLine(centerPosition + point2, centerPosition + point3, color);
                spriteBatch.DrawLine(centerPosition + point3, centerPosition + point4, color);
                spriteBatch.DrawLine(centerPosition + point4, centerPosition + point5, color);
                spriteBatch.DrawLine(centerPosition + point5, centerPosition + point0, color);
            }
        }

        private Vector2 CalculateWorldPosition(int colQ, int rowR)
        {
            // odd-r horizontal layout
            float x;
            if (rowR % 2 == 0)
            {
                x = Constants.HEX_WIDTH * colQ;
            }
            else
            {
                x = Constants.HEX_WIDTH * colQ + Constants.HEX_HALF_WIDTH;
            }
            float y = Constants.HEX_THREE_QUARTER_HEIGHT * rowR;

            var position = new Vector2(x, y);

            return position;
        }

        private string DebuggerDisplay => $"{{TerrainTypeId={_terrainTypeId},CenterPosition={_centerPosition}}}";
    }
}