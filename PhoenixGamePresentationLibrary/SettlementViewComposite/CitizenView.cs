using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GuiControls;
using Input;

namespace PhoenixGamePresentationLibrary.SettlementViewComposite
{
    internal class CitizenView : Control
    {
        #region State
        private readonly SettlementView _settlementView;

        private Button _btnSubtractFarmer;
        private Button _btnAddFarmer;
        private Button _btnSubtractWorker;
        private Button _btnAddWorker;

        private Image _imgFarmer;
        private Image _imgWorker;
        private Image _imgRebel;
        #endregion

        internal CitizenView(Vector2 position, Alignment positionAlignment, SettlementView settlementView, string textureAtlas, string name, IControl parent = null) :
            base(position, positionAlignment, new Vector2(100.0f, 30.0f), textureAtlas, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, name, 0.0f, parent)
        {
            _settlementView = settlementView;

            _btnSubtractFarmer = new Button(new Vector2(Area.X, Area.Y), Alignment.TopLeft, new Vector2(19.0f, 19.0f), "GUI_Textures_1", "minus_n", "minus_a", "minus_a", "minus_h", "btnSubtractFarmer");
            _btnSubtractFarmer.Click += SubtractFarmerButtonClick;
            _btnAddFarmer = new Button(new Vector2(Area.X + 20.0f, Area.Y), Alignment.TopLeft, new Vector2(19.0f, 19.0f), "GUI_Textures_1", "plus_n", "plus_a", "plus_a", "plus_h", "btnAddFarmer");
            _btnAddFarmer.Click += AddFarmerButtonClick;
            _btnSubtractWorker = new Button(new Vector2(Area.X, Area.Y + 30.0f), Alignment.TopLeft, new Vector2(19.0f, 19.0f), "GUI_Textures_1", "minus_n", "minus_a", "minus_a", "minus_h", "btnSubtractWorker");
            _btnSubtractWorker.Click += SubtractWorkerButtonClick;
            _btnAddWorker = new Button(new Vector2(Area.X + 20.0f, Area.Y + 30.0f), Alignment.TopLeft, new Vector2(19.0f, 19.0f), "GUI_Textures_1", "plus_n", "plus_a", "plus_a", "plus_h", "btnAddWorker");
            _btnAddWorker.Click += AddWorkerButtonClick;

            _imgFarmer = new Image(Vector2.Zero, Alignment.TopLeft, new Vector2(20.0f, 30.0f), textureAtlas, $"Citizen_{settlementView.Settlement.RaceType.Name}_Farmer", "imgFarmer");
            _imgWorker = new Image(Vector2.Zero, Alignment.TopLeft, new Vector2(20.0f, 30.0f), textureAtlas, $"Citizen_{settlementView.Settlement.RaceType.Name}_Worker", "imgWorker");
            _imgRebel = new Image(Vector2.Zero, Alignment.TopLeft, new Vector2(20.0f, 30.0f), textureAtlas, $"Citizen_{settlementView.Settlement.RaceType.Name}_Rebel", "imgRebel");
        }

        public override void LoadContent(ContentManager content)
        {
            _btnSubtractFarmer.LoadContent(content);
            _btnAddFarmer.LoadContent(content);
            _btnSubtractWorker.LoadContent(content);
            _btnAddWorker.LoadContent(content);

            _imgFarmer.LoadContent(content);
            _imgWorker.LoadContent(content);
            _imgRebel.LoadContent(content);
        }

        public override void Update(InputHandler input, float deltaTime, Matrix? transform = null)
        {
            EnableOrDisableButtons();

            _btnSubtractFarmer.Update(input, deltaTime);
            _btnAddFarmer.Update(input, deltaTime);
            _btnSubtractWorker.Update(input, deltaTime);
            _btnAddWorker.Update(input, deltaTime);
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

        private void DrawCitizens(SpriteBatch spriteBatch, Vector2 position, Image image, int citizenCount)
        {
            var x = (int)position.X;
            var y = (int)position.Y;
            for (var i = 0; i < citizenCount; ++i)
            {
                image.SetTopLeftPosition(x, y);
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