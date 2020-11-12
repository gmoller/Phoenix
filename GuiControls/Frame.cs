using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Utilities;

namespace GuiControls
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Frame : ControlWithSingleTexture
    {
        #region State
        private readonly int _topPadding;
        private readonly int _bottomPadding;
        private readonly int _leftPadding;
        private readonly int _rightPadding;

        private Rectangle[] _sourcePatches;
        private Rectangle[] _destinationPatches;
        #endregion

        /// <summary>
        /// Use this constructor if Frame is to be used as a child of another control.
        /// When a control is a child of another control, it's position will be relative
        /// to the parent control. Therefore there is no need to pass in a position.
        /// </summary>
        /// <param name="name">Name of frame control.</param>
        /// <param name="size">Size of frame control in pixels.</param>
        /// <param name="textureName">Texture to use for rendering. If texture is from a texture atlas use 'AtlasName.TextureName'</param>
        public Frame(
            string name,
            Vector2 size,
            string textureName) :
            this(
                Vector2.Zero,
                Alignment.TopLeft,
                size,
                textureName,
                0,
                0,
                0,
                0,
                name,
                0.0f)
        {
        }

        /// <summary>
        /// Use this constructor if Frame is to be used as a child of another control.
        /// When a control is a child of another control, it's position will be relative
        /// to the parent control. Therefore there is no need to pass in a position.
        /// </summary>
        /// <param name="name">Name of frame control.</param>
        /// <param name="size">Size of frame control in pixels.</param>
        /// <param name="textureName">Texture to use for rendering. If texture is from a texture atlas use 'AtlasName.TextureName'</param>
        /// <param name="topPadding">Used to 'stretch' the texture to the size of the frame. Using 9-slice scaling (http://en.wikipedia.org/wiki/9-slice_scaling)</param>
        /// <param name="bottomPadding">Used to 'stretch' the texture to the size of the frame. Using 9-slice scaling (http://en.wikipedia.org/wiki/9-slice_scaling)</param>
        /// <param name="leftPadding">Used to 'stretch' the texture to the size of the frame. Using 9-slice scaling (http://en.wikipedia.org/wiki/9-slice_scaling)</param>
        /// <param name="rightPadding">Used to 'stretch' the texture to the size of the frame. Using 9-slice scaling (http://en.wikipedia.org/wiki/9-slice_scaling)</param>
        public Frame(
            string name,
            Vector2 size,
            string textureName,
            int topPadding,
            int bottomPadding,
            int leftPadding,
            int rightPadding) :
            this(
                Vector2.Zero,
                Alignment.TopLeft,
                size,
                textureName,
                topPadding,
                bottomPadding,
                leftPadding,
                rightPadding,
                name,
                0.0f)
        {
        }

        /// <summary>
        /// Use this constructor if Frame is expected to be stand alone (have no parent).
        /// </summary>
        /// <param name="position">Position of frame.</param>
        /// <param name="positionAlignment">Qualifies the position, is that position TopLeft, Center, etc.</param>
        /// <param name="size">Size of frame control in pixels.</param>
        /// <param name="textureName">Texture to use for rendering. If texture is from a texture atlas use 'AtlasName.TextureName'</param>
        /// <param name="name">Name of frame control.</param>
        public Frame(
            Vector2 position,
            Alignment positionAlignment,
            Vector2 size,
            string textureName,
            string name) :
            this(
                position,
                positionAlignment,
                size,
                textureName,
                0,
                0,
                0,
                0,
                name,
                0.0f)
        {
        }

        /// <summary>
        /// Use this constructor if Frame is expected to be stand alone (have no parent).
        /// </summary>
        /// <param name="position">Position of frame.</param>
        /// <param name="positionAlignment">Qualifies the position, is that position TopLeft, Center, etc.</param>
        /// <param name="size">Size of frame control in pixels.</param>
        /// <param name="textureName">Texture to use for rendering. If texture is from a texture atlas use 'AtlasName.TextureName'</param>
        /// <param name="topPadding">Used to 'stretch' the texture to the size of the frame. Using 9-slice scaling (http://en.wikipedia.org/wiki/9-slice_scaling)</param>
        /// <param name="bottomPadding">Used to 'stretch' the texture to the size of the frame. Using 9-slice scaling (http://en.wikipedia.org/wiki/9-slice_scaling)</param>
        /// <param name="leftPadding">Used to 'stretch' the texture to the size of the frame. Using 9-slice scaling (http://en.wikipedia.org/wiki/9-slice_scaling)</param>
        /// <param name="rightPadding">Used to 'stretch' the texture to the size of the frame. Using 9-slice scaling (http://en.wikipedia.org/wiki/9-slice_scaling)</param>
        /// <param name="name">Name of frame control.</param>
        public Frame(
            Vector2 position,
            Alignment positionAlignment,
            Vector2 size,
            string textureName,
            int topPadding,
            int bottomPadding,
            int leftPadding,
            int rightPadding,
            string name) :
            this(
                position,
                positionAlignment,
                size,
                textureName,
                topPadding,
                bottomPadding,
                leftPadding,
                rightPadding,
                name,
                0.0f)
        {
        }

        private Frame(
            Vector2 position,
            Alignment positionAlignment,
            Vector2 size,
            string textureName,
            int topPadding,
            int bottomPadding,
            int leftPadding,
            int rightPadding,
            string name,
            float layerDepth = 0.0f) :
            base(
                position,
                positionAlignment,
                size,
                textureName,
                name,
                layerDepth)
        {
            _topPadding = topPadding;
            _bottomPadding = bottomPadding;
            _leftPadding = leftPadding;
            _rightPadding = rightPadding;
        }

        public override void LoadContent(ContentManager content, bool loadChildrenContent = false)
        {
            Texture = AssetsManager.Instance.GetTexture(TextureAtlas);
            var atlas = AssetsManager.Instance.GetAtlas(TextureAtlas);

            var frame = atlas.Frames[TextureName];
            _sourcePatches = CreatePatches(frame.ToRectangle(), _topPadding, _bottomPadding, _leftPadding, _rightPadding);
            _destinationPatches = CreatePatches(new Rectangle(TopLeft.X, TopLeft.Y, Size.X, Size.Y), _topPadding, _bottomPadding, _leftPadding, _rightPadding);

            base.LoadContent(content, loadChildrenContent);
        }

        protected override void InDraw(SpriteBatch spriteBatch)
        {
            for (var i = 0; i < _sourcePatches.Length; ++i)
            {
                spriteBatch.Draw(Texture, _destinationPatches[i], _sourcePatches[i], Color, 0.0f, Vector2.Zero, SpriteEffects.None, LayerDepth);
            }
        }

        public override void SetTopLeftPosition(PointI point)
        {
            base.SetTopLeftPosition(point);

            if (_destinationPatches != null)
            {
                _destinationPatches = CreatePatches(new Rectangle(TopLeft.X, TopLeft.Y, Size.X, Size.Y), _topPadding, _bottomPadding, _leftPadding, _rightPadding);
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