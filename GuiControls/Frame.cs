using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using Utilities;

namespace GuiControls
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Frame : Control
    {
        private readonly int _topPadding;
        private readonly int _bottomPadding;
        private readonly int _leftPadding;
        private readonly int _rightPadding;

        private readonly DynamicSlots _slots;

        private Rectangle[] _sourcePatches;
        private Rectangle[] _destinationPatches;

        public Frame(
            Vector2 position,
            Alignment positionAlignment,
            Vector2 size,
            string textureAtlas,
            string textureName,
            IControl parent = null,
            string name = "") :
            this(
                position,
                positionAlignment,
                size,
                textureAtlas,
                textureName,
                0,
                0,
                0,
                0,
                null,
                0.0f,
                parent)
        {
        }

        public Frame(
            Vector2 position, 
            Alignment positionAlignment, 
            Vector2 size, 
            string textureAtlas, 
            string textureName, 
            int topPadding, 
            int bottomPadding, 
            int leftPadding, 
            int rightPadding, 
            DynamicSlots slots = null, 
            float layerDepth = 0.0f, 
            IControl parent = null, 
            string name = "") :
            base(
                position, 
                positionAlignment, 
                size, 
                textureAtlas, 
                textureName, 
                null, 
                null, 
                null, 
                null, 
                layerDepth, 
                parent,
                name)
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

        protected override void InDraw(SpriteBatch spriteBatch)
        {
            for (var i = 0; i < _sourcePatches.Length; ++i)
            {
                spriteBatch.Draw(Texture, _destinationPatches[i], _sourcePatches[i], Color, 0.0f, Vector2.Zero, SpriteEffects.None, LayerDepth);
            }

            _slots?.Draw(spriteBatch);
        }

        protected override void InDraw(Matrix? transform = null)
        {
            var spriteBatch = DeviceManager.Instance.GetNewSpriteBatch();
            spriteBatch.Begin(transformMatrix: transform);

            for (var i = 0; i < _sourcePatches.Length; ++i)
            {
                spriteBatch.Draw(Texture, _destinationPatches[i], _sourcePatches[i], Color, 0.0f, Vector2.Zero, SpriteEffects.None, LayerDepth);
            }

            _slots?.Draw(spriteBatch);

            spriteBatch.End();
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
                new Rectangle(leftX,  y,        middleWidth,  topPadding),           // top middle
                new Rectangle(rightX, y,        rightPadding, topPadding),      // top right
                new Rectangle(x,      topY,     leftPadding,  middleHeight),          // left middle
                new Rectangle(leftX,  topY,     middleWidth,  middleHeight),              // middle
                new Rectangle(rightX, topY,     rightPadding, middleHeight),         // right middle
                new Rectangle(x,      bottomY,  leftPadding,  bottomPadding),  // bottom left
                new Rectangle(leftX,  bottomY,  middleWidth,  bottomPadding),       // bottom middle
                new Rectangle(rightX, bottomY,  rightPadding, bottomPadding)   // bottom right
            };

            return patches;
        }
    }
}