using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GuiControls;
using Input;

namespace PhoenixGamePresentationLibrary.SettlementViewComposite
{
    internal class SecondaryFrame : Control
    {
        #region State
        private readonly SettlementView _parent;
        private readonly Vector2 _topLeftPosition;

        private Frame _secondaryFrame;

        #endregion

        internal SecondaryFrame(SettlementView parent, Vector2 topLeftPosition, string textureAtlas) :
            base(topLeftPosition, Alignment.TopLeft, new Vector2(556.0f, 741.0f), textureAtlas, "", "", "", "", "", "SecondaryFrame")
        {
            _parent = parent;
            _topLeftPosition = topLeftPosition;
        }

        public override void LoadContent(ContentManager content)
        {
            _secondaryFrame = new Frame(_topLeftPosition, Alignment.TopLeft, new Vector2(556.0f, 741.0f), TextureAtlas, "frame_main", "secondaryFrame");
            _secondaryFrame.LoadContent(content);

            var bottomFrame = new Frame(new Vector2(-2.0f, 680.0f), Alignment.TopLeft, new Vector2(563.0f, 71.0f), TextureAtlas, "frame_bottom", "bottomFrame", _secondaryFrame);
            bottomFrame.LoadContent(content);
        }

        public override void Update(InputHandler input, float deltaTime, Matrix? transform = null)
        {
            _secondaryFrame.Update(input, deltaTime, transform);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _secondaryFrame.Draw(spriteBatch);
        }
    }
}