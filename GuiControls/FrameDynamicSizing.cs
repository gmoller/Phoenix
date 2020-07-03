using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using Utilities;

namespace GuiControls
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

        private readonly DynamicSlots _slots;

        private Texture2D _texture;
        private Rectangle[] _sourcePatches;
        private Rectangle[] _destinationPatches;

        public Vector2 TopLeftPosition { get; private set; }

        public FrameDynamicSizing(Vector2 topLeftPosition, Vector2 size, string textureAtlasString, string textureString, int topPadding, int bottomPadding, int leftPadding, int rightPadding, DynamicSlots slots = null)
        {
            TopLeftPosition = topLeftPosition;
            _size = size;
            _textureAtlasString = textureAtlasString;
            _textureString = textureString;

            _topPadding = topPadding;
            _bottomPadding = bottomPadding;
            _leftPadding = leftPadding;
            _rightPadding = rightPadding;

            _slots = slots;
        }

        public void LoadContent(ContentManager content)
        {
            _texture = AssetsManager.Instance.GetTexture(_textureAtlasString);
            var atlas = AssetsManager.Instance.GetAtlas(_textureAtlasString);

            var frame = atlas.Frames[_textureString];
            _sourcePatches = CreatePatches(frame.ToRectangle(), _topPadding, _bottomPadding, _leftPadding, _rightPadding);
            _destinationPatches = CreatePatches(new Rectangle((int)TopLeftPosition.X, (int)TopLeftPosition.Y, (int)_size.X, (int)_size.Y), _topPadding, _bottomPadding, _leftPadding, _rightPadding);
        }

        public void Draw()
        {
            var spriteBatch = DeviceManager.Instance.GetNewSpriteBatch();
            spriteBatch.Begin();

            for (var i = 0; i < _sourcePatches.Length; ++i)
            {
                spriteBatch.Draw(_texture, _destinationPatches[i], _sourcePatches[i], Color.White);
            }

            _slots?.Draw(spriteBatch);

            spriteBatch.End();
            DeviceManager.Instance.ReturnSpriteBatchToPool(spriteBatch);
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