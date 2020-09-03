using System;
using System.Collections.Generic;
using System.Linq;
using Assets;
using Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Utilities;
using Utilities.ExtensionMethods;

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
        protected Microsoft.Xna.Framework.Color Color { get; private set; }
        protected float LayerDepth { get; private set; }

        protected Texture2D Texture { get; set; }
        protected Rectangle ActualDestinationRectangle { get; set; }
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

        public event EventHandler<EventArgs> Click;
        #endregion State

        public int Top => ActualDestinationRectangle.Top;
        public int Bottom => ActualDestinationRectangle.Bottom;
        public int Left => ActualDestinationRectangle.Left;
        public int Right => ActualDestinationRectangle.Right;
        public PointI Center => new PointI(ActualDestinationRectangle.Center.X, ActualDestinationRectangle.Center.Y);
        public PointI TopLeft => new PointI(Left, Top);
        public PointI TopRight => new PointI(Right, Top);
        public PointI BottomLeft => new PointI(Left, Bottom);
        public PointI BottomRight => new PointI(Right, Bottom);

        public Rectangle Area => new Rectangle(TopLeft.X, TopLeft.Y, Size.X, Size.Y);

        public PointI RelativeTopLeft => new PointI(Left - (Parent?.Left ?? 0), Top - (Parent?.Top ?? 0));
        public PointI RelativeTopRight => new PointI(RelativeTopLeft.X + Width, RelativeTopLeft.Y);
        public PointI RelativeMiddleRight => new PointI(RelativeTopLeft.X + Width, RelativeTopLeft.Y + (int)(Height * 0.5f));
        public PointI RelativeBottomLeft => new PointI(RelativeTopLeft.X, RelativeTopLeft.Y + Height);

        public int Width => ActualDestinationRectangle.Width;
        public int Height => ActualDestinationRectangle.Height;
        public PointI Size => new PointI(ActualDestinationRectangle.Size.X, ActualDestinationRectangle.Size.Y);

        public EnumerableDictionary<IControl> ChildControls => new EnumerableDictionary<IControl>(_childControls);
        public IControl this[int index] => _childControls.Values.ElementAt(index);
        public IControl this[string key] => FindControl(key);

        private Control()
        {
        }

        protected Control(Vector2 position, Alignment positionAlignment, Vector2 size, string textureAtlas, string textureName, string textureNormal, string textureActive, string textureHover, string textureDisabled, string name, float layerDepth = 0.0f)
        {
            Name = name;
            TextureAtlas = textureAtlas;
            TextureName = textureName;
            Color = Microsoft.Xna.Framework.Color.White;
            LayerDepth = layerDepth;

            _textureNormal = textureNormal;
            _textureActive = textureActive;
            _textureHover = textureHover;
            _textureDisabled = textureDisabled;

            ActualDestinationRectangle = DetermineArea(position, positionAlignment, size);

            Enabled = true;

            _childControls = new Dictionary<string, IControl>();
        }

        protected Control(Control copyThis) : this()
        {
            Copy(copyThis);
        }

        public virtual IControl Clone()
        {
            return null;
        }

        /// <summary>
        /// Adds a child control to this control.
        /// </summary>
        /// <param name="childControl">Control to be added</param>
        /// /// <param name="parentAlignment">Used to determine the position of the child control in relation to the parent</param>
        /// <param name="childAlignment">Used to determine the position of the child control in relation to the parent</param>
        /// <param name="offset">Offset to be added to the child control's top left position</param>
        public void AddControl(IControl childControl, Alignment parentAlignment = Alignment.TopLeft, Alignment childAlignment = Alignment.None, PointI offset = new PointI())
        {
            if (childAlignment == Alignment.None)
            {
                childAlignment = parentAlignment;
            }

            ((Control)childControl).Parent = this;

            var topLeft = DetermineTopLeft(childControl, parentAlignment, childAlignment, offset);

            childControl.SetTopLeftPosition(topLeft);
            _childControls.Add(childControl.Name, childControl);
        }

        public void AddControls(params IControl[] controls)
        {
            foreach (var control in controls)
            {
                AddControl(control, Alignment.TopLeft, Alignment.TopLeft, PointI.Zero);
            }
        }

        public void SetTopLeftPosition(PointI point)
        {
            foreach (var child in ChildControls)
            {
                child.SetTopLeftPosition(point + child.RelativeTopLeft);
            }

            ActualDestinationRectangle = new Rectangle(point.X, point.Y, ActualDestinationRectangle.Width, ActualDestinationRectangle.Height);
        }

        public void MoveTopLeftPosition(PointI point)
        {
            foreach (var child in ChildControls)
            {
                child.MoveTopLeftPosition(point);
            }

            ActualDestinationRectangle = new Rectangle(ActualDestinationRectangle.X + point.X, ActualDestinationRectangle.Y + point.Y, ActualDestinationRectangle.Width, ActualDestinationRectangle.Height);
        }

        public virtual void LoadContent(ContentManager content, bool loadChildrenContent = false)
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

            if (loadChildrenContent)
            {
                foreach (var child in ChildControls)
                {
                    child.LoadContent(content, true);
                }
            }
        }

        public virtual void Update(InputHandler input, float deltaTime, Viewport? viewport)
        {
            var mousePosition = GetMousePosition(input, viewport);
            MouseOver = ActualDestinationRectangle.Contains(mousePosition.X, mousePosition.Y);

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

            if (Click != null)
            {
                if (MouseOver)
                {
                    if (input.IsLeftMouseButtonReleased)
                    {
                        OnClick(new MouseEventArgs(new PointI(input.MousePosition.X, input.MousePosition.Y)));
                    }
                }
            }

            foreach (var childControl in ChildControls)
            {
                childControl.Update(input, deltaTime, viewport);
            }
        }

        private PointI GetMousePosition(InputHandler input, Viewport? viewport)
        {
            PointI mousePosition;
            if (viewport.HasValue)
            {
                mousePosition = new PointI(input.MousePosition.X - viewport.Value.X, input.MousePosition.Y - viewport.Value.Y);
            }
            else
            {
                mousePosition = new PointI(input.MousePosition.X, input.MousePosition.Y);
            }

            //mousePosition = new Point(input.MousePosition.X, input.MousePosition.Y);
            //var context = (GlobalContext)CallContext.LogicalGetData("GameMetadata");
            //var worldPosition = context.WorldPositionPointedAtByMouseCursor;
            //mousePosition = new Point(worldPosition.X, worldPosition.Y);

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

        protected Rectangle DetermineArea(Vector2 position, Alignment alignment, Vector2 size)
        {
            var topLeft = DetermineTopLeft(position, alignment, size);
            var actualDestinationRectangle = new Rectangle((int)topLeft.X, (int)topLeft.Y, (int)size.X, (int)size.Y);

            return actualDestinationRectangle;
        }

        private PointI DetermineTopLeft(IControl childControl, Alignment parentAlignment, Alignment childAlignment, PointI offset)
        {
            PointI topLeft;
            switch (parentAlignment)
            {
                case Alignment.TopLeft when childAlignment == Alignment.TopLeft:
                    topLeft = new PointI(Left, Top);
                    break;
                case Alignment.TopCenter when childAlignment == Alignment.TopCenter:
                    topLeft = new PointI(Left + (int)((Size.X - childControl.Size.X) * 0.5f), Top);
                    break;
                case Alignment.TopRight when childAlignment == Alignment.TopRight:
                    topLeft = new PointI(Right - childControl.Size.X, Top);
                    break;

                case Alignment.MiddleLeft when childAlignment == Alignment.MiddleLeft:
                    topLeft = new PointI(Left, Top + (int)((Size.Y - childControl.Size.Y) * 0.5f));
                    break;
                case Alignment.MiddleCenter when childAlignment == Alignment.MiddleCenter:
                    topLeft = new PointI(Left + (int)((Size.X - childControl.Size.X) * 0.5f), Top + (int)((Size.Y - childControl.Size.Y) * 0.5f));
                    break;
                case Alignment.MiddleRight when childAlignment == Alignment.MiddleRight:
                    topLeft = new PointI(Right, Top + (int)((Size.Y - childControl.Size.Y) * 0.5f));
                    break;
                case Alignment.MiddleRight when childAlignment == Alignment.MiddleLeft:
                    topLeft = new PointI(Right, Top + (int)((Size.Y - childControl.Size.Y) * 0.5f));
                    break;

                case Alignment.BottomLeft when childAlignment == Alignment.BottomLeft:
                    topLeft = new PointI(Left, Bottom - childControl.Size.Y);
                    break;
                case Alignment.BottomLeft when childAlignment == Alignment.TopLeft:
                    topLeft = new PointI(Left, Bottom);
                    break;
                case Alignment.BottomCenter when childAlignment == Alignment.BottomCenter:
                    topLeft = new PointI(Left + (int)((Size.X - childControl.Size.X) * 0.5f), Bottom - childControl.Size.Y);
                    break;
                case Alignment.BottomCenter when childAlignment == Alignment.TopCenter:
                    topLeft = new PointI(Left + (int)((Size.X - childControl.Size.X) * 0.5f), Bottom);
                    break;
                case Alignment.BottomRight when childAlignment == Alignment.BottomRight:
                    topLeft = new PointI(Right - childControl.Size.X, Bottom - childControl.Size.Y);
                    break;
                default:
                    throw new Exception($"ParentAlignment [{parentAlignment}] with ChildAlignment [{childAlignment}] not implemented.");
            }
            topLeft += offset;

            return topLeft;
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

        public void SetTexture(Texture2D texture)
        {
            Texture = texture;
            SourceRectangle = Texture.Bounds;
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