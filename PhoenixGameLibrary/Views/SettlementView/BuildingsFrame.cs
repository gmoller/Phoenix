using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using GameLogic;
using GuiControls;
using Utilities;

namespace PhoenixGameLibrary.Views.SettlementView
{
    public class BuildingsFrame
    {
        private readonly Settlement _settlement;

        private readonly Vector2 _topLeftPosition;

        private readonly Texture2D _texture;
        private readonly AtlasSpec2 _atlas;

        private readonly SmallFrameWithSlots _smallFrame;
        private readonly Label _lblBuildings;

        public BuildingsFrame(Vector2 topLeftPosition, Settlement settlement)
        {
            _settlement = settlement;
            _topLeftPosition = topLeftPosition;

            _smallFrame = SmallFrameWithSlots.Create(topLeftPosition + new Vector2(0, 10), new Vector2(500, 650), 10, 13, "GUI_Textures_1");
            _lblBuildings = new Label("lblBuildings", "CrimsonText-Regular-12", topLeftPosition, HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Left, Color.Orange, Color.Red);

            _texture = AssetsManager.Instance.GetTexture("Buildings");
            _atlas = AssetsManager.Instance.GetAtlas("Buildings");
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
            _lblBuildings.Text = "Buildings";

            if (input.IsLeftMouseButtonReleased)
            {
                // determine where mouse pointer is (is it over a slot? which slot?)
                int baseTopLeftX = (int)(_topLeftPosition.X + 15.0f);
                int baseTopLeftY = (int)(_topLeftPosition.Y + 25.0f);
                foreach (var building in Globals.Instance.BuildingTypes)
                {
                    int topLeftX = baseTopLeftX + building.Slot.X * 49;
                    int topLeftY = baseTopLeftY + building.Slot.Y * 49;
                    var slotRectangle = new Rectangle(topLeftX, topLeftY, 40, 40);
                    if (slotRectangle.Contains(input.MousePostion))
                    {
                        // can building be built? requirements met? not already built?
                        if (_settlement.BuildingReadyToBeBeBuilt(building.Name))
                        {
                            _settlement.AddToProductionQueue(building);
                        }
                    }
                }
            }
        }

        public void Draw()
        {
            _smallFrame.Draw();
            _lblBuildings.Draw();

            var spriteBatch = DeviceManager.Instance.GetCurrentSpriteBatch();
            spriteBatch.Begin();

            int baseTopLeftX = (int)(_topLeftPosition.X + 15.0f);
            int baseTopLeftY = (int)(_topLeftPosition.Y + 25.0f);
            foreach (var building in Globals.Instance.BuildingTypes)
            {
                int topLeftX = baseTopLeftX + building.Slot.X * 49;
                int topLeftY = baseTopLeftY + building.Slot.Y * 49;
                DrawBuilding(spriteBatch, building.Name, topLeftX, topLeftY);
            }

            DrawArrows(spriteBatch);

            spriteBatch.End();
        }

        private void DrawBuilding(SpriteBatch spriteBatch, string buildingName, int topLeftX, int topLeftY)
        {
            var rect = new Rectangle(topLeftX, topLeftY, 40, 40);
            var color = Color.Transparent;
            if (_settlement.BuildingCanNotBeBuilt(buildingName))
            {
                color = Color.Red;
            }
            else if (_settlement.BuildingHasBeenBuilt(buildingName))
            {
                color = Color.ForestGreen;
            }
            else if (_settlement.BuildingReadyToBeBeBuilt(buildingName))
            {
                color = Color.LightGreen;
            }
            else // can be built, but requirements not met
            {
                color = Color.Black;
            }

            spriteBatch.FillRectangle(rect, color);
            spriteBatch.Draw(_texture, rect, _atlas.Frames[buildingName].ToRectangle(), Color.White);
        }

        private void DrawArrows(SpriteBatch spriteBatch)
        {
            var baseTopLeftX = (int)(_topLeftPosition.X + 15.0f);
            var baseTopLeftY = (int)(_topLeftPosition.Y + 25.0f);

            var topLeftX = baseTopLeftX;
            var topLeftY = baseTopLeftY;
            topLeftY += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_UpRight_Converge_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_Split_DownLeft"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_UpRight_Converge_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_Split_All"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Left_To_Right"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Left_To_Right"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Left_To_Right"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Left_To_Right"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Left_To_Right"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Left_To_Down"].ToRectangle(), Color.White);

            topLeftX = baseTopLeftX;
            topLeftY += 98;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_Split_DownRight"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_LeftRight_Converge_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_Split_DownLeft"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Right_To_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Right_Split_DownLeft"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_Split_All"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Left_Split_DownRight"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Left_To_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);

            topLeftX = baseTopLeftX;
            topLeftY += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 147;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 294;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);

            topLeftX = baseTopLeftX;
            topLeftY += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 98;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 98;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_UpRight_Converge_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_Split_DownLeft"].ToRectangle(), Color.White);

            topLeftX = baseTopLeftX;
            topLeftY += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 147;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 147;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 147;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);

            topLeftX = baseTopLeftX;
            topLeftY += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 98;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_UpRight_Converge_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Right_To_Left"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_Split_DownLeft"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_Split_DownRight"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Left_To_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);

            topLeftX = baseTopLeftX;
            topLeftY += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 98;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 196;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 147;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);

            topLeftX = baseTopLeftX;
            topLeftY += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 98;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_Split_DownRight"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_UpLeft_Converge_Down"].ToRectangle(), Color.White);
            topLeftX += 98;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_UpRight_Converge_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_Split_All"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_UpLeft_Converge_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);

            topLeftX = baseTopLeftX;
            topLeftY += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 98;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 147;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 98;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 98;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);

            topLeftX = baseTopLeftX;
            topLeftY += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 98;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 147;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_Split_DownRight"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_LeftRight_Converge_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_Split_DownLeft"].ToRectangle(), Color.White);
            topLeftX += 98;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);

            topLeftX = baseTopLeftX;
            topLeftY += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 98;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 245;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 98;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);

            topLeftX = baseTopLeftX;
            topLeftY += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 98;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_UpRight_Converge_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Right_To_Left"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Right_To_Left"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Right_To_Left"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Right_To_Left"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_Split_LeftRight"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Left_To_Right"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_UpLeft_Converge_Down"].ToRectangle(), Color.White);

            topLeftX = baseTopLeftX;
            topLeftY += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);

            topLeftX = baseTopLeftX;
            topLeftY += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_UpRight_Converge_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Right_To_Left"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Right_To_Left"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Right_To_Left"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Right_To_Left"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Right_To_Left"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Right_To_Left"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Right_To_Left"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Right_To_Left"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 40), _atlas.Frames["FlowChart_Up_To_Left"].ToRectangle(), Color.White);
        }
    }
}