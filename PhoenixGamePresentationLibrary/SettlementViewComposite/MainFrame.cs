using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GuiControls;
using Input;

namespace PhoenixGamePresentationLibrary.SettlementViewComposite
{
    internal class MainFrame : Control
    {
        #region State
        private readonly SettlementView _parent;
        private readonly Vector2 _topLeftPosition;

        private Frame _frmMain;
        #endregion

        public MainFrame(SettlementView parent, Vector2 topLeftPosition, string textureAtlas) :
            base(topLeftPosition, Alignment.TopLeft, new Vector2(556.0f, 741.0f), textureAtlas, "", "", "", "", "", "MainFrame")
        {
            _parent = parent;
            _topLeftPosition = topLeftPosition;

            _frmMain = new Frame(_topLeftPosition, Alignment.TopLeft, Size.ToVector2(), TextureAtlas, "frame_main", "frmMain");
            var frmHeader = new Frame(new Vector2(-2.0f, -100.0f), Alignment.TopLeft, new Vector2(560.0f, 146.0f), TextureAtlas, "frame_big_heading", "frmHeader", _frmMain);
            var lblSettlementName1 = new LabelAutoSized(new Vector2(278.0f, 49.0f), Alignment.MiddleCenter, GetTextFuncForSettlementType, "Carolingia-Regular-24", Color.Purple, "lblSettlementName1", Color.DarkBlue, frmHeader);
            var lblSettlementName2 = new LabelAutoSized(new Vector2(278.0f, 80.0f), Alignment.MiddleCenter, GetTextFuncForSettlementName, "Carolingia-Regular-24", Color.Purple, "lblSettlementName2", Color.DarkBlue, frmHeader);
            var btnClose = new Button(new Vector2(508.0f, 9.0f), Alignment.TopLeft, new Vector2(43.0f, 44.0f), TextureAtlas, "close_button_n", "close_button_a", "close_button_a", "close_button_h", "btnClose", 0.0f, _frmMain.ChildControls["frmHeader"]);
            btnClose.Click += CloseButtonClick;

            var frmResources = new Frame(new Vector2(20.0f, 190.0f), Alignment.TopLeft, new Vector2(515, 175), TextureAtlas, "frame2_whole", 50, 50, 50, 50, "frmResources", _frmMain);
            var lblResources = new LabelAutoSized(new Vector2(20.0f, 0.0f), Alignment.TopLeft, "Resources", "CrimsonText-Regular-12", Color.Orange, "lblResources", Color.DarkBlue, frmResources);
            var lblFood = new LabelAutoSized(new Vector2(20.0f, 25.0f), Alignment.TopLeft, "Food", "CrimsonText-Regular-12", Color.Orange, "lblFood", frmResources);
            var lblProduction = new LabelAutoSized(new Vector2(20.0f, 55.0f), Alignment.TopLeft, "Production", "CrimsonText-Regular-12", Color.Orange, "lblProduction", frmResources);
            var lblGold = new LabelAutoSized(new Vector2(20.0f, 85.0f), Alignment.TopLeft, "Gold", "CrimsonText-Regular-12", Color.Orange, "lblGold", frmResources);
            var lblPower = new LabelAutoSized(new Vector2(20.0f, 115.0f), Alignment.TopLeft, "Power", "CrimsonText-Regular-12", Color.Orange, "lblPower", frmResources);
            var lblResearch = new LabelAutoSized(new Vector2(20.0f, 145.0f), Alignment.TopLeft, "Research", "CrimsonText-Regular-12", Color.Orange, "lblResearch", frmResources);

            var frmProducing = new Frame(new Vector2(20.0f, 400.0f), Alignment.TopLeft, new Vector2(515, 160), TextureAtlas, "frame2_whole", 50, 50, 50, 50, "frmProducing", _frmMain);
            var lblProducing = new LabelAutoSized(new Vector2(20.0f, 0.0f), Alignment.TopLeft, "Producing", "CrimsonText-Regular-12", Color.Orange, "lblProducing", Color.DarkBlue, frmProducing);
            var lblCurrent = new LabelAutoSized(new Vector2(20.0f, 25.0f), Alignment.TopLeft, GetTextFuncForCurrentlyProducing, "CrimsonText-Regular-12", Color.Orange, "lblCurrent", frmProducing);

            var frmFooter = new Frame(new Vector2(-2.0f, 680.0f), Alignment.TopLeft, new Vector2(563.0f, 71.0f), TextureAtlas, "frame_bottom", "frmFooter", _frmMain);
        }

        private string GetTextFuncForSettlementType()
        {
            return $"{_parent.Settlement.SettlementType} of";
        }

        private string GetTextFuncForSettlementName()
        {
            return $"{_parent.Settlement.Name}";
        }

        private string GetTextFuncForCurrentlyProducing()
        {
            return $"{_parent.Settlement.CurrentlyBuilding}";
        }

        public override void LoadContent(ContentManager content)
        {
            _frmMain.LoadContent(content);
            _frmMain.ChildControls["frmHeader"].LoadContent(content);
            //_frmMain["frmHeader.lblSettlementName1"].LoadContent(content);
            _frmMain.ChildControls["frmHeader"].ChildControls["lblSettlementName1"].LoadContent(content);
            _frmMain.ChildControls["frmHeader"].ChildControls["lblSettlementName2"].LoadContent(content);
            _frmMain.ChildControls["frmHeader"].ChildControls["btnClose"].LoadContent(content);
            _frmMain.ChildControls["frmResources"].LoadContent(content);
            _frmMain.ChildControls["frmResources"].ChildControls["lblResources"].LoadContent(content);
            _frmMain.ChildControls["frmResources"].ChildControls["lblFood"].LoadContent(content);
            _frmMain.ChildControls["frmResources"].ChildControls["lblProduction"].LoadContent(content);
            _frmMain.ChildControls["frmResources"].ChildControls["lblGold"].LoadContent(content);
            _frmMain.ChildControls["frmResources"].ChildControls["lblPower"].LoadContent(content);
            _frmMain.ChildControls["frmResources"].ChildControls["lblResearch"].LoadContent(content);
            _frmMain.ChildControls["frmProducing"].LoadContent(content);
            _frmMain.ChildControls["frmProducing"].ChildControls["lblProducing"].LoadContent(content);
            _frmMain.ChildControls["frmProducing"].ChildControls["lblCurrent"].LoadContent(content);
            _frmMain.ChildControls["frmFooter"].LoadContent(content);
        }

        public override void Update(InputHandler input, float deltaTime, Matrix? transform = null)
        {
            _frmMain.Update(input, deltaTime, transform);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _frmMain.Draw(spriteBatch);
        }

        private void CloseButtonClick(object sender, EventArgs e)
        {
            _parent.CloseButtonClick(sender, e);
        }
    }
}