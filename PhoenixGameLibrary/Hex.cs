using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using Utilities;

namespace PhoenixGameLibrary
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public struct Hex
    {
        private readonly int _colQ;
        private readonly int _rowR;
        private readonly int _frameId;
        private readonly float _layerDepth;

        private readonly Vector2 _centerPosition;

        public Hex(int colQ, int rowR, int frameId, float layerDepth, Camera camera)
        {
            _colQ = colQ;
            _rowR = rowR;
            _frameId = frameId;
            _layerDepth = layerDepth;

            _centerPosition = new Vector2();
            _centerPosition = CalculateWorldPosition(colQ, rowR) - new Vector2(camera.Width * 0.5f, camera.Height * 0.5f);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture, AtlasSpec2 spec, Camera camera)
        {
            if (camera.VisibleArea.X - Constants.HEX_WIDTH < _centerPosition.X && 
                camera.VisibleArea.X + camera.VisibleArea.Width + Constants.HEX_WIDTH > _centerPosition.X &&
                camera.VisibleArea.Y - Constants.HEX_HEIGHT < _centerPosition.Y &&
                camera.VisibleArea.Y + camera.VisibleArea.Height + Constants.HEX_HEIGHT > _centerPosition.Y)
            {
                var frame = spec.Frames[_frameId];
                var sourceRectangle = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
                spriteBatch.Draw(texture, _centerPosition, sourceRectangle, Color.White, 0.0f, Constants.HEX_ORIGIN, Constants.HEX_SCALE, SpriteEffects.None, _layerDepth);
            }
        }

        public void DrawHexBorder(SpriteBatch spriteBatch, Camera camera)
        {
            var color = Color.PeachPuff;
            var centerPosition = CalculateWorldPosition(_colQ, _rowR) - new Vector2(camera.Width * 0.5f, camera.Height * 0.5f);
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

        private string DebuggerDisplay
        {
            get { return $"{_colQ},{_rowR}"; }
        }
    }
}