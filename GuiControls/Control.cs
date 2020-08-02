using System;
using AssetsLibrary;
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
        private float _cooldownTimeInMilliseconds;

        private readonly string _textureNormal;
        private readonly string _textureActive;
        private readonly string _textureHover;
        private readonly string _textureDisabled;

        protected readonly IControl Parent;

        protected readonly string TextureAtlas;
        protected readonly string TextureName;
        protected readonly Color Color;
        protected readonly float LayerDepth;

        protected Texture2D Texture { get; set; }
        protected Rectangle ActualDestinationRectangle { get; private set; }
        protected Rectangle SourceRectangle { get; private set; }
        protected AtlasSpec2 Atlas { get; private set; }

        public string Name { get; protected set; }

        public bool Enabled { get; set; }
        public bool MouseOver { get; private set; }

        public event EventHandler Click;

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
        public Point RelativeTopRight => new Point(RelativeTopLeft.X + Width, RelativeTopLeft.Y);
        public Point RelativeMiddleRight => new Point(RelativeTopLeft.X + Width, RelativeTopLeft.Y + (int)(Height * 0.5f));
        public Point RelativeBottomLeft => new Point(RelativeTopLeft.X, RelativeTopLeft.Y + Height);

        public int Width => ActualDestinationRectangle.Width;
        public int Height => ActualDestinationRectangle.Height;
        public Point Size => ActualDestinationRectangle.Size;

        private Control()
        {
        }

        protected Control(Vector2 position, Alignment positionAlignment, Vector2 size, string textureAtlas, string textureName, string textureNormal, string textureActive, string textureHover, string textureDisabled, float layerDepth = 0.0f, IControl parent = null, string name = "")
        {
            Parent = parent;

            Name = name;
            TextureAtlas = textureAtlas;
            TextureName = textureName;
            Color = Color.White;
            LayerDepth = layerDepth;

            _textureNormal = textureNormal;
            _textureActive = textureActive;
            _textureHover = textureHover;
            _textureDisabled = textureDisabled;

            DetermineArea(position, positionAlignment, size);

            Enabled = true;
        }

        protected Control(Control copyThis) : this()
        {
            _cooldownTimeInMilliseconds = copyThis._cooldownTimeInMilliseconds;
            _textureNormal = copyThis._textureNormal;
            _textureActive = copyThis._textureActive;
            _textureHover = copyThis._textureHover;
            _textureDisabled = copyThis._textureDisabled;
            Parent = copyThis.Parent;
            TextureAtlas = copyThis.TextureAtlas;
            TextureName = copyThis.TextureName;
            Color = copyThis.Color;
            LayerDepth = copyThis.LayerDepth;
            Texture = copyThis.Texture;
            ActualDestinationRectangle = copyThis.ActualDestinationRectangle;
            SourceRectangle = copyThis.SourceRectangle;
            Atlas = copyThis.Atlas;
            Name = copyThis.Name;
            Enabled = copyThis.Enabled;
            MouseOver = copyThis.MouseOver;
            Click = copyThis.Click;
        }

        public virtual IControl Clone()
        {
            return null;
        }

        public void SetTopLeftPosition(int x, int y)
        {
            ActualDestinationRectangle = new Rectangle(x, y, ActualDestinationRectangle.Width, ActualDestinationRectangle.Height);
        }

        public void MoveTopLeftPosition(int x, int y)
        {
            ActualDestinationRectangle = new Rectangle(ActualDestinationRectangle.X + x, ActualDestinationRectangle.Y + y, ActualDestinationRectangle.Width, ActualDestinationRectangle.Height);
        }

        public virtual void LoadContent(ContentManager content)
        {
            if (TextureAtlas.HasValue())
            {
                Atlas = AssetsManager.Instance.GetAtlas(TextureAtlas);
                Texture = AssetsManager.Instance.GetTexture(TextureAtlas);
                SourceRectangle = Atlas.Frames[TextureName].ToRectangle();
            }
            else // no atlas
            {
                SetTexture(_textureNormal);
            }
        }

        protected virtual void BeforeUpdate(InputHandler input, float deltaTime, Matrix? transform = null)
        {
        }

        protected virtual void AfterUpdate(InputHandler input, float deltaTime, Matrix? transform = null)
        {
        }

        public virtual void Update(InputHandler input, float deltaTime, Matrix? transform = null)
        {
            if (!Enabled)
            {
                SetTexture(_textureDisabled);
                return;
            }

            BeforeUpdate(input, deltaTime, transform);

            Point mousePosition;
            if (transform == null)
            {
                mousePosition = input.MousePosition;
            }
            else
            {
                var worldPosition = DeviceManager.Instance.WorldPositionPointedAtByMouseCursor;
                mousePosition = new Point(worldPosition.X, worldPosition.Y);
            }

            MouseOver = ActualDestinationRectangle.Contains(mousePosition);
            input.Eaten = MouseOver;

            if (_cooldownTimeInMilliseconds > 0.0f)
            {
                _cooldownTimeInMilliseconds -= deltaTime;
                if (_cooldownTimeInMilliseconds <= 0.0f)
                {
                    OnClickComplete();
                }
                return;
            }

            SetTexture(MouseOver ? _textureHover : _textureNormal);

            if (MouseOver && input.IsLeftMouseButtonReleased)
            {
                OnClick(new EventArgs());
            }

            AfterUpdate(input, deltaTime, transform);
        }

        private void OnClickComplete()
        {
            _cooldownTimeInMilliseconds = 0.0f;

            SetTexture(_textureNormal);
        }

        private void OnClick(EventArgs e)
        {
            _cooldownTimeInMilliseconds = 200.0f;
            SetTexture(_textureActive);
            Click?.Invoke(this, e);
        }

        protected virtual void BeforeDraw(SpriteBatch spriteBatch)
        {
        }

        protected virtual void BeforeDraw(Matrix? transform = null)
        {
        }

        protected virtual void InDraw(SpriteBatch spriteBatch)
        {
        }

        protected virtual void InDraw(Matrix? transform = null)
        {
        }

        protected virtual void AfterDraw(SpriteBatch spriteBatch)
        {
        }

        protected virtual void AfterDraw(Matrix? transform = null)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            BeforeDraw(spriteBatch);

            InDraw(spriteBatch);

            AfterDraw(spriteBatch);
        }

        public virtual void Draw(Matrix? transform = null)
        {
            BeforeDraw(transform);

            var spriteBatch = BeginSpriteBatch(transform);

            InDraw(transform);

            EndSpriteBatch(spriteBatch);

            AfterDraw(transform);
        }

        protected void DetermineArea(Vector2 position, Alignment alignment, Vector2 size)
        {
            //var topLeft = DetermineTopLeft(position, alignment, size);
            var topLeft = DetermineTopLeft(position * DeviceManager.Instance.SizeRatio, alignment, size * DeviceManager.Instance.SizeRatio);
            if (Parent == null)
            {
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

        private Vector2 DetermineTopLeft(Vector2 position, Alignment alignment, Vector2 size)
        {
            Vector2 topLeft;
            switch (alignment)
            {
                case Alignment.TopLeft:
                    topLeft = position;
                    break;
                case Alignment.TopCenter:
                    topLeft = new Vector2(position.X - size.X * 0.5f, position.Y);
                    break;
                case Alignment.TopRight:
                    topLeft = new Vector2(position.X - size.X, position.Y);
                    break;
                case Alignment.MiddleLeft:
                    topLeft = new Vector2(position.X, position.Y - size.Y * 0.5f);
                    break;
                case Alignment.MiddleCenter:
                    topLeft = new Vector2(position.X - size.X * 0.5f, position.Y - size.Y * 0.5f);
                    break;
                case Alignment.MiddleRight:
                    topLeft = new Vector2(position.X - size.X, position.Y - size.Y * 0.5f);
                    break;
                case Alignment.BottomLeft:
                    topLeft = new Vector2(position.X, position.Y - size.Y);
                    break;
                case Alignment.BottomCenter:
                    topLeft = new Vector2(position.X - size.X * 0.5f, position.Y - size.Y);
                    break;
                case Alignment.BottomRight:
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
            //spriteBatch.Begin(rasterizerState: new RasterizerState { ScissorTestEnable = true }, transformMatrix: transform);

            //_originalScissorRectangle = spriteBatch.GraphicsDevice.ScissorRectangle;
            //spriteBatch.GraphicsDevice.ScissorRectangle = ActualDestinationRectangle;

            return spriteBatch;
        }

        protected void EndSpriteBatch(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            //spriteBatch.GraphicsDevice.ScissorRectangle = _originalScissorRectangle;
            DeviceManager.Instance.ReturnSpriteBatchToPool(spriteBatch);
        }

        protected void SetTexture(string textureName)
        {
            if (Atlas != null) // HasAtlas
            {
                var f = Atlas.Frames[textureName];
                SourceRectangle = new Rectangle(f.X, f.Y, f.Width, f.Height);
            }
            else
            {
                if (textureName.HasValue())
                {
                    Texture = AssetsManager.Instance.GetTexture(textureName);
                    SourceRectangle = Texture.Bounds;
                }
            }
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        protected string DebuggerDisplay => $"{{Name={Name},TopLeftPosition={TopLeft},RelativeTopLeft={RelativeTopLeft},Size={Size}}}";
    }
}