using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GuiControls;
using Input;

namespace PhoenixGamePresentationLibrary.SettlementViewComposite
{
    internal class PopulationFrame
    {
        private readonly SettlementView _parent;

        private readonly Vector2 _topLeftPosition;

        private LabelAutoSized _lblRace;
        private LabelAutoSized _lblPopulationGrowth;
        private Frame _smallFrame;
        private LabelAutoSized _lblFarmers;
        private LabelAutoSized _lblWorkers;
        private LabelAutoSized _lblRebels;

        private Button _btnSubtractFarmer;
        private Button _btnAddFarmer;
        private Button _btnSubtractWorker;
        private Button _btnAddWorker;

        private Image _imgFarmer;
        private Image _imgWorker;
        private Image _imgRebel;

        internal PopulationFrame(SettlementView parent, Vector2 topLeftPosition)
        {
            _parent = parent;
            _topLeftPosition = topLeftPosition;
        }

        internal void LoadContent(ContentManager content)
        {
            _lblRace = new LabelAutoSized(_topLeftPosition, Alignment.MiddleLeft, string.Empty, "CrimsonText-Regular-12", Color.Orange, "lblRace");
            _lblRace.LoadContent(content);
            _lblPopulationGrowth = new LabelAutoSized(new Vector2(_topLeftPosition.X + 516.0f, _topLeftPosition.Y), Alignment.MiddleRight, "Population: 0", "CrimsonText-Regular-12", Color.Orange, "lblPopulationGrowth");
            _lblPopulationGrowth.LoadContent(content);

            _smallFrame = new Frame(_topLeftPosition + new Vector2(0.0f, 10.0f), Alignment.TopLeft, new Vector2(515, 120), "GUI_Textures_1", "frame2_whole", 50, 50, 50, 50, "smallFrame");
            _smallFrame.LoadContent(content);

            _lblFarmers = new LabelAutoSized(new Vector2(_topLeftPosition.X + 20.0f, _topLeftPosition.Y + 25.0f), Alignment.TopLeft, "Farmers:", "CrimsonText-Regular-12", Color.Orange, "lblFarmers");
            _lblFarmers.LoadContent(content);
            _lblWorkers = new LabelAutoSized(new Vector2(_topLeftPosition.X + 20.0f, _topLeftPosition.Y + 55.0f), Alignment.TopLeft, "Workers:", "CrimsonText-Regular-12", Color.Orange, "lblWorkers");
            _lblWorkers.LoadContent(content);
            _lblRebels = new LabelAutoSized(new Vector2(_topLeftPosition.X + 20.0f, _topLeftPosition.Y + 85.0f), Alignment.TopLeft, "Rebels:", "CrimsonText-Regular-12", Color.Orange, "lblRebels");
            _lblRebels.LoadContent(content);

            _btnSubtractFarmer = new Button(new Vector2(_topLeftPosition.X + 140.0f, _topLeftPosition.Y + 25.0f), Alignment.TopLeft, new Vector2(19.0f, 19.0f), "GUI_Textures_1", "minus_n", "minus_a", "minus_a", "minus_h", "btnSubtractFarmer");
            _btnSubtractFarmer.LoadContent(content);
            _btnSubtractFarmer.Click += btnSubtractFarmerClick;
            _btnAddFarmer = new Button(new Vector2(_topLeftPosition.X + 160.0f, _topLeftPosition.Y + 25.0f), Alignment.TopLeft, new Vector2(19.0f, 19.0f), "GUI_Textures_1", "plus_n", "plus_a", "plus_a", "plus_h", "btnAddFarmer");
            _btnAddFarmer.LoadContent(content);
            _btnAddFarmer.Click += btnAddFarmerClick;
            _btnSubtractWorker = new Button(new Vector2(_topLeftPosition.X + 140.0f, _topLeftPosition.Y + 55.0f), Alignment.TopLeft, new Vector2(19.0f, 19.0f), "GUI_Textures_1", "minus_n", "minus_a", "minus_a", "minus_h", "btnSubtractWorker");
            _btnSubtractWorker.LoadContent(content);
            _btnSubtractWorker.Click += btnSubtractWorkerClick;
            _btnAddWorker = new Button(new Vector2(_topLeftPosition.X + 160.0f, _topLeftPosition.Y + 55.0f), Alignment.TopLeft, new Vector2(19.0f, 19.0f), "GUI_Textures_1", "plus_n", "plus_a", "plus_a", "plus_h", "btnAddWorker");
            _btnAddWorker.LoadContent(content);
            _btnAddWorker.Click += btnAddWorkerClick;

            _imgFarmer = new Image(Vector2.Zero, Alignment.TopLeft, new Vector2(20, 30), "Citizens", $"Citizen_{_parent.Settlement.RaceType.Name}_Farmer", "imgFarmer");
            _imgFarmer.LoadContent(content);
            _imgWorker = new Image(Vector2.Zero, Alignment.TopLeft, new Vector2(20, 30), "Citizens", $"Citizen_{_parent.Settlement.RaceType.Name}_Worker", "imgWorker");
            _imgWorker.LoadContent(content);
            _imgRebel = new Image(Vector2.Zero, Alignment.TopLeft, new Vector2(20, 30), "Citizens", $"Citizen_{_parent.Settlement.RaceType.Name}_Rebel", "imgRebel");
            _imgRebel.LoadContent(content);

            EnableOrDisableButtons();
        }

