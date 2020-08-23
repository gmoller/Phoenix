using System.Runtime.Remoting.Messaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using GuiControls;
using Input;
using MonoGameUtilities;
using PhoenixGameLibrary;
using PhoenixGamePresentationLibrary.Views;

namespace PhoenixGamePresentationLibrary.SettlementViewComposite
{
    internal class BuildingsView : Control
    {
        #region State
        private readonly SettlementView _settlementView;

        private readonly IControl _slots;

        private Texture2D _texture;
        private AtlasSpec2 _atlas;
        #endregion

        internal BuildingsView(string name, SettlementView settlementView, string textureAtlas) :
            base(Vector2.Zero, Alignment.TopLeft, new Vector2(515.0f, 450.0f), textureAtlas, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, name)
        {
            _settlementView = settlementView;

            _slots = new DynamicSlots(new Vector2(0.0f, 0.0f), Alignment.TopLeft, new Vector2(515.0f, 450.0f), textureAtlas, "slot", 10, 13, 10.0f, "slots");
        }

        public override void LoadContent(ContentManager content, bool loadChildrenContent = false)
        {
            _slots.LoadContent(content);

            _texture = AssetsManager.Instance.GetTexture("Buildings");
            _atlas = AssetsManager.Instance.GetAtlas("Buildings");
        }

        public override void Update(InputHandler input, float deltaTime, Matrix? transform = null)
        {
            if (input.IsLeftMouseButtonReleased)
            {
                var context = (GlobalContext)CallContext.LogicalGetData("AmbientGlobalContext");
                var buildingTypes = context.GameMetadata.BuildingTypes;

                // determine where mouse pointer is (is it over a slot? which slot?)
                var baseTopLeftX = Left + 15;
                var baseTopLeftY = Top + 25;
                foreach (var building in buildingTypes)
                {
                    var topLeftX = baseTopLeftX + building.Slot.X * 49;
                    var topLeftY = baseTopLeftY + building.Slot.Y * 25;
                    var slotRectangle = new Rectangle(topLeftX, topLeftY, 40, 20);
                    if (slotRectangle.Contains(input.MousePosition))
                    {
                        // can building be built? requirements met? not already built?
                        if (_settlementView.Settlement.BuildingReadyToBeBeBuilt(building.Name))
                        {
                            _settlementView.Settlement.AddToProductionQueue(building);
                        }
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawBuildings(spriteBatch);
            DrawArrows(spriteBatch);
        }

        private void DrawBuildings(SpriteBatch spriteBatch)
        {
            var context = (GlobalContext)CallContext.LogicalGetData("AmbientGlobalContext");
            var buildingTypes = context.GameMetadata.BuildingTypes;

            var baseTopLeftX = Left + 15;
            var baseTopLeftY = Top + 25;
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
            if (_settlementView.Settlement.BuildingCanNotBeBuilt(buildingName))
            {
                color = Color.Red;
            }
            else if (_settlementView.Settlement.BuildingHasBeenBuilt(buildingName))
            {
                color = Color.ForestGreen;
            }
            else if (_settlementView.Settlement.BuildingReadyToBeBeBuilt(buildingName))
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
            var baseTopLeftX = Left + 15;
            var baseTopLeftY = Top + 25;

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