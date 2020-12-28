using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Zen.GuiControls;
using Zen.GuiControls.TheControls;
using Zen.Input;
using Zen.MonoGameUtilities.ExtensionMethods;
using Zen.Utilities;

namespace PhoenixGamePresentation.Views.SettlementViewComposite
{
    internal class MainFrame : Control
    {
        #region State
        private SettlementView SettlementView { get; }
        private Controls Controls { get; }
        #endregion State

        public MainFrame(SettlementView settlementView, Vector2 topLeftPosition, string textureAtlas) :
            base("MainFrame")
        {
            Size = new PointI(556, 741);
            Position = topLeftPosition.ToPointI();

            SettlementView = settlementView;

            var pairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("textureName1", $"{textureAtlas}.frame_main"),
                new KeyValuePair<string, string>("textureName2", $"{textureAtlas}.frame_big_heading"),
                new KeyValuePair<string, string>("textureName3", $"{textureAtlas}.frame2_whole"),
                new KeyValuePair<string, string>("textureName4", $"{textureAtlas}.frame_bottom"),
                new KeyValuePair<string, string>("position1", $"{Convert.ToInt32(topLeftPosition.X)};{Convert.ToInt32(topLeftPosition.Y)}"),
                new KeyValuePair<string, string>("size1", $"{Size.X};{Size.Y}"),
                new KeyValuePair<string, string>("textureNormal1", $"{textureAtlas}.close_button_n"),
                new KeyValuePair<string, string>("textureActive1", $"{textureAtlas}.close_button_a"),
                new KeyValuePair<string, string>("textureHover1", $"{textureAtlas}.close_button_h"),
            };

            var spec = File.ReadAllText(@".\Views\SettlementViewComposite\MainFrameControls.txt");
            Controls = ControlCreator.CreateFromSpecification(spec, pairs);
            Controls.SetOwner(this);
            Controls["frmMain.frmPopulation"].AddControl(new CitizenView("citizenView", new Vector2(90.0f, 135.0f), Alignment.TopLeft, settlementView, "Citizens"));
            Controls["frmMain.frmResources"].AddControl(new FoodView("foodView", new Vector2(130.0f, 0.0f), Alignment.TopLeft, settlementView, "Bread", "Corn"), Alignment.TopLeft, Alignment.TopLeft, new PointI(130, 20));
            Controls["frmMain.frmResources"].AddControl(new ProductionView("productionView", new Vector2(130.0f, 0.0f), Alignment.TopLeft, settlementView, "Anvil", "Pickaxe"), Alignment.TopLeft, Alignment.TopLeft, new PointI(130, 50));
        }

        public override IControl Clone()
        {
            throw new NotImplementedException();
        }

        public override void LoadContent(ContentManager content, bool loadChildrenContent = false)
        {
            Controls.LoadContent(content, true);
        }

        public override void Update(InputHandler input, GameTime gameTime, Viewport? viewport)
        {
            Controls.Update(input, gameTime, viewport);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Controls.Draw(spriteBatch);
        }

        public static string GetTextFuncForRace(object sender)
        {
            var label = (Label)sender;
            var mainFrame = (MainFrame)label.Owner;
            var settlementView = mainFrame.SettlementView;
            return $"{settlementView.Settlement.RaceType.Name}";
        }

        public static string GetTextFuncForPopulationGrowth(object sender)
        {
            var label = (Label)sender;
            var mainFrame = (MainFrame)label.Owner;
            var settlementView = mainFrame.SettlementView;
            return $"Population: {settlementView.Settlement.Population} (+{settlementView.Settlement.GrowthRate})";
        }

        public static string GetTextFuncForSettlementType(object sender)
        {
            var label = (Label)sender;
            var mainFrame = (MainFrame)label.Owner;
            var settlementView = mainFrame.SettlementView;
            return $"{settlementView.Settlement.SettlementType} of";
        }

        public static string GetTextFuncForSettlementName(object sender)
        {
            var label = (Label)sender;
            var mainFrame = (MainFrame)label.Owner;
            var settlementView = mainFrame.SettlementView;
            return $"{settlementView.Settlement.Name}";
        }

        public static string GetTextFuncForCurrentlyProducing(object sender)
        {
            var label = (Label)sender;
            var mainFrame = (MainFrame)label.Owner;
            var settlementView = mainFrame.SettlementView;
            return $"{settlementView.Settlement.CurrentlyBuilding}";
        }

        public static void CloseButtonClick(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var mainFrame = (MainFrame)button.Owner;
            var settlementView = mainFrame.SettlementView;
            settlementView.CloseButtonClick(sender, e);
        }
    }
}