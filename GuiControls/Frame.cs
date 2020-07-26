using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using Utilities;
using Input;

namespace GuiControls
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Frame : IControl
    {
        private readonly IControl _parent; // TODO: move into base/interface?

        private readonly string _textureAtlasString;
        private readonly string _textureString;
        private readonly int _topPadding;
        private readonly int _bottomPadding;
        private readonly int _leftPadding;
        private readonly int _rightPadding;

        private readonly DynamicSlots _slots;

        private Texture2D _texture;
        private Rectangle[] _sourcePatches;
        private Rectangle[] _destinationPatches;

        private Rectangle _area;

        public string Name { get; }
        public int Top => _area.Top;
        public int Bottom => _area.Bottom;
        public int Left => _area.Left;
        public int Right => _area.Right;
        public int Width => _area.Width;
        public int Height => _area.Height;
        public Microsoft.Xna.Framework.Point Center => _area.Center;
        public Microsoft.Xna.Framework.Point TopLeft => new Microsoft.Xna.Framework.Point(Left, Top);
        public Microsoft.Xna.Framework.Point TopRight => new Microsoft.Xna.Framework.Point(Right, Top);
        public Microsoft.Xna.Framework.Point BottomLeft => new Microsoft.Xna.Framework.Point(Left, Bottom);
        public Microsoft.Xna.Framework.Point BottomRight => new Microsoft.Xna.Framework.Point(Right, Bottom);
        public Microsoft.Xna.Framework.Point Size => _area.Size;

        public Vector2 RelativePosition => new Vector2(0 + TopLeft.X, 0 + TopLeft.Y);

        public Frame(string name, Vector2 topLeftPosition, Vector2 size, string textureAtlasString, string textureString, int topPadding, int bottomPadding, int leftPadding, int rightPadding, DynamicSlots slots = null, IControl parent = null)
        {
            _parent = parent;

            Name = name;
            _area = new Rectangle((int)topLeftPosition.X, (int)topLeftPosition.Y, (int)size.X, (int)size.Y);
            _textureAtlasString = textureAtlasString;
            _textureString = textureString;

            _topPadding = topPadding;
            _bottomPadding = bottomPadding;
            _leftPadding = leftPadding;
            _rightPadding = rightPadding;

            _slots = slots;
        }

        public void SetTopLeftPosition(int x, int y)
        {
            //_area.X = x;
            //_area.Y = y;
        }

        public void LoadContent(ContentManager content)
        {
            _texture = AssetsManager.Instance.GetTexture(_textureAtlasString);
            var atlas = AssetsManager.Instance.GetAtlas(_textureAtlasString);

            var frame = atlas.Frames[_textureString];
            _sourcePatches = CreatePatches(frame.ToRectangle(), _topPadding, _bottomPadding, _leftPadding, _rightPadding);
            _destinationPatches = CreatePatches(new Rectangle(TopLeft.X, TopLeft.Y, (int)Size.X, (int)Size.Y), _topPadding, _bottomPadding, _leftPadding, _rightPadding);
        }

        public void Update(InputHandler input, float deltaTime, Matrix? transform = null)
        {
        }

        public void Draw(Matrix? transform = null)
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

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Name={Name},TopLeftPosition={TopLeft},RelativePosition={RelativePosition},Size={Size}}}";
    }
}