        internal void Update(InputHandler input, float deltaTime)
        {
            _lblRace.Text = $"{_parent.Settlement.RaceType.Name}";
            _lblPopulationGrowth.Text = $"Population: {_parent.Settlement.Population} (+{_parent.Settlement.GrowthRate})";

            _btnSubtractFarmer.Update(input, deltaTime);
            _btnAddFarmer.Update(input, deltaTime);
            _btnSubtractWorker.Update(input, deltaTime);
            _btnAddWorker.Update(input, deltaTime);

            EnableOrDisableButtons();
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            _lblRace.Draw(spriteBatch);
            _lblPopulationGrowth.Draw(spriteBatch);
            _smallFrame.Draw(spriteBatch);
            _lblFarmers.Draw(spriteBatch);
            _lblWorkers.Draw(spriteBatch);
            _lblRebels.Draw(spriteBatch);

            DrawCitizens(spriteBatch, new Vector2(_topLeftPosition.X + 200, _topLeftPosition.Y + 20), _imgFarmer, _parent.Settlement.Citizens.SubsistenceFarmers);
            DrawCitizens(spriteBatch, new Vector2(_topLeftPosition.X + 200 + (_parent.Settlement.Citizens.SubsistenceFarmers * 20) + 20, _topLeftPosition.Y + 20), _imgFarmer, _parent.Settlement.Citizens.AdditionalFarmers);
            DrawCitizens(spriteBatch, new Vector2(_topLeftPosition.X + 200, _topLeftPosition.Y + 50), _imgWorker, _parent.Settlement.Citizens.Workers);
            DrawCitizens(spriteBatch, new Vector2(_topLeftPosition.X + 200, _topLeftPosition.Y + 80), _imgRebel, 0); //_settlement.Citizens.Rebels

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

        private void btnSubtractFarmerClick(object sender, EventArgs e)
        {
            _parent.Settlement.Citizens.ConvertFarmerToWorker();
        }

        private void btnAddFarmerClick(object sender, EventArgs e)
        {
            _parent.Settlement.Citizens.ConvertWorkerToFarmer();
        }

        private void btnSubtractWorkerClick(object sender, EventArgs e)
        {
            _parent.Settlement.Citizens.ConvertWorkerToFarmer();
        }

        private void btnAddWorkerClick(object sender, EventArgs e)
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