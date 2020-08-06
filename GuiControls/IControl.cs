﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GuiControls
{
    public interface IControl
    {
        IControl Parent { get; }
        List<IControl> ChildControls { get; }

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

        Point RelativeTopLeft { get; }
        Point RelativeTopRight { get; }
        Point RelativeMiddleRight { get; }
        Point RelativeBottomLeft { get; }

        int Width { get; }
        int Height { get; }
        Point Size { get; }

        event EventHandler Click;

        void AddControl(IControl control);
        void AddControls(params IControl[] controls);
        void SetTopLeftPosition(int x, int y);
        void MoveTopLeftPosition(int x, int y);
        void LoadContent(ContentManager content);
        void Update(InputHandler input, float deltaTime, Matrix? transform = null);
        void Draw(Matrix? transform = null);
        void Draw(SpriteBatch spriteBatch);

        IControl Clone();
        string Serialize();
        void Deserialize(string json);
    }
}