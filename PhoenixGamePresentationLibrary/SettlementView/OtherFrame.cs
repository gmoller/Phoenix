using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GuiControls;
using Input;

namespace PhoenixGamePresentationLibrary.SettlementView
{
    internal class OtherFrame
    {
        private readonly SettlementView _parent;
        private readonly Vector2 _topLeftPosition;

        private Label _lblOther;
        private FrameDynamicSizing _smallFrameOther;

        internal OtherFrame(SettlementView parent, Vector2 topLeftPosition)
        {
            _parent = parent;
            _topLeftPosition = topLeftPosition;
        }

        internal void LoadContent(ContentManager content)
        {
            _lblOther = new Label("lblOther", "CrimsonText-Regular-12", _topLeftPosition + new Vector2(0.0f, -10.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Left, Color.Orange, Color.Red);

            var slots3 = new DynamicSlots(_topLeftPosition + new Vector2(0.0f, 0.0f), new Vector2(515, 65), "GUI_Textures_1", "slot", 2, 1, 10.0f);
            slots3.LoadContent(content);
            _smallFrameOther = new FrameDynamicSizing(_topLeftPosition + new Vector2(0.0f, 0.0f), new Vector2(515, 65), "GUI_Textures_1", "frame2_whole", 50, 50, 50, 50, slots3);
            _smallFrameOther.LoadContent(content);
        }

        internal void Update(InputHandler input, float deltaTime)
        {
            _lblOther.Text = "Other";
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            _lblOther.Draw(spriteBatch);
            _smallFrameOther.Draw();
            spriteBatch.End();
        }
    }
}