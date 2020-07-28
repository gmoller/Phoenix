using System;
using Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Utilities;
using Point = Microsoft.Xna.Framework.Point;

namespace GuiControls
{
    public abstract class Control : IControl
    {
        private Rectangle _originalScissorRectangle;

        protected readonly IControl Parent;

        protected readonly string TextureAtlas;
        protected readonly string TextureName;
        protected readonly byte? TextureId;
        protected readonly Color Color;
        protected readonly float LayerDepth;

        protected Texture2D Texture;
        protected Rectangle ActualDestinationRectangle;
        protected Rectangle SourceRectangle;

        public string Name { get; protected set; }

        public int Top => ActualDestinationRectangle.Top;
        public int Bottom => ActualDestinationRectangle.Bottom;
        public int Left => ActualDestinationRectangle.Left;
        public int Right => ActualDestinationRectangle.Right;
        public Point Center => ActualDestinationRectangle.Center;
        public Point TopLeft => new Point(Left, Top);
        public Point TopRight => new Point(Right, Top);
        public Point BottomLeft => new Point(Left, Bottom);
        public Point BottomRight => new Point(Right, Bottom);

        public Point RelativeTopLeft => new Point(Left - (Parent?.Left ?? 0), Top - (Parent?.Top ?? 0));

        public int Width => ActualDestinationRectangle.Width;
        public int Height => ActualDestinationRectangle.Height;
        public Point Size => ActualDestinationRectangle.Size;

        protected Control(string name, Vector2 position, ContentAlignment alignment, Vector2 size, string textureAtlas, string textureName, byte? textureId = null, float layerDepth = 0.0f, IControl parent = null)
        {
            Parent = parent;

            Name = name;
            TextureAtlas = textureAtlas;
            TextureName = textureName;
            TextureId = textureId;
            Color = Color.White;
            LayerDepth = layerDepth;

            var topLeft = DetermineTopLeft(position * DeviceManager.Instance.SizeRatio, alignment, size * DeviceManager.Instance.SizeRatio);
            if (Parent == null)
            {
                // the same
                ActualDestinationRectangle = new Rectangle((int)topLeft.X, (int)topLeft.Y, (int)size.X, (int)size.Y);
            }
            else
            {
                // offset from parent's position
                var x = (int)(Parent.TopLeft.X + topLeft.X);
                var y = (int)(Parent.TopLeft.Y + topLeft.Y);
                ActualDestinationRectangle = new Rectangle(x, y, (int)size.X, (int)size.Y);
            }
        }

        public void SetTopLeftPosition(int x, int y)
        {
            ActualDestinationRectangle.X = x;
            ActualDestinationRectangle.Y = y;
        }

        public void MoveTopLeftPosition(int x, int y)
        {
            ActualDestinationRectangle.X += x;
            ActualDestinationRectangle.Y += y;
        }

        public abstract void LoadContent(ContentManager content);

        public abstract void Update(InputHandler input, float deltaTime, Matrix? transform = null);

        public abstract void Draw(Matrix? transform = null);

        private Vector2 DetermineTopLeft(Vector2 position, ContentAlignment alignment, Vector2 size)
        {
            Vector2 topLeft;
            switch (alignment)
            {
                case ContentAlignment.TopLeft:
                    topLeft = position;
                    break;
                case ContentAlignment.TopCenter:
                    topLeft = new Vector2(position.X - size.X * 0.5f, position.Y);
                    break;
                case ContentAlignment.TopRight:
                    topLeft = new Vector2(position.X - size.X, position.Y);
                    break;
                case ContentAlignment.MiddleLeft:
                    topLeft = new Vector2(position.X, position.Y - size.Y * 0.5f);
                    break;
                case ContentAlignment.MiddleCenter:
                    topLeft = new Vector2(position.X - size.X * 0.5f, position.Y - size.Y * 0.5f);
                    break;
                case ContentAlignment.MiddleRight:
                    topLeft = new Vector2(position.X - size.X, position.Y - size.Y * 0.5f);
                    break;
                case ContentAlignment.BottomLeft:
                    topLeft = new Vector2(position.X, position.Y - size.Y);
                    break;
                case ContentAlignment.BottomCenter:
                    topLeft = new Vector2(position.X - size.X * 0.5f, position.Y - size.Y);
                    break;
                case ContentAlignment.BottomRight:
                    topLeft = new Vector2(position.X - size.X, position.Y - size.Y);
                    break;
                default:
                    throw new Exception($"Alignment [{alignment}] not implemented.");
            }

            return topLeft;
        }

        protected SpriteBatch BeginSpriteBatch(Matrix? transform)
        {
            var spriteBatch = DeviceManager.Instance.GetNewSpriteBatch();
            spriteBatch.Begin(rasterizerState: new RasterizerState { ScissorTestEnable = true }, transformMatrix: transform);

            _originalScissorRectangle = spriteBatch.GraphicsDevice.ScissorRectangle;
            spriteBatch.GraphicsDevice.ScissorRectangle = ActualDestinationRectangle;

            return spriteBatch;
        }

        protected void EndSpriteBatch(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.GraphicsDevice.ScissorRectangle = _originalScissorRectangle;
            DeviceManager.Instance.ReturnSpriteBatchToPool(spriteBatch);
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        protected string DebuggerDisplay => $"{{Name={Name},TopLeftPosition={TopLeft},RelativeTopLeft={RelativeTopLeft},Size={Size}}}";
    }
}