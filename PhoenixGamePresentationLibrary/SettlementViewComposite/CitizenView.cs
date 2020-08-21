using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GuiControls;
using Input;

namespace PhoenixGamePresentationLibrary.SettlementViewComposite
{
    internal class CitizenView
    {
        private readonly SettlementView _parent;

        private readonly Vector2 _topLeftPosition;

        private Button _btnSubtractFarmer;
        private Button _btnAddFarmer;
        private Button _btnSubtractWorker;
        private Button _btnAddWorker;

        private Image _imgFarmer;
        private Image _imgWorker;
        private Image _imgRebel;

        internal CitizenView(SettlementView parent, Vector2 topLeftPosition)
        {
            _parent = parent;
            _topLeftPosition = topLeftPosition;
        }

        internal void LoadContent(ContentManager content)
        {
            _btnSubtractFarmer = new Button(new Vector2(_topLeftPosition.X + 140.0f, _topLeftPosition.Y + 30.0f), Alignment.TopLeft, new Vector2(19.0f, 19.0f), "GUI_Textures_1", "minus_n", "minus_a", "minus_a", "minus_h", "btnSubtractFarmer");
            _btnSubtractFarmer.LoadContent(content);
            _btnSubtractFarmer.Click += SubtractFarmerButtonClick;
            _btnAddFarmer = new Button(new Vector2(_topLeftPosition.X + 160.0f, _topLeftPosition.Y + 30.0f), Alignment.TopLeft, new Vector2(19.0f, 19.0f), "GUI_Textures_1", "plus_n", "plus_a", "plus_a", "plus_h", "btnAddFarmer");
            _btnAddFarmer.LoadContent(content);
            _btnAddFarmer.Click += AddFarmerButtonClick;
            _btnSubtractWorker = new Button(new Vector2(_topLeftPosition.X + 140.0f, _topLeftPosition.Y + 60.0f), Alignment.TopLeft, new Vector2(19.0f, 19.0f), "GUI_Textures_1", "minus_n", "minus_a", "minus_a", "minus_h", "btnSubtractWorker");
            _btnSubtractWorker.LoadContent(content);
            _btnSubtractWorker.Click += SubtractWorkerButtonClick;
            _btnAddWorker = new Button(new Vector2(_topLeftPosition.X + 160.0f, _topLeftPosition.Y + 60.0f), Alignment.TopLeft, new Vector2(19.0f, 19.0f), "GUI_Textures_1", "plus_n", "plus_a", "plus_a", "plus_h", "btnAddWorker");
            _btnAddWorker.LoadContent(content);
            _btnAddWorker.Click += AddWorkerButtonClick;

            // TODO: fix hardcoded race type name
            _imgFarmer = new Image(Vector2.Zero, Alignment.TopLeft, new Vector2(20, 30), "Citizens", "Citizen_Barbarians_Farmer", "imgFarmer");
            _imgFarmer.LoadContent(content);
            _imgWorker = new Image(Vector2.Zero, Alignment.TopLeft, new Vector2(20, 30), "Citizens", "Citizen_Barbarians_Worker", "imgWorker");
            _imgWorker.LoadContent(content);
            _imgRebel = new Image(Vector2.Zero, Alignment.TopLeft, new Vector2(20, 30), "Citizens", "Citizen_Barbarians_Rebel", "imgRebel");
            _imgRebel.LoadContent(content);

            EnableOrDisableButtons();
        }

        internal void Update(InputHandler input, float deltaTime)
        {
            _btnSubtractFarmer.Update(input, deltaTime);
            _btnAddFarmer.Update(input, deltaTime);
            _btnSubtractWorker.Update(input, deltaTime);
            _btnAddWorker.Update(input, deltaTime);

            EnableOrDisableButtons();
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            DrawCitizens(spriteBatch, new Vector2(_topLeftPosition.X + 200, _topLeftPosition.Y + 25), _imgFarmer, _parent.Settlement.Citizens.SubsistenceFarmers);
            DrawCitizens(spriteBatch, new Vector2(_topLeftPosition.X + 200 + (_parent.Settlement.Citizens.SubsistenceFarmers * 20) + 20, _topLeftPosition.Y + 25), _imgFarmer, _parent.Settlement.Citizens.AdditionalFarmers);
            DrawCitizens(spriteBatch, new Vector2(_topLeftPosition.X + 200, _topLeftPosition.Y + 55), _imgWorker, _parent.Settlement.Citizens.Workers);
            DrawCitizens(spriteBatch, new Vector2(_topLeftPosition.X + 200, _topLeftPosition.Y + 85), _imgRebel, 0); //_settlement.Citizens.Rebels

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

                x += 20;
            }
        }

        private void SubtractFarmerButtonClick(object sender, EventArgs e)
        {
            _parent.Settlement.Citizens.ConvertFarmerToWorker();
        }

        private void AddFarmerButtonClick(object sender, EventArgs e)
        {
            _parent.Settlement.Citizens.ConvertWorkerToFarmer();
        }

        private void SubtractWorkerButtonClick(object sender, EventArgs e)
        {
            _parent.Settlement.Citizens.ConvertWorkerToFarmer();
        }

        private void AddWorkerButtonClick(object sender, EventArgs e)
        {
            _parent.Settlement.Citizens.ConvertFarmerToWorker();
        }

        private void EnableOrDisableButtons()
        {
            if (_parent.Settlement == null) return;

            _btnSubtractFarmer.Enabled = _parent.Settlement.Citizens.AdditionalFarmers > 0;
            _btnAddFarmer.Enabled = _parent.Settlement.Citizens.Workers > 0;
            _btnSubtractWorker.Enabled = _parent.Settlement.Citizens.Workers > 0;
            _btnAddWorker.Enabled = _parent.Settlement.Citizens.AdditionalFarmers > 0;
        }
    }
}