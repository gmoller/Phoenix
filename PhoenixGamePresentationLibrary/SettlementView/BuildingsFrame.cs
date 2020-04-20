using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using GameLogic;
using GuiControls;
using Utilities;

namespace PhoenixGamePresentationLibrary.SettlementView
{
    internal class BuildingsFrame
    {
        private readonly SettlementView _parent;

        private readonly Vector2 _topLeftPosition;

        private readonly Texture2D _texture;
        private readonly AtlasSpec2 _atlas;

        private readonly FrameDynamicSizing _smallFrameBuildings;
        private readonly FrameDynamicSizing _smallFrameUnits;
        private readonly FrameDynamicSizing _smallFrameOther;
        private readonly Label _lblBuildings;
        private readonly Label _lblUnits;
        private readonly Label _lblOther;

        internal BuildingsFrame(SettlementView parent, Vector2 topLeftPosition)
        {
            _parent = parent;
            _topLeftPosition = topLeftPosition;

            _smallFrameBuildings = new FrameDynamicSizing(topLeftPosition + new Vector2(0.0f, 10.0f), new Vector2(515, 450), "GUI_Textures_1", "frame2_whole", 50, 50, 50, 50, 10, 13);
            _lblBuildings = new Label("lblBuildings", "CrimsonText-Regular-12", topLeftPosition, HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Left, Color.Orange, Color.Red);
            _smallFrameUnits = new FrameDynamicSizing(topLeftPosition + new Vector2(0.0f, 495.0f), new Vector2(515, 75), "GUI_Textures_1", "frame2_whole", 50, 50, 50, 50, 10, 2);
            _lblUnits = new Label("lblUnits", "CrimsonText-Regular-12", topLeftPosition + new Vector2(0, 485), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Left, Color.Orange, Color.Red);
            _smallFrameOther = new FrameDynamicSizing(topLeftPosition + new Vector2(0.0f, 610.0f), new Vector2(515, 65), "GUI_Textures_1", "frame2_whole", 50, 50, 50, 50, 2, 1);
            _lblOther = new Label("lblOther", "CrimsonText-Regular-12", topLeftPosition + new Vector2(0, 600), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Left, Color.Orange, Color.Red);

            _texture = AssetsManager.Instance.GetTexture("Buildings");
            _atlas = AssetsManager.Instance.GetAtlas("Buildings");
        }

        internal void Update(float deltaTime, InputHandler input)
        {
            _lblBuildings.Text = "Buildings";
            _lblUnits.Text = "Units";
            _lblOther.Text = "Other";

            if (input.IsLeftMouseButtonReleased)
            {
                // determine where mouse pointer is (is it over a slot? which slot?)
                int baseTopLeftX = (int)(_topLeftPosition.X + 15.0f);
                int baseTopLeftY = (int)(_topLeftPosition.Y + 25.0f);
                foreach (var building in Globals.Instance.BuildingTypes)
                {
                    int topLeftX = baseTopLeftX + building.Slot.X * 49;
                    int topLeftY = baseTopLeftY + building.Slot.Y * 25;
                    var slotRectangle = new Rectangle(topLeftX, topLeftY, 40, 20);
                    if (slotRectangle.Contains(input.MousePostion))
                    {
                        // can building be built? requirements met? not already built?
                        if (_parent.Settlement.BuildingReadyToBeBeBuilt(building.Name))
                        {
                            _parent.Settlement.AddToProductionQueue(building);
                        }
                    }
                }
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            _smallFrameBuildings.Draw();
            _lblBuildings.Draw(spriteBatch);
            _smallFrameUnits.Draw();
            _lblUnits.Draw(spriteBatch);
            _smallFrameOther.Draw();
            _lblOther.Draw(spriteBatch);

            int baseTopLeftX = (int)(_topLeftPosition.X + 15.0f);
            int baseTopLeftY = (int)(_topLeftPosition.Y + 25.0f);
            foreach (var building in Globals.Instance.BuildingTypes)
            {
                int topLeftX = baseTopLeftX + building.Slot.X * 49;
                int topLeftY = baseTopLeftY + building.Slot.Y * 25;
                DrawBuilding(spriteBatch, building.Name, topLeftX, topLeftY);
            }

            DrawArrows(spriteBatch);

            spriteBatch.End();
        }

        private void DrawBuilding(SpriteBatch spriteBatch, string buildingName, int topLeftX, int topLeftY)
        {
            var rect = new Rectangle(topLeftX, topLeftY, 40, 20);
            var color = Color.Transparent;
            if (_parent.Settlement.BuildingCanNotBeBuilt(buildingName))
            {
                color = Color.Red;
            }
            else if (_parent.Settlement.BuildingHasBeenBuilt(buildingName))
            {
                color = Color.ForestGreen;
            }
            else if (_parent.Settlement.BuildingReadyToBeBeBuilt(buildingName))
            {
                color = Color.LightGreen;
            }
            else // can be built, but requirements not met
            {
                color = Color.Black;
            }

            spriteBatch.FillRectangle(rect, color, 0.0f);
            spriteBatch.Draw(_texture, rect, _atlas.Frames[buildingName].ToRectangle(), Color.White);
        }

        private void DrawArrows(SpriteBatch spriteBatch)
        {
            var baseTopLeftX = (int)(_topLeftPosition.X + 15.0f);
            var baseTopLeftY = (int)(_topLeftPosition.Y + 25.0f);

            var topLeftX = baseTopLeftX;
            var topLeftY = baseTopLeftY;
            topLeftY += 25;

            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_UpRight_Converge_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_Split_DownLeft"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_UpRight_Converge_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_Split_All"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Left_To_Right"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Left_To_Right"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Left_To_Right"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Left_To_Right"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Left_To_Right"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Left_To_Down"].ToRectangle(), Color.White);

