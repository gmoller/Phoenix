using System;
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
        private const float Scale = 1.0f;
        private const float SideLength = 16.0f * Scale;

        private readonly int _colQ;
        private readonly int _rowR;
        private readonly int _frameId;

        private Vector2 Position { get; } // center position of hex
        private Vector2[] Points { get; }

        public Hex(int colQ, int rowR, int frameId)
        {
            _colQ = colQ;
            _rowR = rowR;
            _frameId = frameId;

            var h = (float)Math.Sin(MathHelper.ToRadians(30)) * SideLength;
            var r = (float)Math.Cos(MathHelper.ToRadians(30)) * SideLength;

            Position = Vector2.Zero;
            Points = new Vector2[0];

            Position = CalculateWorldPosition(colQ, rowR, h, r);
            Points = CalculateVertices(Position, SideLength, h, r);
        }

        private Vector2 CalculateWorldPosition(int colQ, int rowR, float h, float r)
        {
            float hexWideWidth = SideLength + h + h;
            float hexNarrowWidth = (h * 3) + 0;
            float hexHeight = (r * 2) + 0;

            //var basePosition = new Vector2(16.0f, 24.0f);
            var basePosition = Vector2.Zero;
            float x = basePosition.X + (hexNarrowWidth * colQ);
            float y = basePosition.Y + (hexHeight * (colQ * 0.5f + rowR)) - (colQ / 2 * hexHeight);

            return new Vector2(x, y);
        }

        private Vector2[] CalculateVertices(Vector2 position, float sideLength, float h, float r)
        {
            var vertices = new Vector2[6];

            vertices[0] = new Vector2(-sideLength / 2.0f, -r);
            //vertices[0] = GetFlatHexCornerOffset(sideLength, 4);

            vertices[1] = new Vector2(sideLength / 2.0f, -r);
            //vertices[1] = GetFlatHexCornerOffset(sideLength, 5);

            vertices[2] = new Vector2(sideLength / 2.0f + h, 0.0f);
            //vertices[2] = GetFlatHexCornerOffset(sideLength, 0);

            vertices[3] = new Vector2(sideLength / 2.0f, r);
            //vertices[3] = GetFlatHexCornerOffset(sideLength, 1);

            vertices[4] = new Vector2(-sideLength / 2.0f, r);
            //vertices[4] = GetFlatHexCornerOffset(sideLength, 2);

            vertices[5] = new Vector2(-sideLength / 2.0f - h, 0.0f);
            //vertices[5] = GetFlatHexCornerOffset(sideLength, 3);

            return vertices;
        }

        //private Vector2 GetFlatHexCornerOffset(float sideLength, byte corner)
        //{
        //    var angleInDegrees = 60 * corner;
        //    var angleInRadians = MathHelper.ToRadians(angleInDegrees);
        //    var x = (float)(sideLength * Math.Cos(angleInRadians));
        //    var y = (float)(sideLength * Math.Sin(angleInRadians));

        //    return new Vector2(x, y);
        //}

        public void Draw(SpriteBatch spriteBatch, Texture2D texture, AtlasSpec2 spec)
        {
            var frame = spec.Frames[_frameId];
            var sourceRectangle = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
            spriteBatch.Draw(texture, Position, sourceRectangle, Color.White, 0.0f, new Vector2(16.0f, 24.0f + 10.0f), Scale, SpriteEffects.None, 0.0f); // why 10.0f???

            //var color = Color.PeachPuff;
            //spriteBatch.DrawLine(Position + Points[0], Position + Points[1], color);
            //spriteBatch.DrawLine(Position + Points[1], Position + Points[2], color);
            //spriteBatch.DrawLine(Position + Points[2], Position + Points[3], color);
            //spriteBatch.DrawLine(Position + Points[3], Position + Points[4], color);
            //spriteBatch.DrawLine(Position + Points[4], Position + Points[5], color);
            //spriteBatch.DrawLine(Position + Points[5], Position + Points[0], color);
        }

        private string DebuggerDisplay
        {
            get { return $"{_colQ},{_rowR}"; }
        }
    }
}