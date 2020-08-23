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
        IControl Parent { get;  }
        EnumerableDictionary<IControl> ChildControls { get; }
        IControl this[int index] { get; }
        IControl this[string key] { get; }

        string Name { get; }

        int Top { get; }
        int Bottom { get; }
        int Left { get; }
        int Right { get; }
        Point Center { get; }
        Point TopLeft { get; }
        Point TopRight { get; }
        Point BottomLeft { get; }
        Point BottomRight { get; }

        Rectangle Area { get; }

        Point RelativeTopLeft { get; }
        Point RelativeTopRight { get; }
        Point RelativeMiddleRight { get; }
        Point RelativeBottomLeft { get; }

        int Width { get; }
        int Height { get; }
        Point Size { get; }

        bool Enabled { get; set; }

        event EventHandler Click;

        void AddControl(IControl childControl, Alignment parentAlignment = Alignment.TopLeft, Alignment childAlignment = Alignment.None, Point offset = new Point());
        void AddControls(params IControl[] controls);
        void SetTopLeftPosition(Point point);
        void MoveTopLeftPosition(Point point);
        void LoadContent(ContentManager content, bool loadChildrenContent = false);
        void Update(InputHandler input, float deltaTime, Matrix? transform = null);
        void Draw(Matrix? transform = null);
        void Draw(SpriteBatch spriteBatch);

        IControl Clone();
        string Serialize();
        void Deserialize(string json);
    }
}