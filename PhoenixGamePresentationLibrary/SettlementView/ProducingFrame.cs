using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GuiControls;
using Input;

namespace PhoenixGamePresentationLibrary.SettlementView
{
    internal class ProducingFrame
    {
        private readonly SettlementView _parent;
        private readonly Vector2 _topLeftPosition;

        private FrameDynamicSizing _smallFrame;
        private Label _lblProducing;
        private Label _lblCurrent;

        internal ProducingFrame(SettlementView parent, Vector2 topLeftPosition)
        {
            _parent = parent;
            _topLeftPosition = topLeftPosition;
        }

        internal void LoadContent(ContentManager content)
        {
            _smallFrame = new FrameDynamicSizing(_topLeftPosition + new Vector2(0.0f, 10.0f), new Vector2(515, 160), "GUI_Textures_1", "frame2_whole", 50, 50, 50, 50);
            _smallFrame.LoadContent(content);

            _lblProducing = new Label("lblProducing", "CrimsonText-Regular-12", _topLeftPosition, HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Left, Color.Orange, Color.Red);
            _lblCurrent = new Label("lblCurrent", "CrimsonText-Regular-12", new Vector2(_topLeftPosition.X + 20.0f, _topLeftPosition.Y + 40.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Left, Color.Orange);
        }

        internal void Update(InputHandler input, float deltaTime)
        {
            _lblProducing.Text = "Producing";
            _lblCurrent.Text = _parent.Settlement.CurrentlyBuilding;
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            _smallFrame.Draw();
            _lblProducing.Draw();
            _lblCurrent.Draw();
            spriteBatch.End();
        }
    }
}