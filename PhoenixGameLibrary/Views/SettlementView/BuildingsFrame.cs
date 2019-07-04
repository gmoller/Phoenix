using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using GuiControls;
using Utilities;

namespace PhoenixGameLibrary.Views.SettlementView
{
    public class BuildingsFrame
    {
        private readonly Settlement _settlement;

        private readonly Vector2 _topLeftPosition;

        private readonly Texture2D _texture2;
        private readonly AtlasSpec2 _atlas;

        private readonly SmallFrame _smallFrame;
        private readonly Label _lblBuildings;

        public BuildingsFrame(Vector2 topLeftPosition, Settlement settlement)
        {
            _settlement = settlement;
            _topLeftPosition = topLeftPosition;

            _smallFrame = SmallFrame.Create(topLeftPosition + new Vector2(0, 10), new Vector2(500, 650), 10, 13, "GUI_Textures_1");
            _lblBuildings = new Label("lblBuildings", "CrimsonText-Regular-12", topLeftPosition, HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Left, Color.Orange, Color.Red);

            _texture2 = AssetsManager.Instance.GetTexture("Buildings");
            _atlas = AssetsManager.Instance.GetAtlas("Buildings");
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
            _lblBuildings.Text = "Buildings";
        }

        public void Draw()
        {
            _smallFrame.Draw();
            _lblBuildings.Draw();

            var spriteBatch = DeviceManager.Instance.GetCurrentSpriteBatch();
            spriteBatch.Begin();

            int topLeftX = (int)(_topLeftPosition.X + 15.0f);
            int topLeftY = (int)(_topLeftPosition.Y + 25.0f);
            spriteBatch.Draw(_texture2, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["ShipWrightsGuild"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture2, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["Sawmill"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture2, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["Barracks"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture2, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["Smithy"].ToRectangle(), Color.White);

            topLeftX = (int)(_topLeftPosition.X + 15.0f);
            topLeftY += 49;
            spriteBatch.Draw(_texture2, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_UpRight_Converge_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture2, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_Split_DownLeft"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture2, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_UpRight_Converge_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture2, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_Split_All"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture2, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Left_To_Right"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture2, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Left_To_Right"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture2, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Left_To_Right"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture2, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Left_To_Right"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture2, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Left_To_Right"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture2, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Left_To_Down"].ToRectangle(), Color.White);

            spriteBatch.End();
        }
    }
}