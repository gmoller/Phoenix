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

        private Frame _smallFrame;
        private LabelOld _lblProducing;
        private LabelOld _lblCurrent;

        internal ProducingFrame(SettlementView parent, Vector2 topLeftPosition)
        {
            _parent = parent;
            _topLeftPosition = topLeftPosition;
        }

        internal void LoadContent(ContentManager content)
        {
            _smallFrame = new Frame("SmallFrame", _topLeftPosition + new Vector2(0.0f, 10.0f), ContentAlignment.TopLeft, new Vector2(515, 160), "GUI_Textures_1", "frame2_whole", null, 50, 50, 50, 50);
            _smallFrame.LoadContent(content);

            _lblProducing = new LabelOld("lblProducing", "CrimsonText-Regular-12", _topLeftPosition, HorizontalAlignment.Left, VerticalAlignment.Top, new Vector2(10, 10), string.Empty, HorizontalAlignment.Left, Color.Orange, Color.Red);
            _lblCurrent = new LabelOld("lblCurrent", "CrimsonText-Regular-12", new Vector2(_topLeftPosition.X + 20.0f, _topLeftPosition.Y + 40.0f), HorizontalAlignment.Left, VerticalAlignment.Top, new Vector2(10, 10), string.Empty, HorizontalAlignment.Left, Color.Orange);
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