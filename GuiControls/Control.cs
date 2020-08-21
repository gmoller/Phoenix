using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using AssetsLibrary;
using Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Utilities;
using Utilities.ExtensionMethods;
using Point = Microsoft.Xna.Framework.Point;

namespace GuiControls
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class Control : IControl
    {
        private const float CLICK_COOLDOWN_TIME_IN_MILLISECONDS = 100.0f;

        #region State
        private float _cooldownTimeInMilliseconds;

        private string _textureNormal;
        private string _textureActive;
        private string _textureHover;
        private string _textureDisabled;

        private Dictionary<string, IControl> _childControls;

        public IControl Parent { get; private set; }

        protected string TextureAtlas { get; private set; }
        protected string TextureName { get; private set; }
        protected Color Color { get; private set; }
        protected float LayerDepth { get; private set; }

        protected Texture2D Texture { get; set; }
        protected Rectangle ActualDestinationRectangle { get; private set; }
        protected Rectangle SourceRectangle { get; private set; }
        protected AtlasSpec2 Atlas { get; private set; }

        public string Name { get; protected set; }

        private bool _enabled;
        public bool Enabled
        {
            get => Parent?.Enabled ?? _enabled;
            set => _enabled = value;
        }
        public bool MouseOver { get; private set; }

        public event EventHandler Click;
        #endregion

        public int Top => ActualDestinationRectangle.Top;
        public int Bottom => ActualDestinationRectangle.Bottom;
        public int Left => ActualDestinationRectangle.Left;
        public int Right => ActualDestinationRectangle.Right;
        public Point Center => ActualDestinationRectangle.Center;
        public Point TopLeft => new Point(Left, Top);
        public Point TopRight => new Point(Right, Top);
        public Point BottomLeft => new Point(Left, Bottom);
        public Point BottomRight => new Point(Right, Bottom);

        public Rectangle Area => new Rectangle(TopLeft, Size);

        public Point RelativeTopLeft => new Point(Left - (Parent?.Left ?? 0), Top - (Parent?.Top ?? 0));
        public Point RelativeTopRight => new Point(RelativeTopLeft.X + Width, RelativeTopLeft.Y);
        public Point RelativeMiddleRight => new Point(RelativeTopLeft.X + Width, RelativeTopLeft.Y + (int)(Height * 0.5f));
        public Point RelativeBottomLeft => new Point(RelativeTopLeft.X, RelativeTopLeft.Y + Height);

        public int Width => ActualDestinationRectangle.Width;
        public int Height => ActualDestinationRectangle.Height;
        public Point Size => ActualDestinationRectangle.Size;

        public EnumerableDictionary<IControl> ChildControls => new EnumerableDictionary<IControl>(_childControls);
        public IControl this[string key] => FindControl(key);

        private Control()
        {
        }

        protected Control(Vector2 position, Alignment positionAlignment, Vector2 size, string textureAtlas, string textureName, string textureNormal, string textureActive, string textureHover, string textureDisabled, string name, float layerDepth = 0.0f, IControl parent = null)
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

            _childControls = new Dictionary<string, IControl>();

            Parent?.AddControl(this);
        }

        protected Control(Control copyThis) : this()
        {
            Copy(copyThis);
        }

        public virtual IControl Clone()
        {
            return null;
        }

        public void AddControl(IControl control)
        {
            if (control.Parent == null)
            {
                ((Control)control).Parent = this;
            }

            _childControls.Add(control.Name, control);
        }

        public void AddControls(params IControl[] controls)
        {
            foreach (var control in controls)
            {
                AddControl(control);
            }
        }

        public void SetTopLeftPosition(int x, int y)
        {
            foreach (var child in ChildControls)
            {
                child.SetTopLeftPosition(x + child.RelativeTopLeft.X, y + child.RelativeTopLeft.Y);
            }

            ActualDestinationRectangle = new Rectangle(x, y, ActualDestinationRectangle.Width, ActualDestinationRectangle.Height);
        }

        public void MoveTopLeftPosition(int x, int y)
        {
            foreach (var child in ChildControls)
            {
                child.MoveTopLeftPosition(x, y);
            }

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

        public virtual void Update(InputHandler input, float deltaTime, Matrix? transform = null)
        {
            var mousePosition = GetMousePosition(input, transform);
            MouseOver = ActualDestinationRectangle.Contains(mousePosition);

            if (Enabled)
            {
                SetTexture(MouseOver ? _textureHover : _textureNormal);
            }
            else
            {
                SetTexture(_textureDisabled);
            }

            if (_cooldownTimeInMilliseconds > 0.0f)
            {
                SetTexture(_textureActive);
                _cooldownTimeInMilliseconds -= deltaTime;
                if (_cooldownTimeInMilliseconds <= 0.0f)
                {
                    OnClickComplete();
                }
                return;
            }

            if (MouseOver && input.IsLeftMouseButtonReleased)
            {
                OnClick(new EventArgs());
            }

            foreach (var childControl in ChildControls)
            {
                childControl.Update(input, deltaTime, transform);
            }
        }

        private Point GetMousePosition(InputHandler input, Matrix? transform)
        {
            Point mousePosition;
            if (transform == null)
            {
                mousePosition = input.MousePosition;
            }
            else
            {
                var context = (GlobalContext)CallContext.LogicalGetData("AmbientGlobalContext");
                var worldPosition = context.WorldPositionPointedAtByMouseCursor;
                mousePosition = new Point(worldPosition.X, worldPosition.Y);
            }

            return mousePosition;
        }

        private void OnClickComplete()
        {
            _cooldownTimeInMilliseconds = 0.0f;
        }

        private void OnClick(EventArgs e)
        {
            if (!Enabled) return;

            _cooldownTimeInMilliseconds = CLICK_COOLDOWN_TIME_IN_MILLISECONDS;
            Click?.Invoke(this, e);
        }

        protected virtual void InDraw(SpriteBatch spriteBatch)
        {
        }

        protected virtual void InDraw(Matrix? transform = null)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            InDraw(spriteBatch);

            foreach (var childControl in ChildControls)
            {
                childControl.Draw(spriteBatch);
            }
        }

        public virtual void Draw(Matrix? transform = null)
        {
            InDraw(transform);

            foreach (var childControl in ChildControls)
            {
                childControl.Draw(transform);
            }
        }

        protected void DetermineArea(Vector2 position, Alignment alignment, Vector2 size)
        {
            var topLeft = DetermineTopLeft(position, alignment, size);
            //var topLeft = DetermineTopLeft(position * DeviceManager.Instance.SizeRatio, alignment, size * DeviceManager.Instance.SizeRatio);
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

        public string Serialize()
        {
            var json = JsonConvert.SerializeObject(this);

            return json;
        }

        public void Deserialize(string json)
        {
            var o = JsonConvert.DeserializeObject<Control>(json);
            Copy(o);
        }

        private void Copy(Control copyThis)
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
            _childControls = copyThis._childControls;
        }

        private IControl FindControl(string key)
        {
            var split = key.Split('.');
            var childControl = ChildControls[split[0]];
            for (var i = 1; i < split.Length; i++)
            {
                childControl = childControl[split[i]];
            }

            return childControl;
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        protected string DebuggerDisplay => $"{{Name={Name},TopLeftPosition={TopLeft},RelativeTopLeft={RelativeTopLeft},Size={Size}}}";
    }
}