            topLeftX = baseTopLeftX;
            topLeftY += 50;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_Split_DownRight"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_LeftRight_Converge_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_Split_DownLeft"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Right_To_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Right_Split_DownLeft"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_Split_All"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Left_Split_DownRight"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Left_To_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);

            topLeftX = baseTopLeftX;
            topLeftY += 25;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 147;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 294;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);

            topLeftX = baseTopLeftX;
            topLeftY += 25;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 98;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 98;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_UpRight_Converge_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_Split_DownLeft"].ToRectangle(), Color.White);

            topLeftX = baseTopLeftX;
            topLeftY += 25;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 147;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 147;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 147;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);

            topLeftX = baseTopLeftX;
            topLeftY += 25;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 98;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_UpRight_Converge_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Right_To_Left"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_Split_DownLeft"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_Split_DownRight"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Left_To_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);

            topLeftX = baseTopLeftX;
            topLeftY += 25;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 98;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 196;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 147;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);

            topLeftX = baseTopLeftX;
            topLeftY += 25;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 98;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_Split_DownRight"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_UpLeft_Converge_Down"].ToRectangle(), Color.White);
            topLeftX += 98;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_UpRight_Converge_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_Split_All"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_UpLeft_Converge_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);

            topLeftX = baseTopLeftX;
            topLeftY += 25;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 98;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 147;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 98;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 98;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);

            topLeftX = baseTopLeftX;
            topLeftY += 25;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 98;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 147;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_Split_DownRight"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_LeftRight_Converge_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_Split_DownLeft"].ToRectangle(), Color.White);
            topLeftX += 98;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);

            topLeftX = baseTopLeftX;
            topLeftY += 25;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 98;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 245;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 98;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);

            topLeftX = baseTopLeftX;
            topLeftY += 25;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);
            topLeftX += 98;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_UpRight_Converge_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Right_To_Left"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Right_To_Left"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Right_To_Left"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Right_To_Left"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_Split_LeftRight"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Left_To_Right"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_UpLeft_Converge_Down"].ToRectangle(), Color.White);

            topLeftX = baseTopLeftX;
            topLeftY += 25;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_To_Down"].ToRectangle(), Color.White);

            topLeftX = baseTopLeftX;
            topLeftY += 25;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_UpRight_Converge_Down"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Right_To_Left"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Right_To_Left"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Right_To_Left"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Right_To_Left"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Right_To_Left"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Right_To_Left"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Right_To_Left"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Right_To_Left"].ToRectangle(), Color.White);
            topLeftX += 49;
            spriteBatch.Draw(_texture, new Rectangle(topLeftX, topLeftY, 40, 20), _atlas.Frames["FlowChart_Up_To_Left"].ToRectangle(), Color.White);
        }
    }
}