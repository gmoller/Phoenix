using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GuiControls;
using Input;

namespace PhoenixGamePresentationLibrary.SettlementView
{
    internal class PopulationFrame
    {
        private readonly SettlementView _parent;
        private ContentManager _content;

        private readonly Vector2 _topLeftPosition;

        private Label _lblRace;
        private Label _lblPopulationGrowth;
        private Frame _smallFrame;
        private LabelOld _lblFarmers1;
        private LabelOld _lblWorkers1;
        private LabelOld _lblRebels1;

        private Button _btnSubtractFarmer;
        private Button _btnAddFarmer;
        private Button _btnSubtractWorker;
        private Button _btnAddWorker;

        internal PopulationFrame(SettlementView parent, Vector2 topLeftPosition)
        {
            _parent = parent;
            _topLeftPosition = topLeftPosition;
        }

        internal void LoadContent(ContentManager content)
        {
            _lblRace = new Label("lblRace", _topLeftPosition, Alignment.MiddleLeft, string.Empty, "CrimsonText-Regular-12", Color.Orange);
            _lblRace.LoadContent(content);
            _lblPopulationGrowth = new Label("lblPopulationGrowth", new Vector2(_topLeftPosition.X + 516.0f, _topLeftPosition.Y), Alignment.MiddleRight, "Population: 0", "CrimsonText-Regular-12", Color.Orange);
            _lblPopulationGrowth.LoadContent(content);

            _smallFrame = new Frame("SmallFrame", _topLeftPosition + new Vector2(0.0f, 10.0f), Alignment.TopLeft, new Vector2(515, 120), "GUI_Textures_1", "frame2_whole", 50, 50, 50, 50);
            _smallFrame.LoadContent(content);

            _lblFarmers1 = new LabelOld("lblFarmers1", "CrimsonText-Regular-12", new Vector2(_topLeftPosition.X + 20.0f, _topLeftPosition.Y + 30.0f), HorizontalAlignment.Left, VerticalAlignment.Top, new Vector2(10, 10), "Farmers:", HorizontalAlignment.Left, Color.Orange);
            _lblWorkers1 = new LabelOld("lblWorkers1", "CrimsonText-Regular-12", new Vector2(_topLeftPosition.X + 20.0f, _topLeftPosition.Y + 60.0f), HorizontalAlignment.Left, VerticalAlignment.Top, new Vector2(10, 10), "Workers:", HorizontalAlignment.Left, Color.Orange);
            _lblRebels1 = new LabelOld("lblRebels1", "CrimsonText-Regular-12", new Vector2(_topLeftPosition.X + 20.0f, _topLeftPosition.Y + 90.0f), HorizontalAlignment.Left, VerticalAlignment.Top, new Vector2(10, 10), "Rebels:", HorizontalAlignment.Left, Color.Orange);

            _btnSubtractFarmer = new Button("btnSubtractFarmer", new Vector2(_topLeftPosition.X + 140.0f, _topLeftPosition.Y + 25.0f), Alignment.TopLeft, new Vector2(19.0f, 19.0f), "GUI_Textures_1", "minus_n", "minus_a", "minus_a", "minus_h");
            _btnSubtractFarmer.LoadContent(content);
            _btnSubtractFarmer.Click += btnSubtractFarmerClick;
            _btnAddFarmer = new Button("btnAddFarmer", new Vector2(_topLeftPosition.X + 160.0f, _topLeftPosition.Y + 25.0f), Alignment.TopLeft, new Vector2(19.0f, 19.0f), "GUI_Textures_1", "plus_n", "plus_a", "plus_a", "plus_h");
            _btnAddFarmer.LoadContent(content);
            _btnAddFarmer.Click += btnAddFarmerClick;
            _btnSubtractWorker = new Button("btnSubtractWorker", new Vector2(_topLeftPosition.X + 140.0f, _topLeftPosition.Y + 55.0f), Alignment.TopLeft, new Vector2(19.0f, 19.0f), "GUI_Textures_1", "minus_n", "minus_a", "minus_a", "minus_h");
            _btnSubtractWorker.LoadContent(content);
            _btnSubtractWorker.Click += btnSubtractWorkerClick;
            _btnAddWorker = new Button("btnAddWorker", new Vector2(_topLeftPosition.X + 160.0f, _topLeftPosition.Y + 55.0f), Alignment.TopLeft, new Vector2(19.0f, 19.0f), "GUI_Textures_1", "plus_n", "plus_a", "plus_a", "plus_h");
            _btnAddWorker.LoadContent(content);
            _btnAddWorker.Click += btnAddWorkerClick;

            EnableOrDisableButtons();

            _content = content;
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
            spriteBatch.Begin();
            _lblRace.Draw();
            _lblPopulationGrowth.Draw();
            _smallFrame.Draw();
            _lblFarmers1.Draw();
            _lblWorkers1.Draw();
            _lblRebels1.Draw();

            DrawCitizens(spriteBatch, new Vector2(_topLeftPosition.X + 200, _topLeftPosition.Y + 20), _parent.Settlement.RaceType.Name, "Farmer", _parent.Settlement.Citizens.SubsistenceFarmers);
            DrawCitizens(spriteBatch, new Vector2(_topLeftPosition.X + 200 + (_parent.Settlement.Citizens.SubsistenceFarmers * 20) + 20, _topLeftPosition.Y + 20), _parent.Settlement.RaceType.Name, "Farmer", _parent.Settlement.Citizens.AdditionalFarmers);
            DrawCitizens(spriteBatch, new Vector2(_topLeftPosition.X + 200, _topLeftPosition.Y + 50), _parent.Settlement.RaceType.Name, "Worker", _parent.Settlement.Citizens.Workers);
            DrawCitizens(spriteBatch, new Vector2(_topLeftPosition.X + 200, _topLeftPosition.Y + 80), _parent.Settlement.RaceType.Name, "Rebel", 0); //_settlement.Citizens.Rebels
            spriteBatch.End();

            _btnSubtractFarmer.Draw();
            _btnAddFarmer.Draw();
            _btnSubtractWorker.Draw();
            _btnAddWorker.Draw();
        }

        internal void DrawCitizens(SpriteBatch spriteBatch, Vector2 position, string raceTypeName, string citizenType, int citizenCount)
        {
            int x = (int)position.X;
            int y = (int)position.Y;
            var image = new Image("Image", Vector2.Zero, Alignment.TopLeft, new Vector2(20, 30), "Citizens", $"Citizen_{raceTypeName}_{citizenType}");
            image.LoadContent(_content);
            for (int i = 0; i < citizenCount; ++i)
            {
                image.SetTopLeftPosition(x, y);
                image.Draw();

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
            if (_parent.Settlement != null)
            {
                _btnSubtractFarmer.Enabled = _parent.Settlement.Citizens.AdditionalFarmers > 0;
                _btnAddFarmer.Enabled = _parent.Settlement.Citizens.Workers > 0;
                _btnSubtractWorker.Enabled = _parent.Settlement.Citizens.Workers > 0;
                _btnAddWorker.Enabled = _parent.Settlement.Citizens.AdditionalFarmers > 0;
            }
        }
    }
}