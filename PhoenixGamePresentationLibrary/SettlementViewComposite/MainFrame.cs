using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GuiControls;
using Input;
using MonoGameUtilities.ExtensionMethods;
using Point = Utilities.Point;

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

            _frmMain.AddControl(new Frame("frmHeader", new Vector2(560.0f, 146.0f), TextureAtlas, "frame_big_heading"), Alignment.TopCenter, Alignment.TopCenter, new Point(0, -100));
            _frmMain["frmHeader"].AddControl(new LabelSized("lblSettlementName1", new Vector2(100.0f, 15.0f), Alignment.MiddleCenter, GetTextFuncForSettlementType, "Carolingia-Regular-24", Color.Purple, Color.DarkBlue), Alignment.TopCenter, Alignment.TopCenter, new Point(0, 45));
            _frmMain["frmHeader"].AddControl(new LabelSized("lblSettlementName2", new Vector2(100.0f, 15.0f), Alignment.MiddleCenter, GetTextFuncForSettlementName, "Carolingia-Regular-24", Color.Purple, Color.DarkBlue), Alignment.TopCenter, Alignment.TopCenter, new Point(0, 75));
            var btnClose = new Button("btnClose", new Vector2(43.0f, 44.0f), TextureAtlas, "close_button_n", "close_button_a", "close_button_a", "close_button_h");
            btnClose.Click += CloseButtonClick;
            _frmMain["frmHeader"].AddControl(btnClose, Alignment.TopRight, Alignment.TopRight, new Point(-8, 8));

            _frmMain.AddControl(new Frame("frmPopulation", new Vector2(515.0f, 120.0f), TextureAtlas, "frame2_whole", 50, 50, 50, 50), Alignment.TopCenter, Alignment.TopCenter, new Point(0, 50));
            _frmMain["frmPopulation"].AddControl(new LabelSized("lblRace", new Vector2(100.0f, 15.0f), Alignment.TopLeft, GetTextFuncForRace(), "CrimsonText-Regular-12", Color.Orange), Alignment.TopLeft, Alignment.TopLeft, new Point(0, -20));
            _frmMain["frmPopulation"].AddControl(new LabelSized("lblPopulationGrowth", new Vector2(100.0f, 15.0f), Alignment.TopRight, GetTextFuncForPopulationGrowth(), "CrimsonText-Regular-12", Color.Orange), Alignment.TopRight, Alignment.TopRight, new Point(0, -20));
            _frmMain["frmPopulation"].AddControl(new LabelSized("lblFarmers", new Vector2(100.0f, 15.0f), Alignment.TopLeft, "Farmers:", "CrimsonText-Regular-12", Color.Orange), Alignment.TopLeft, Alignment.TopLeft, new Point(20, 25));
            _frmMain["frmPopulation"].AddControl(new LabelSized("lblWorkers", new Vector2(100.0f, 15.0f), Alignment.TopLeft, "Workers:", "CrimsonText-Regular-12", Color.Orange), Alignment.TopLeft, Alignment.TopLeft, new Point(20, 55));
            _frmMain["frmPopulation"].AddControl(new LabelSized("lblRebels", new Vector2(100.0f, 15.0f), Alignment.TopLeft, "Rebels:", "CrimsonText-Regular-12", Color.Orange), Alignment.TopLeft, Alignment.TopLeft, new Point(20, 85));
            _frmMain["frmPopulation"].AddControl(new CitizenView("citizenView", new Vector2(130.0f, 135.0f), Alignment.TopLeft, settlementView, "Citizens"), Alignment.TopLeft, Alignment.TopLeft, new Point(130, 20));

            _frmMain.AddControl(new Frame("frmResources", new Vector2(515.0f, 175.0f), TextureAtlas, "frame2_whole", 50, 50, 50, 50), Alignment.TopCenter, Alignment.TopCenter, new Point(0, 190));
            _frmMain["frmResources"].AddControl(new LabelSized("lblResources", new Vector2(100.0f, 15.0f), Alignment.TopLeft, "Resources", "CrimsonText-Regular-12", Color.Orange, Color.DarkBlue), Alignment.TopLeft, Alignment.TopLeft, new Point(20, 0));
            _frmMain["frmResources"].AddControl(new LabelSized("lblFood", new Vector2(100.0f, 15.0f), Alignment.TopLeft, "Food", "CrimsonText-Regular-12", Color.Orange), Alignment.TopLeft, Alignment.TopLeft, new Point(20, 25));
            _frmMain["frmResources.lblFood"].AddControl(new FoodView("foodView", new Vector2(130.0f, 0.0f), Alignment.TopLeft, settlementView, "Bread", "Corn"), Alignment.TopLeft, Alignment.TopLeft, new Point(130, 0));
            _frmMain["frmResources"].AddControl(new LabelSized("lblProduction", new Vector2(100.0f, 15.0f), Alignment.TopLeft, "Production", "CrimsonText-Regular-12", Color.Orange), Alignment.TopLeft, Alignment.TopLeft, new Point(20, 55));
            _frmMain["frmResources.lblProduction"].AddControl(new FoodView("productionView", new Vector2(130.0f, 0.0f), Alignment.TopLeft, settlementView, "Anvil", "Pickaxe"), Alignment.TopLeft, Alignment.TopLeft, new Point(130, 0));
            _frmMain["frmResources"].AddControl(new LabelSized("lblGold", new Vector2(100.0f, 15.0f), Alignment.TopLeft, "Gold", "CrimsonText-Regular-12", Color.Orange), Alignment.TopLeft, Alignment.TopLeft, new Point(20, 85));
            _frmMain["frmResources"].AddControl(new LabelSized("lblPower", new Vector2(100.0f, 15.0f), Alignment.TopLeft, "Power", "CrimsonText-Regular-12", Color.Orange), Alignment.TopLeft, Alignment.TopLeft, new Point(20, 115));
            _frmMain["frmResources"].AddControl(new LabelSized("lblResearch", new Vector2(100.0f, 15.0f), Alignment.TopLeft, "Research", "CrimsonText-Regular-12", Color.Orange), Alignment.TopLeft, Alignment.TopLeft, new Point(20, 145));

            _frmMain.AddControl(new Frame("frmProducing", new Vector2(515.0f, 160.0f), TextureAtlas, "frame2_whole", 50, 50, 50, 50), Alignment.TopCenter, Alignment.TopCenter, new Point(0, 400));
            _frmMain["frmProducing"].AddControl(new LabelSized("lblProducing", new Vector2(100.0f, 15.0f), Alignment.TopLeft, "Producing", "CrimsonText-Regular-12", Color.Orange, Color.DarkBlue), Alignment.TopLeft, Alignment.TopLeft, new Point(20, 0));
            _frmMain["frmProducing"].AddControl(new LabelSized("lblCurrent", new Vector2(100.0f, 15.0f), Alignment.TopLeft, GetTextFuncForCurrentlyProducing, "CrimsonText-Regular-12", Color.Orange), Alignment.TopLeft, Alignment.TopLeft, new Point(20, 25));

            _frmMain.AddControl(new Frame("frmFooter", new Vector2(563.0f, 71.0f), TextureAtlas, "frame_bottom"), Alignment.BottomCenter, Alignment.BottomCenter, new Point(0, 5));
        }

        #region Funcs
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
        #endregion

        public override void LoadContent(ContentManager content, bool loadChildrenContent = false)
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