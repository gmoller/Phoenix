using System;
using System.Collections.Generic;
using System.Reflection;
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
    internal class CitizenView : Control
    {
        #region State
        private SettlementView SettlementView { get; }
        private Controls Controls { get; }
        #endregion State

        internal CitizenView(string name, Vector2 position, Alignment positionAlignment, SettlementView settlementView, string textureAtlas) :
            base(name)
        {
            Size = new PointI(100, 30);
            PositionAlignment = positionAlignment;
            Position = position.ToPointI();

            SettlementView = settlementView;

            var kvps = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("position1", $"{Convert.ToInt32(position.X + Bounds.X)};{Convert.ToInt32(position.Y + Bounds.Y)}"),
                new KeyValuePair<string, string>("position2", $"{Convert.ToInt32(position.X + Bounds.X + 20)};{Convert.ToInt32(position.Y + Bounds.Y)}"),
                new KeyValuePair<string, string>("position3", $"{Convert.ToInt32(position.X + Bounds.X)};{Convert.ToInt32(position.Y + Bounds.Y + 30)}"),
                new KeyValuePair<string, string>("position4", $"{Convert.ToInt32(position.X + Bounds.X + 20)};{Convert.ToInt32(position.Y + Bounds.Y + 30)}"),
                new KeyValuePair<string, string>("textureName1", $"{textureAtlas}.Citizen_{settlementView.Settlement.RaceType.Name}_Farmer"),
                new KeyValuePair<string, string>("textureName2", $"{textureAtlas}.Citizen_{settlementView.Settlement.RaceType.Name}_Worker"),
                new KeyValuePair<string, string>("textureName3", $"{textureAtlas}.Citizen_{settlementView.Settlement.RaceType.Name}_Rebel")
            };

            //var spec = File.ReadAllText(path);
            var spec = ResourceReader.ReadResource("PhoenixGamePresentation.Views.SettlementViewComposite.CitizenViewControls.txt", Assembly.GetExecutingAssembly());
            Controls = ControlCreator.CreateFromSpecification(spec, kvps);
            Controls.SetOwner(this);
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
            EnableOrDisableButtons();

            Controls.Update(input, gameTime, viewport);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var imgFarmer = Controls["imgFarmer"];
            var imgWorker = Controls["imgWorker"];
            var imgRebel = Controls["imgRebel"];

            var offsetX = 200.0f;
            var offsetY = 15.0f;
            DrawCitizens(spriteBatch, new Vector2(Bounds.X + offsetX, Bounds.Y + offsetY), imgFarmer, SettlementView.Settlement.Citizens.SubsistenceFarmers);
            DrawCitizens(spriteBatch, new Vector2(Bounds.X + offsetX + SettlementView.Settlement.Citizens.SubsistenceFarmers * imgFarmer.Width + imgFarmer.Width, Bounds.Y + offsetY), imgFarmer, SettlementView.Settlement.Citizens.AdditionalFarmers);
            DrawCitizens(spriteBatch, new Vector2(Bounds.X + offsetX, Bounds.Y + offsetY + imgWorker.Height), imgWorker, SettlementView.Settlement.Citizens.Workers);
            DrawCitizens(spriteBatch, new Vector2(Bounds.X + offsetX, Bounds.Y + offsetY + imgRebel.Height + imgRebel.Height), imgRebel, 0); //_settlement.Citizens.Rebels

            Controls.Draw(spriteBatch);
        }

        private void DrawCitizens(SpriteBatch spriteBatch, Vector2 position, IControl image, int citizenCount)
        {
            image.Visible = true;

            var x = (int)position.X;
            var y = (int)position.Y;
            for (var i = 0; i < citizenCount; i++)
            {
                image.Position = new PointI(x, y);
                image.Draw(spriteBatch);

                x += image.Width;
            }

            image.Visible = false;
        }

        private void EnableOrDisableButtons()
        {
            if (SettlementView.Settlement == null) return;

            Controls["btnSubtractFarmer"].Enabled = SettlementView.Settlement.Citizens.AdditionalFarmers > 0;
            Controls["btnAddFarmer"].Enabled = SettlementView.Settlement.Citizens.Workers > 0;
            Controls["btnSubtractWorker"].Enabled = SettlementView.Settlement.Citizens.Workers > 0;
            Controls["btnAddWorker"].Enabled = SettlementView.Settlement.Citizens.AdditionalFarmers > 0;
        }

        public static void SubtractFarmerButtonClick(object sender, EventArgs e)
        {
            var button = (Zen.GuiControls.TheControls.Button)sender;
            var citizenView = (CitizenView)button.Owner;
            var settlementView = citizenView.SettlementView;
            settlementView.Settlement.Citizens.ConvertFarmerToWorker();
        }

        public static void AddFarmerButtonClick(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var citizenView = (CitizenView)button.Owner;
            var settlementView = citizenView.SettlementView;
            settlementView.Settlement.Citizens.ConvertWorkerToFarmer();
        }

        public static void SubtractWorkerButtonClick(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var citizenView = (CitizenView)button.Owner;
            var settlementView = citizenView.SettlementView;
            settlementView.Settlement.Citizens.ConvertWorkerToFarmer();
        }

        public static void AddWorkerButtonClick(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var citizenView = (CitizenView)button.Owner;
            var settlementView = citizenView.SettlementView;
            settlementView.Settlement.Citizens.ConvertFarmerToWorker();
        }
    }
}