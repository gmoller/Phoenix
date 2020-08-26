using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using GuiControls;

namespace PhoenixGamePresentation
{
    internal class ToolTip
    {
        private readonly Vector2 _topLeftPosition;

        private readonly Frame _frame;
        private readonly List<IControl> _controls;

        public Vector2 Position { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        internal ToolTip(Vector2 topLeftPosition)
        {
            _topLeftPosition = topLeftPosition;

            _frame = new Frame(_topLeftPosition, Alignment.TopLeft, new Vector2(100.0f, 100.0f), "GUI_Textures_1", "frame3_whole", 47, 47, 47, 47, "Frame");
            _controls = new List<IControl>();
        }

        internal void LoadContent(ContentManager content)
        {
            _frame.LoadContent(content);
        }

        internal void AddControl(IControl control)
        {
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