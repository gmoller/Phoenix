using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using Utilities;

namespace PhoenixGamePresentationLibrary
{
    public class FrameDynamicSizing
    {
        private readonly string _textureAtlasString;
        private readonly string _textureString;
        private readonly Vector2 _size;
        private readonly int _topPadding;
        private readonly int _bottomPadding;
        private readonly int _leftPadding;
        private readonly int _rightPadding;

        private Texture2D _texture;
        private Rectangle[] _sourcePatches;
        private Rectangle[] _destinationPatches;

        private readonly int _numberOfSlotsX;
        private readonly int _numberOfSlotsY;
        private Rectangle _slot;

        public Vector2 TopLeftPosition { get; private set; }

        public FrameDynamicSizing(Vector2 topLeftPosition, Vector2 size, string textureAtlasString, string textureString, int topPadding, int bottomPadding, int leftPadding, int rightPadding, int numberOfSlotsX = 0, int numberOfSlotsY = 0)
        {
            TopLeftPosition = topLeftPosition;
            _size = size;
            _textureAtlasString = textureAtlasString;
            _textureString = textureString;

            _topPadding = topPadding;
            _bottomPadding = bottomPadding;
            _leftPadding = leftPadding;
            _rightPadding = rightPadding;

            _numberOfSlotsX = numberOfSlotsX;
            _numberOfSlotsY = numberOfSlotsY;
        }

        internal void LoadContent(ContentManager content)
        {
            _texture = AssetsManager.Instance.GetTexture(_textureAtlasString);
            var atlas = AssetsManager.Instance.GetAtlas(_textureAtlasString);

            var frame = atlas.Frames[_textureString];
            _sourcePatches = CreatePatches(frame.ToRectangle(), _topPadding, _bottomPadding, _leftPadding, _rightPadding);
            _destinationPatches = CreatePatches(new Rectangle((int)TopLeftPosition.X, (int)TopLeftPosition.Y, (int)_size.X, (int)_size.Y), _topPadding, _bottomPadding, _leftPadding, _rightPadding);

            frame = atlas.Frames["slot"];
            _slot = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);

            TopLeftPosition += new Vector2(6.0f, 6.0f); // TODO: de-hardcode
        }

        public void Draw()
        {
            var spriteBatch = DeviceManager.Instance.GetNewSpriteBatch();
            spriteBatch.Begin();

            for (var i = 0; i < _sourcePatches.Length; ++i)
            {
                spriteBatch.Draw(_texture, _destinationPatches[i], _sourcePatches[i], Color.White);
            }

            DrawSlots(spriteBatch);

            spriteBatch.End();
            DeviceManager.Instance.ReturnSpriteBatchToPool(spriteBatch);
        }

        private void DrawSlots(SpriteBatch spriteBatch)
        {
            float x = _destinationPatches[0].X + 10.0f;
            float y = _destinationPatches[0].Y + 10.0f;

            for (int j = 0; j < _numberOfSlotsY; ++j)
            {
                for (int i = 0; i < _numberOfSlotsX; ++i)
                {
                    var rect = new Rectangle((int)x, (int)y, 49, 25);
                    spriteBatch.Draw(_texture, rect, _slot, Color.White);
                    x += 49.0f; // _slot.Width + 0.0f;
                }

                x = _destinationPatches[0].X + 10.0f;
                y += 25.0f;
            }
        }

        private Rectangle[] CreatePatches(Rectangle rectangle, int topPadding, int bottomPadding, int leftPadding, int rightPadding)
        {
            var x = rectangle.X;
            var y = rectangle.Y;
            var w = rectangle.Width;
            var h = rectangle.Height;
            var middleWidth = w - leftPadding - rightPadding;
            var middleHeight = h - topPadding - bottomPadding;
            var bottomY = y + h - bottomPadding;
            var rightX = x + w - rightPadding;
            var leftX = x + leftPadding;
            var topY = y + topPadding;
            var patches = new[]
            {
                new Rectangle(x,      y,        leftPadding,  topPadding),      // top left
                new Rectangle(leftX,  y,        middleWidth,  topPadding),      // top middle
                new Rectangle(rightX, y,        rightPadding, topPadding),      // top right
                new Rectangle(x,      topY,     leftPadding,  middleHeight),    // left middle
                new Rectangle(leftX,  topY,     middleWidth,  middleHeight),    // middle
                new Rectangle(rightX, topY,     rightPadding, middleHeight),    // right middle
                new Rectangle(x,      bottomY,  leftPadding,  bottomPadding),   // bottom left
                new Rectangle(leftX,  bottomY,  middleWidth,  bottomPadding),   // bottom middle
                new Rectangle(rightX, bottomY,  rightPadding, bottomPadding)    // bottom right
            };

            return patches;
        }
    }
}