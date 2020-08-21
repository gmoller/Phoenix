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
        private readonly SettlementView _settlementView;

        private Frame _frmMain;
        #endregion

        public MainFrame(SettlementView settlementView, Vector2 topLeftPosition, string textureAtlas) :
            base(topLeftPosition, Alignment.TopLeft, new Vector2(556.0f, 741.0f), textureAtlas, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "MainFrame")
        {
            _settlementView = settlementView;

            _frmMain = new Frame(topLeftPosition, Alignment.TopLeft, Size.ToVector2(), TextureAtlas, "frame_main", "frmMain");
            var frmHeader = new Frame(new Vector2(-2.0f, -100.0f), Alignment.TopLeft, new Vector2(560.0f, 146.0f), TextureAtlas, "frame_big_heading", "frmHeader", _frmMain);
            var lblSettlementName1 = new LabelAutoSized(new Vector2(278.0f, 49.0f), Alignment.MiddleCenter, GetTextFuncForSettlementType, "Carolingia-Regular-24", Color.Purple, "lblSettlementName1", Color.DarkBlue, frmHeader);
            var lblSettlementName2 = new LabelAutoSized(new Vector2(278.0f, 80.0f), Alignment.MiddleCenter, GetTextFuncForSettlementName, "Carolingia-Regular-24", Color.Purple, "lblSettlementName2", Color.DarkBlue, frmHeader);
            var btnClose = new Button(new Vector2(508.0f, 9.0f), Alignment.TopLeft, new Vector2(43.0f, 44.0f), TextureAtlas, "close_button_n", "close_button_a", "close_button_a", "close_button_h", "btnClose", 0.0f, _frmMain.ChildControls["frmHeader"]);
            btnClose.Click += CloseButtonClick;

            var frmPopulation = new Frame(new Vector2(20.0f, 50.0f), Alignment.TopLeft, new Vector2(515, 120), TextureAtlas, "frame2_whole", 50, 50, 50, 50, "frmPopulation", _frmMain);
            var lblRace = new LabelAutoSized(new Vector2(0.0f, -20.0f), Alignment.TopLeft, GetTextFuncForRace(), "CrimsonText-Regular-12", Color.Orange, "lblRace", frmPopulation);
            var lblPopulationGrowth = new LabelAutoSized(frmPopulation.RelativeTopRight.ToVector2() + new Vector2(-20.0f, -70.0f), Alignment.TopRight, GetTextFuncForPopulationGrowth(), "CrimsonText-Regular-12", Color.Orange, "lblPopulationGrowth", frmPopulation);
            var lblFarmers = new LabelAutoSized(new Vector2(20.0f, 25.0f), Alignment.TopLeft, "Farmers:", "CrimsonText-Regular-12", Color.Orange, "lblFarmers", frmPopulation);
            var lblWorkers = new LabelAutoSized(new Vector2(20.0f, 55.0f), Alignment.TopLeft, "Workers:", "CrimsonText-Regular-12", Color.Orange, "lblWorkers", frmPopulation);
            var lblRebels = new LabelAutoSized(new Vector2(20.0f, 85.0f), Alignment.TopLeft, "Rebels:", "CrimsonText-Regular-12", Color.Orange, "lblRebels", frmPopulation);
            var citizenView = new CitizenView(new Vector2(130.0f, 20.0f), Alignment.TopLeft, settlementView, "Citizens", "citizenView", frmPopulation);

            var frmResources = new Frame(new Vector2(20.0f, 190.0f), Alignment.TopLeft, new Vector2(515, 175), TextureAtlas, "frame2_whole", 50, 50, 50, 50, "frmResources", _frmMain);
            var lblResources = new LabelAutoSized(new Vector2(20.0f, 0.0f), Alignment.TopLeft, "Resources", "CrimsonText-Regular-12", Color.Orange, "lblResources", Color.DarkBlue, frmResources);
            var lblFood = new LabelAutoSized(new Vector2(20.0f, 25.0f), Alignment.TopLeft, "Food", "CrimsonText-Regular-12", Color.Orange, "lblFood", frmResources);
            var foodView = new FoodView(new Vector2(130.0f, 0.0f), Alignment.TopLeft, settlementView, "Bread", "Corn", "foodView", lblFood);
            var lblProduction = new LabelAutoSized(new Vector2(20.0f, 55.0f), Alignment.TopLeft, "Production", "CrimsonText-Regular-12", Color.Orange, "lblProduction", frmResources);
            var productionView = new ProductionView(new Vector2(130.0f, 0.0f), Alignment.TopLeft, settlementView, "Anvil", "Pickaxe", "productionView", lblProduction);
            var lblGold = new LabelAutoSized(new Vector2(20.0f, 85.0f), Alignment.TopLeft, "Gold", "CrimsonText-Regular-12", Color.Orange, "lblGold", frmResources);
            var lblPower = new LabelAutoSized(new Vector2(20.0f, 115.0f), Alignment.TopLeft, "Power", "CrimsonText-Regular-12", Color.Orange, "lblPower", frmResources);
            var lblResearch = new LabelAutoSized(new Vector2(20.0f, 145.0f), Alignment.TopLeft, "Research", "CrimsonText-Regular-12", Color.Orange, "lblResearch", frmResources);

            var frmProducing = new Frame(new Vector2(20.0f, 400.0f), Alignment.TopLeft, new Vector2(515, 160), TextureAtlas, "frame2_whole", 50, 50, 50, 50, "frmProducing", _frmMain);
            var lblProducing = new LabelAutoSized(new Vector2(20.0f, 0.0f), Alignment.TopLeft, "Producing", "CrimsonText-Regular-12", Color.Orange, "lblProducing", Color.DarkBlue, frmProducing);
            var lblCurrent = new LabelAutoSized(new Vector2(20.0f, 25.0f), Alignment.TopLeft, GetTextFuncForCurrentlyProducing, "CrimsonText-Regular-12", Color.Orange, "lblCurrent", frmProducing);

            var frmFooter = new Frame(new Vector2(-2.0f, 680.0f), Alignment.TopLeft, new Vector2(563.0f, 71.0f), TextureAtlas, "frame_bottom", "frmFooter", _frmMain);
        }

        private string GetTextFuncForRace()
        {
            return $"{_settlementView.Settlement.RaceType.Name}";
        }

        private string GetTextFuncForPopulationGrowth()
        {
            return $"Population: {_settlementView.Settlement.Population} (+{_settlementView.Settlement.GrowthRate})";
        }

        private string GetTextFuncForSettlementType()
        {
            return $"{_settlementView.Settlement.SettlementType} of";
        }

        private string GetTextFuncForSettlementName()
        {
            return $"{_settlementView.Settlement.Name}";
        }

        private string GetTextFuncForCurrentlyProducing()
        {
            return $"{_settlementView.Settlement.CurrentlyBuilding}";
        }

        public override void LoadContent(ContentManager content)
        {
            _frmMain.LoadContent(content);
            _frmMain["frmHeader"].LoadContent(content);
            _frmMain["frmHeader.lblSettlementName1"].LoadContent(content);
            _frmMain["frmHeader.lblSettlementName2"].LoadContent(content);
            _frmMain["frmHeader.btnClose"].LoadContent(content);
            _frmMain["frmPopulation"].LoadContent(content);
            _frmMain["frmPopulation.lblRace"].LoadContent(content);
            _frmMain["frmPopulation.lblPopulationGrowth"].LoadContent(content);
            _frmMain["frmPopulation.lblFarmers"].LoadContent(content);
            _frmMain["frmPopulation.lblWorkers"].LoadContent(content);
            _frmMain["frmPopulation.lblRebels"].LoadContent(content);
            _frmMain["frmPopulation.citizenView"].LoadContent(content);
            _frmMain["frmResources"].LoadContent(content);
            _frmMain["frmResources.lblResources"].LoadContent(content);
            _frmMain["frmResources.lblFood"].LoadContent(content);
            _frmMain["frmResources.lblFood.foodView"].LoadContent(content);
            _frmMain["frmResources.lblProduction"].LoadContent(content);
            _frmMain["frmResources.lblProduction.productionView"].LoadContent(content);
            _frmMain["frmResources.lblGold"].LoadContent(content);
            _frmMain["frmResources.lblPower"].LoadContent(content);
            _frmMain["frmResources.lblResearch"].LoadContent(content);
            _frmMain["frmProducing"].LoadContent(content);
            _frmMain["frmProducing.lblProducing"].LoadContent(content);
            _frmMain["frmProducing.lblCurrent"].LoadContent(content);
            _frmMain["frmFooter"].LoadContent(content);
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
            _settlementView.CloseButtonClick(sender, e);
        }
    }
}