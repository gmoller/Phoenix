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
        private LabelAutoSized _lblProducing;
        private LabelAutoSized _lblCurrent;

        internal ProducingFrame(SettlementView parent, Vector2 topLeftPosition)
        {
            _parent = parent;
            _topLeftPosition = topLeftPosition;
        }

        internal void LoadContent(ContentManager content)
        {
            _smallFrame = new Frame(_topLeftPosition + new Vector2(0.0f, 10.0f), Alignment.TopLeft, new Vector2(515, 160), "GUI_Textures_1", "frame2_whole", 50, 50, 50, 50, "smallFrame");
            _smallFrame.LoadContent(content);

            _lblProducing = new LabelAutoSized(_topLeftPosition, Alignment.TopLeft, "Producing", "CrimsonText-Regular-12", Color.Orange, "lblProducing", Color.Red);
            _lblProducing.LoadContent(content);
            _lblCurrent = new LabelAutoSized(new Vector2(_topLeftPosition.X + 20.0f, _topLeftPosition.Y + 35.0f), Alignment.TopLeft, string.Empty, "CrimsonText-Regular-12", Color.Orange, "lblCurrent");
            _lblCurrent.LoadContent(content);
        }

        internal void Update(InputHandler input, float deltaTime)
        {
            _lblCurrent.Text = _parent.Settlement.CurrentlyBuilding;
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            _smallFrame.Draw(spriteBatch);
            _lblProducing.Draw(spriteBatch);
            _lblCurrent.Draw(spriteBatch);
        }
    }
}