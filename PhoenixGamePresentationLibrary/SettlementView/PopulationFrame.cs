﻿using System;
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

        internal PopulationFrame(SettlementView parent, Vector2 topLeftPosition)
        {
            _parent = parent;
            _topLeftPosition = topLeftPosition;
        }

        internal void LoadContent(ContentManager content)
        {
            _lblRace = new LabelAutoSized(_topLeftPosition, Alignment.MiddleLeft, string.Empty, "CrimsonText-Regular-12", Color.Orange);
            _lblRace.LoadContent(content);
            _lblPopulationGrowth = new LabelAutoSized(new Vector2(_topLeftPosition.X + 516.0f, _topLeftPosition.Y), Alignment.MiddleRight, "Population: 0", "CrimsonText-Regular-12", Color.Orange);
            _lblPopulationGrowth.LoadContent(content);

            _smallFrame = new Frame(_topLeftPosition + new Vector2(0.0f, 10.0f), Alignment.TopLeft, new Vector2(515, 120), "GUI_Textures_1", "frame2_whole", 50, 50, 50, 50);
            _smallFrame.LoadContent(content);

            _lblFarmers = new LabelAutoSized(new Vector2(_topLeftPosition.X + 20.0f, _topLeftPosition.Y + 25.0f), Alignment.TopLeft, "Farmers:", "CrimsonText-Regular-12", Color.Orange);
            _lblFarmers.LoadContent(content);
            _lblWorkers = new LabelAutoSized(new Vector2(_topLeftPosition.X + 20.0f, _topLeftPosition.Y + 55.0f), Alignment.TopLeft, "Workers:", "CrimsonText-Regular-12", Color.Orange);
            _lblWorkers.LoadContent(content);
            _lblRebels = new LabelAutoSized(new Vector2(_topLeftPosition.X + 20.0f, _topLeftPosition.Y + 85.0f), Alignment.TopLeft, "Rebels:", "CrimsonText-Regular-12", Color.Orange);
            _lblRebels.LoadContent(content);

            _btnSubtractFarmer = new Button(new Vector2(_topLeftPosition.X + 140.0f, _topLeftPosition.Y + 25.0f), Alignment.TopLeft, new Vector2(19.0f, 19.0f), "GUI_Textures_1", "minus_n", "minus_a", "minus_a", "minus_h");
            _btnSubtractFarmer.LoadContent(content);
            _btnSubtractFarmer.Click += btnSubtractFarmerClick;
            _btnAddFarmer = new Button(new Vector2(_topLeftPosition.X + 160.0f, _topLeftPosition.Y + 25.0f), Alignment.TopLeft, new Vector2(19.0f, 19.0f), "GUI_Textures_1", "plus_n", "plus_a", "plus_a", "plus_h");
            _btnAddFarmer.LoadContent(content);
            _btnAddFarmer.Click += btnAddFarmerClick;
            _btnSubtractWorker = new Button(new Vector2(_topLeftPosition.X + 140.0f, _topLeftPosition.Y + 55.0f), Alignment.TopLeft, new Vector2(19.0f, 19.0f), "GUI_Textures_1", "minus_n", "minus_a", "minus_a", "minus_h");
            _btnSubtractWorker.LoadContent(content);
            _btnSubtractWorker.Click += btnSubtractWorkerClick;
            _btnAddWorker = new Button(new Vector2(_topLeftPosition.X + 160.0f, _topLeftPosition.Y + 55.0f), Alignment.TopLeft, new Vector2(19.0f, 19.0f), "GUI_Textures_1", "plus_n", "plus_a", "plus_a", "plus_h");
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
            _lblFarmers.Draw();
            _lblWorkers.Draw();
            _lblRebels.Draw();

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
            var x = (int)position.X;
            var y = (int)position.Y;
            var image = new Image(Vector2.Zero, Alignment.TopLeft, new Vector2(20, 30), "Citizens", $"Citizen_{raceTypeName}_{citizenType}");
            image.LoadContent(_content);
            for (var i = 0; i < citizenCount; ++i)
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