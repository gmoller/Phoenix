using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GuiControls;

namespace PhoenixGamePresentationLibrary
{
    internal class ToolTip
    {
        private readonly FrameDynamicSizing _frame;
        private readonly List<IControl> _controls;

        public Vector2 Position { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        internal ToolTip(Vector2 topLeftPosition)
        {
            _frame = new FrameDynamicSizing(topLeftPosition, new Vector2(100.0f, 100.0f), "GUI_Textures_1", "frame3_whole", 47, 47, 47, 47);
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