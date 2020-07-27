using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using AssetsLibrary;
using Input;
using Microsoft.Xna.Framework.Graphics;

namespace GuiControls
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Frame : ControlBase
    {
        private readonly int _topPadding;
        private readonly int _bottomPadding;
        private readonly int _leftPadding;
        private readonly int _rightPadding;

        private readonly DynamicSlots _slots;

        private Rectangle[] _sourcePatches;
        private Rectangle[] _destinationPatches;

        public Frame(string name, Vector2 position, ContentAlignment alignment, Vector2 size, string textureAtlas, string textureName, byte? textureId, int topPadding, int bottomPadding, int leftPadding, int rightPadding, DynamicSlots slots = null, float layerDepth = 0.0f, IControl parent = null) :
            base(name, position, alignment, size, textureAtlas, textureName, textureId, layerDepth, parent)
        {
            _topPadding = topPadding;
            _bottomPadding = bottomPadding;
            _leftPadding = leftPadding;
            _rightPadding = rightPadding;

            _slots = slots;
        }

        public override void LoadContent(ContentManager content)
        {
            Texture = AssetsManager.Instance.GetTexture(TextureAtlas);
            var atlas = AssetsManager.Instance.GetAtlas(TextureAtlas);

            var frame = atlas.Frames[TextureName];
            _sourcePatches = CreatePatches(frame.ToRectangle(), _topPadding, _bottomPadding, _leftPadding, _rightPadding);
            _destinationPatches = CreatePatches(new Rectangle(TopLeft.X, TopLeft.Y, (int)Size.X, (int)Size.Y), _topPadding, _bottomPadding, _leftPadding, _rightPadding);
        }

        public override void Update(InputHandler input, float deltaTime, Matrix? transform = null)
        {
        }

        public override void Draw(Matrix? transform = null)
        {
            var spriteBatch = BeginSpriteBatch(transform);

            for (var i = 0; i < _sourcePatches.Length; ++i)
            {
                spriteBatch.Draw(Texture, _destinationPatches[i], _sourcePatches[i], Color, 0.0f, Vector2.Zero, SpriteEffects.None, LayerDepth);
            }

            _slots?.Draw(spriteBatch);

            EndSpriteBatch(spriteBatch);
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