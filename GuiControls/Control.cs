using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Input;
using MonoGameUtilities.ExtensionMethods;
using Utilities;

namespace GuiControls
{
    public abstract class Control : IControl
    {
        #region State
        public ControlStatus Status { get; set; }

        public bool Enabled { get; set; }

        public ChildControls ChildControls { get; }
        private Packages Packages { get; }

        private Vector2 Position { get; set; }
        private Alignment PositionAlignment { get; }
        public PointI Size { get; }
        public IControl Parent { get; set; }

        protected float LayerDepth { get; }

        public string Name { get; }
        #endregion

        protected Control(Vector2 position, Alignment positionAlignment, Vector2 size, string name, float layerDepth = 0.0f)
        {
            Name = name;
            LayerDepth = layerDepth;

            Position = position;
            PositionAlignment = positionAlignment;
            Size = size.ToPointI();

            Status = ControlStatus.None;
            Enabled = true;

            ChildControls = new ChildControls();
            Packages = new Packages();
        }

        #region Accessors
        protected Rectangle ActualDestinationRectangle => ControlHelper.DetermineArea(Position, PositionAlignment, Size);
        public int Top => ActualDestinationRectangle.Top;
        public int Bottom => ActualDestinationRectangle.Bottom;
        public int Left => ActualDestinationRectangle.Left;
        public int Right => ActualDestinationRectangle.Right;
        public PointI Center => new PointI(ActualDestinationRectangle.Center.X, ActualDestinationRectangle.Center.Y);
        public PointI TopLeft => new PointI(Left, Top);
        public PointI TopRight => new PointI(Right, Top);
        public PointI BottomLeft => new PointI(Left, Bottom);
        public PointI BottomRight => new PointI(Right, Bottom);
        public int Width => ActualDestinationRectangle.Width;
        public int Height => ActualDestinationRectangle.Height;

        public Rectangle Area => ActualDestinationRectangle; // TODO

        public PointI RelativeTopLeft => new PointI(Left - (Parent?.Left ?? 0), Top - (Parent?.Top ?? 0));
        public PointI RelativeTopRight => new PointI(RelativeTopLeft.X + Width, RelativeTopLeft.Y);
        public PointI RelativeMiddleRight => new PointI(RelativeTopLeft.X + Width, RelativeTopLeft.Y + (int)(Height * 0.5f));
        public PointI RelativeBottomLeft => new PointI(RelativeTopLeft.X, RelativeTopLeft.Y + Height);

        public IControl this[int index] => ChildControls[index];
        public IControl this[string key] => ChildControls.FindControl(key);
        #endregion

        /// <summary>
        /// Add a package to this control.
        /// </summary>
        /// <param name="package">Package to add</param>
        public void AddPackage(IPackage package)
        {
            Packages.Add(package);
        }

        /// <summary>
        /// Adds a child control to this control.
        /// </summary>
        /// <param name="childControl">Control to be added</param>
        /// <param name="parentAlignment">Used to determine the position of the child control in relation to the parent</param>
        /// <param name="childAlignment">Used to determine the position of the child control in relation to the parent</param>
        /// <param name="offset">Offset to be added to the child control's top left position</param>
        public void AddControl(Control childControl, Alignment parentAlignment = Alignment.TopLeft,
            Alignment childAlignment = Alignment.None, PointI offset = new PointI())
        {
            if (childAlignment == Alignment.None)
            {
                childAlignment = parentAlignment;
            }

            childControl.Parent = this;

            var topLeft = ControlHelper.DetermineTopLeft(childControl, parentAlignment, childAlignment, offset, Position, PositionAlignment, Size);

            childControl.SetTopLeftPosition(topLeft);
            ChildControls.Add(childControl.Name, childControl);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        public virtual void SetTopLeftPosition(PointI point)
        {
            ChildControls.SetTopLeftPosition(point);
            Position = ControlHelper.DetermineTopLeft(new Vector2(point.X, point.Y), PositionAlignment, Size);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        public void MoveTopLeftPosition(PointI point)
        {
            ChildControls.MoveTopLeftPosition(point);
            Position = new Vector2(Position.X + point.X, Position.Y + point.Y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="loadChildrenContent"></param>
        public virtual void LoadContent(ContentManager content, bool loadChildrenContent = false)
        {
            if (loadChildrenContent)
            {
                ChildControls.LoadChildControls(content, true);
            }
        }

        public virtual void Update(InputHandler input, float deltaTime, Viewport? viewport)
        {
            if (!Enabled)
            {
                Packages.Reset();
                Status = ControlStatus.None;
                return;
            }

            if (Status == ControlStatus.None && ControlHelper.IsMouseOverControl(ActualDestinationRectangle, input, viewport))
            {
                Status = ControlStatus.MouseOver;
            }
            else if (Status == ControlStatus.MouseOver && !ControlHelper.IsMouseOverControl(ActualDestinationRectangle, input, viewport))
            {
                Status = ControlStatus.None;
            }

            Status = Packages.Process(this, input, deltaTime);

            ChildControls.UpdateChildControls(input, deltaTime, viewport);
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

            ChildControls.DrawChildControls(spriteBatch);
        }

        public void Draw(Matrix? transform = null)
        {
            InDraw(transform);

            ChildControls.DrawChildControls(transform);
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        protected string DebuggerDisplay => $"{{Name={Name},TopLeftPosition={TopLeft},RelativeTopLeft={RelativeTopLeft},Size={Size}}}";
    }
}