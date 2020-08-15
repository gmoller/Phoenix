using System.Runtime.Remoting.Messaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using GuiControls;
using Input;
using PhoenixGameLibrary.GameData;
using Utilities;

namespace PhoenixGamePresentationLibrary.SettlementView
{
    internal class BuildingsFrame
    {
        private readonly SettlementView _parent;
        private readonly Vector2 _topLeftPosition;

        private Texture2D _texture;
        private AtlasSpec2 _atlas;

        private LabelAutoSized _lblBuildings;
        private Frame _smallFrameBuildings;

        internal BuildingsFrame(SettlementView parent, Vector2 topLeftPosition)
        {
            _parent = parent;
            _topLeftPosition = topLeftPosition;
        }

        internal void LoadContent(ContentManager content)
        {
            _lblBuildings = new LabelAutoSized(_topLeftPosition - new Vector2(0.0f, 5.0f), Alignment.TopLeft, "Buildings", "CrimsonText-Regular-12", Color.Orange, "lblBuildings", Color.Red);
            _lblBuildings.LoadContent(content);

            var slots = new DynamicSlots(_topLeftPosition + new Vector2(0.0f, 10.0f), Alignment.TopLeft, new Vector2(515, 450), "GUI_Textures_1", "slot", 10, 13, 10.0f, "slots");
            slots.LoadContent(content);
            _smallFrameBuildings = new Frame(_topLeftPosition + new Vector2(0.0f, 10.0f), Alignment.TopLeft, new Vector2(515, 450), "GUI_Textures_1", "frame2_whole", 50, 50, 50, 50, "smallFrameBuildings", slots);
            _smallFrameBuildings.LoadContent(content);

            _texture = AssetsManager.Instance.GetTexture("Buildings");
            _atlas = AssetsManager.Instance.GetAtlas("Buildings");
        }

        internal void Update(InputHandler input, float deltaTime)
        {
            if (input.IsLeftMouseButtonReleased)
            {
                var context = (GlobalContext)CallContext.LogicalGetData("AmbientGlobalContext");
                var buildingTypes = ((GameMetadata)context.GameMetadata).BuildingTypes;

                // determine where mouse pointer is (is it over a slot? which slot?)
                var baseTopLeftX = (int)(_topLeftPosition.X + 15.0f);
                var baseTopLeftY = (int)(_topLeftPosition.Y + 25.0f);
                foreach (var building in buildingTypes)
                {
                    var topLeftX = baseTopLeftX + building.Slot.X * 49;
                    var topLeftY = baseTopLeftY + building.Slot.Y * 25;
                    var slotRectangle = new Rectangle(topLeftX, topLeftY, 40, 20);
                    if (slotRectangle.Contains(input.MousePosition))
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
            _lblBuildings.Draw(spriteBatch);
            _smallFrameBuildings.Draw(spriteBatch);

            DrawBuildings(spriteBatch);
            DrawArrows(spriteBatch);
        }

        private void DrawBuildings(SpriteBatch spriteBatch)
        {
            var context = (GlobalContext)CallContext.LogicalGetData("AmbientGlobalContext");
            var buildingTypes = ((GameMetadata)context.GameMetadata).BuildingTypes;

            var baseTopLeftX = (int)(_topLeftPosition.X + 15.0f);
            var baseTopLeftY = (int)(_topLeftPosition.Y + 25.0f);
            foreach (var building in buildingTypes)
            {
                var topLeftX = baseTopLeftX + building.Slot.X * 49;
                var topLeftY = baseTopLeftY + building.Slot.Y * 25;
                DrawBuilding(spriteBatch, building.Name, topLeftX, topLeftY);
            } 
        }

        private void DrawBuilding(SpriteBatch spriteBatch, string buildingName, int topLeftX, int topLeftY)
        {
            var rect = new Rectangle(topLeftX, topLeftY, 40, 20);
            Color color;
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