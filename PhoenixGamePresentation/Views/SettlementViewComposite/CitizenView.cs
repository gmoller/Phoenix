using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GuiControls;
using GuiControls.PackagesClasses;
using Input;
using Utilities;

namespace PhoenixGamePresentation.Views.SettlementViewComposite
{
    internal class CitizenView : Control
    {
        #region State
        private readonly SettlementView _settlementView;

        private Button _btnSubtractFarmer;
        private Button _btnAddFarmer;
        private Button _btnSubtractWorker;
        private Button _btnAddWorker;

        private IControl _imgFarmer;
        private IControl _imgWorker;
        private IControl _imgRebel;
        #endregion State

        internal CitizenView(string name, Vector2 position, Alignment positionAlignment, SettlementView settlementView, string textureAtlas) :
            base(position, positionAlignment, new Vector2(100.0f, 30.0f), name)
        {
            _settlementView = settlementView;

            _btnSubtractFarmer = new Button(position + new Vector2(Area.X, Area.Y), Alignment.TopLeft, new Vector2(19.0f, 19.0f), "GUI_Textures_1.minus_n", "GUI_Textures_1.minus_a", "GUI_Textures_1.minus_h", "GUI_Textures_1.minus_a", "btnSubtractFarmer");
            _btnSubtractFarmer.AddPackage(new ControlClick((o, args) => SubtractFarmerButtonClick(o, new EventArgs())));
            _btnAddFarmer = new Button(position + new Vector2(Area.X + 20.0f, Area.Y), Alignment.TopLeft, new Vector2(19.0f, 19.0f), "GUI_Textures_1.plus_n", "GUI_Textures_1.plus_a", "GUI_Textures_1.plus_h", "GUI_Textures_1.plus_a", "btnAddFarmer");
            _btnAddFarmer.AddPackage(new ControlClick((o, args) => AddFarmerButtonClick(o, new EventArgs())));
            _btnSubtractWorker = new Button(position + new Vector2(Area.X, Area.Y + 30.0f), Alignment.TopLeft, new Vector2(19.0f, 19.0f), "GUI_Textures_1.minus_n", "GUI_Textures_1.minus_a", "GUI_Textures_1.minus_h", "GUI_Textures_1.minus_a", "btnSubtractWorker");
            _btnSubtractWorker.AddPackage(new ControlClick((o, args) => SubtractWorkerButtonClick(o, new EventArgs())));
            _btnAddWorker = new Button(position + new Vector2(Area.X + 20.0f, Area.Y + 30.0f), Alignment.TopLeft, new Vector2(19.0f, 19.0f), "GUI_Textures_1.plus_n", "GUI_Textures_1.plus_a", "GUI_Textures_1.plus_h", "GUI_Textures_1.plus_a", "btnAddWorker");
            _btnAddWorker.AddPackage(new ControlClick((o, args) => AddWorkerButtonClick(o, new EventArgs())));

            _imgFarmer = new Image(Vector2.Zero, Alignment.TopLeft, new Vector2(20.0f, 30.0f), $"{textureAtlas}.Citizen_{settlementView.Settlement.RaceType.Name}_Farmer", "imgFarmer");
            _imgWorker = new Image(Vector2.Zero, Alignment.TopLeft, new Vector2(20.0f, 30.0f), $"{textureAtlas}.Citizen_{settlementView.Settlement.RaceType.Name}_Worker", "imgWorker");
            _imgRebel = new Image(Vector2.Zero, Alignment.TopLeft, new Vector2(20.0f, 30.0f), $"{textureAtlas}.Citizen_{settlementView.Settlement.RaceType.Name}_Rebel", "imgRebel");
        }

        public override void LoadContent(ContentManager content, bool loadChildrenContent = false)
        {
            _btnSubtractFarmer.LoadContent(content);
            _btnAddFarmer.LoadContent(content);
            _btnSubtractWorker.LoadContent(content);
            _btnAddWorker.LoadContent(content);

            _imgFarmer.LoadContent(content);
            _imgWorker.LoadContent(content);
            _imgRebel.LoadContent(content);
        }

        public override void Update(InputHandler input, float deltaTime, Viewport? viewport)
        {
            EnableOrDisableButtons();

            _btnSubtractFarmer.Update(input, deltaTime, viewport);
            _btnAddFarmer.Update(input, deltaTime, viewport);
            _btnSubtractWorker.Update(input, deltaTime, viewport);
            _btnAddWorker.Update(input, deltaTime, viewport);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            float offsetX = 70.0f;
            float offsetY = -5.0f;
            DrawCitizens(spriteBatch, new Vector2(Area.X + offsetX, Area.Y + offsetY), _imgFarmer, _settlementView.Settlement.Citizens.SubsistenceFarmers);
            DrawCitizens(spriteBatch, new Vector2(Area.X + offsetX + _settlementView.Settlement.Citizens.SubsistenceFarmers * _imgFarmer.Width + _imgFarmer.Width, Area.Y + offsetY), _imgFarmer, _settlementView.Settlement.Citizens.AdditionalFarmers);
            DrawCitizens(spriteBatch, new Vector2(Area.X + offsetX, Area.Y + offsetY + _imgFarmer.Height), _imgWorker, _settlementView.Settlement.Citizens.Workers);
            DrawCitizens(spriteBatch, new Vector2(Area.X + offsetX, Area.Y + offsetY + _imgFarmer.Height + _imgWorker.Height), _imgRebel, 0); //_settlement.Citizens.Rebels

            _btnSubtractFarmer.Draw(spriteBatch);
            _btnAddFarmer.Draw(spriteBatch);
            _btnSubtractWorker.Draw(spriteBatch);
            _btnAddWorker.Draw(spriteBatch);
        }

        private void DrawCitizens(SpriteBatch spriteBatch, Vector2 position, IControl image, int citizenCount)
        {
            var x = (int)position.X;
            var y = (int)position.Y;
            for (var i = 0; i < citizenCount; ++i)
            {
                image.SetTopLeftPosition(new PointI(x, y));
                image.Draw(spriteBatch);

                x += image.Width;
            }
        }

        private void SubtractFarmerButtonClick(object sender, EventArgs e)
        {
            _settlementView.Settlement.Citizens.ConvertFarmerToWorker();
        }

        private void AddFarmerButtonClick(object sender, EventArgs e)
        {
            _settlementView.Settlement.Citizens.ConvertWorkerToFarmer();
        }

        private void SubtractWorkerButtonClick(object sender, EventArgs e)
        {
            _settlementView.Settlement.Citizens.ConvertWorkerToFarmer();
        }

        private void AddWorkerButtonClick(object sender, EventArgs e)
        {
            _settlementView.Settlement.Citizens.ConvertFarmerToWorker();
        }

        private void EnableOrDisableButtons()
        {
            if (_settlementView.Settlement == null) return;

            _btnSubtractFarmer.Enabled = _settlementView.Settlement.Citizens.AdditionalFarmers > 0;
            _btnAddFarmer.Enabled = _settlementView.Settlement.Citizens.Workers > 0;
            _btnSubtractWorker.Enabled = _settlementView.Settlement.Citizens.Workers > 0;
            _btnAddWorker.Enabled = _settlementView.Settlement.Citizens.AdditionalFarmers > 0;
        }
    }
}