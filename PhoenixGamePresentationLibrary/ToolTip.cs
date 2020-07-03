﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using GuiControls;

namespace PhoenixGamePresentationLibrary
{
    internal class ToolTip
    {
        private readonly Vector2 _topLeftPosition;

        private FrameDynamicSizing _frame;
        private List<IControl> _controls;

        public Vector2 Position { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        internal ToolTip(Vector2 topLeftPosition)
        {
            _topLeftPosition = topLeftPosition;
        }

        internal void LoadContent(ContentManager content)
        {
            _frame = new FrameDynamicSizing(_topLeftPosition, new Vector2(100.0f, 100.0f), "GUI_Textures_1", "frame3_whole", 47, 47, 47, 47);
            _frame.LoadContent(content);
            _controls = new List<IControl>();
        }

        internal void AddControl(IControl control)
        {
            control.Position = _frame.TopLeftPosition + control.Position;
            _controls.Add(control);
        }

        public void Update(GameTime gameTime, Matrix? transform = null)
        {
        }

        public void Draw(Matrix? transform = null)
        {
            _frame.Draw();
            foreach (var control in _controls)
            {
                control.Draw(transform);
            }
        }
    }
}