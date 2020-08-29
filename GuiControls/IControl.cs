using System;
using Microsoft.Xna.Framework;
using Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Utilities;
using Point = Utilities.Point;

namespace GuiControls
{
    public interface IControl
    {
        /// <summary>
        /// Parent control that "owns" this control.
        /// </summary>
        IControl Parent { get;  }
        /// <summary>
        /// Enumerable list of all child controls "owned" by this control.
        /// </summary>
        EnumerableDictionary<IControl> ChildControls { get; }
        /// <summary>
        /// Indexer by index to get a child control.
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns>Child control</returns>
        IControl this[int index] { get; }
        /// <summary>
        /// Indexer by key to get a child control (Name of child control).
        /// Note: A syntax like OuterFrame["InnerFrame.PrettyLabel.HappyImage"] can be used to get child
        /// controls within child controls. This will return 'HappyImage' within the control hierarchy.
        /// </summary>
        /// <param name="key">Key child control name)</param>
        /// <returns>Child control</returns>
        IControl this[string key] { get; }

        /// <summary>
        /// Name of the control.
        /// This will be used as a dictionary key if control is added to a parent.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Position of this controls Top, relative to the top of the screen.
        /// </summary>
        int Top { get; }
        /// <summary>
        /// Position of this controls Bottom, relative to the top of the screen.
        /// </summary>
        int Bottom { get; }
        /// <summary>
        /// Position of this controls Left, relative to the left of the screen.
        /// </summary>
        int Left { get; }
        /// <summary>
        /// Position of this controls Right, relative to the left of the screen.
        /// </summary>
        int Right { get; }
        /// <summary>
        /// Position of this controls Center, relative to the top left of the screen.
        /// </summary>
        Point Center { get; }
        /// <summary>
        /// Position of this controls TopLeft point, relative to the top left of the screen.
        /// </summary>
        Point TopLeft { get; }
        /// <summary>
        /// Position of this controls TopRight point, relative to the top left of the screen.
        /// </summary>
        Point TopRight { get; }
        /// <summary>
        /// Position of this controls BottomLeft point, relative to the top left of the screen.
        /// </summary>
        Point BottomLeft { get; }
        /// <summary>
        /// Position of this controls BottomRight point, relative to the top left of the screen.
        /// </summary>
        Point BottomRight { get; }

        Rectangle Area { get; }

        /// <summary>
        /// Position of this controls TopLeft point, relative to the TopLeft of the parent control.
        /// </summary>
        Point RelativeTopLeft { get; }
        /// <summary>
        /// Position of this controls TopRight point, relative to the TopLeft of the parent control.
        /// </summary>
        Point RelativeTopRight { get; }
        /// <summary>
        /// Position of this controls MiddleRight point, relative to the TopLeft of the parent control.
        /// </summary>
        Point RelativeMiddleRight { get; }
        /// <summary>
        /// Position of this controls BottomLeft point, relative to the TopLeft of the parent control.
        /// </summary>
        Point RelativeBottomLeft { get; }

        int Width { get; }
        int Height { get; }
        Point Size { get; }

        bool Enabled { get; set; }

        bool MouseOver { get; }

        event EventHandler Click;

        void AddControl(IControl childControl, Alignment parentAlignment = Alignment.TopLeft, Alignment childAlignment = Alignment.None, Point offset = new Point());
        void SetTopLeftPosition(Point point);
        void MoveTopLeftPosition(Point point);
        void LoadContent(ContentManager content, bool loadChildrenContent = false);
        void Update(InputHandler input, float deltaTime, Viewport? viewport);
        void Draw(Matrix? transform = null);
        void Draw(SpriteBatch spriteBatch);

        IControl Clone();
        string Serialize();
        void Deserialize(string json);
    }
}