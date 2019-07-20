using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using GuiControls;
using PhoenixGameLibrary;

namespace PhoenixGamePresentationLibrary.SettlementView
{
    internal class PopulationFrame
    {
        private readonly SettlementView _parent;
        private readonly Vector2 _topLeftPosition;

        private readonly Label _lblRace;
        private readonly Label _lblPopulationGrowth;
        private readonly FrameDynamicSizing _smallFrame;
        private readonly Label _lblFarmers1;
        private readonly Label _lblWorkers1;
        private readonly Label _lblRebels1;

        private Button _btnSubtractFarmer;
        private Button _btnAddFarmer;
        private Button _btnSubtractWorker;
        private Button _btnAddWorker;

        internal PopulationFrame(SettlementView parent, Vector2 topLeftPosition)
        {
            _parent = parent;
            _topLeftPosition = topLeftPosition;

            _lblRace = new Label("lblRace", "CrimsonText-Regular-12", topLeftPosition, HorizontalAlignment.Left, VerticalAlignment.Middle, Vector2.Zero, string.Empty, HorizontalAlignment.Left, Color.Orange);
            _lblPopulationGrowth = new Label("lblPopulationGrowth", "CrimsonText-Regular-12", new Vector2(topLeftPosition.X + 516.0f, topLeftPosition.Y), HorizontalAlignment.Right, VerticalAlignment.Middle, Vector2.Zero, "Population: 0", HorizontalAlignment.Right, Color.Orange);

            _smallFrame = new FrameDynamicSizing(topLeftPosition + new Vector2(0.0f, 10.0f), new Vector2(515, 120), "GUI_Textures_1", "frame2_whole", 50, 50, 50, 50);

            _lblFarmers1 = new Label("lblFarmers1", "CrimsonText-Regular-12", new Vector2(topLeftPosition.X + 20.0f, topLeftPosition.Y + 30.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, "Farmers:", HorizontalAlignment.Left, Color.Orange);
            _lblWorkers1 = new Label("lblWorkers1", "CrimsonText-Regular-12", new Vector2(topLeftPosition.X + 20.0f, topLeftPosition.Y + 60.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, "Workers:", HorizontalAlignment.Left, Color.Orange);
            _lblRebels1 = new Label("lblRebels1", "CrimsonText-Regular-12", new Vector2(topLeftPosition.X + 20.0f, topLeftPosition.Y + 90.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, "Rebels:", HorizontalAlignment.Left, Color.Orange);

            _btnSubtractFarmer = new Button("btnSubtractFarmer", new Vector2(topLeftPosition.X + 140.0f, topLeftPosition.Y + 25.0f), HorizontalAlignment.Left, VerticalAlignment.Top, new Vector2(19.0f, 19.0f), "GUI_Textures_1", "minus_n", "minus_a", "minus_a", "minus_h");
            _btnSubtractFarmer.Click += btnSubtractFarmerClick;
            _btnAddFarmer = new Button("btnAddFarmer", new Vector2(topLeftPosition.X + 160.0f, topLeftPosition.Y + 25.0f), HorizontalAlignment.Left, VerticalAlignment.Top, new Vector2(19.0f, 19.0f), "GUI_Textures_1", "plus_n", "plus_a", "plus_a", "plus_h");
            _btnAddFarmer.Click += btnAddFarmerClick;
            _btnSubtractWorker = new Button("btnSubtractWorker", new Vector2(topLeftPosition.X + 140.0f, topLeftPosition.Y + 55.0f), HorizontalAlignment.Left, VerticalAlignment.Top, new Vector2(19.0f, 19.0f), "GUI_Textures_1", "minus_n", "minus_a", "minus_a", "minus_h");
            _btnSubtractWorker.Click += btnSubtractWorkerClick;
            _btnAddWorker = new Button("btnAddWorker", new Vector2(topLeftPosition.X + 160.0f, topLeftPosition.Y + 55.0f), HorizontalAlignment.Left, VerticalAlignment.Top, new Vector2(19.0f, 19.0f), "GUI_Textures_1", "plus_n", "plus_a", "plus_a", "plus_h");
            _btnAddWorker.Click += btnAddWorkerClick;

            EnableOrDisableButtons();
        }

        internal void Update(GameTime gameTime, InputHandler input)
        {
            _lblRace.Text = $"{_parent.Settlement.RaceType.Name}";
            _lblPopulationGrowth.Text = $"Population: {_parent.Settlement.Population} (+{_parent.Settlement.GrowthRate})";

            _btnSubtractFarmer.Update(gameTime);
            _btnAddFarmer.Update(gameTime);
            _btnSubtractWorker.Update(gameTime);
            _btnAddWorker.Update(gameTime);

            EnableOrDisableButtons();
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            _lblRace.Draw();
            _lblPopulationGrowth.Draw();
            _smallFrame.Draw();
            _lblFarmers1.Draw();
            _lblWorkers1.Draw();
            _lblRebels1.Draw();

            spriteBatch.Begin();
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
            var texture = AssetsManager.Instance.GetTexture("Citizens");
            var atlas = AssetsManager.Instance.GetAtlas("Citizens");

            int x = (int)position.X;
            int y = (int)position.Y;
            var sourceRectangle = atlas.Frames[$"Citizen_{raceTypeName}_{citizenType}"].ToRectangle();
            for (int i = 0; i < citizenCount; ++i)
            {
                var destinationRectangle = new Rectangle(x, y, sourceRectangle.Width, sourceRectangle.Height);
                spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White);
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