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

        private LabelAutoSized _lblOther;
        private Frame _smallFrameOther;

        internal OtherFrame(SettlementView parent, Vector2 topLeftPosition)
        {
            _parent = parent;
            _topLeftPosition = topLeftPosition;
        }

        internal void LoadContent(ContentManager content)
        {
            _lblOther = new LabelAutoSized(_topLeftPosition + new Vector2(0.0f, -15.0f), Alignment.TopLeft, "Other", "CrimsonText-Regular-12", Color.Orange, Color.Red);
            _lblOther.LoadContent(content);

            var slots3 = new DynamicSlots(_topLeftPosition + new Vector2(0.0f, 0.0f), Alignment.TopLeft, new Vector2(515, 65), "GUI_Textures_1", "slot", 2, 1, 10.0f);
            slots3.LoadContent(content);
            _smallFrameOther = new Frame(_topLeftPosition + new Vector2(0.0f, 0.0f), Alignment.TopLeft, new Vector2(515, 65), "GUI_Textures_1", "frame2_whole", 50, 50, 50, 50, slots3);
            _smallFrameOther.LoadContent(content);
        }

        internal void Update(InputHandler input, float deltaTime)
        {
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            _lblOther.Draw(spriteBatch);
            _smallFrameOther.Draw(spriteBatch);
        }
    }
}