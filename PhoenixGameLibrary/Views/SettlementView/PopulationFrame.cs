using Microsoft.Xna.Framework;
using GuiControls;

namespace PhoenixGameLibrary.Views.SettlementView
{
    public class PopulationFrame
    {
        private readonly Settlement _settlement;

        private readonly Label _lblRace;
        private readonly Label _lblPopulationGrowth;
        private readonly SmallFrame _smallFrame;
        private readonly Label _lblFarmers1;
        private readonly Label _lblFarmers2;
        private readonly Label _lblWorkers1;
        private readonly Label _lblWorkers2;

        public PopulationFrame(Vector2 topLeftPosition, Settlement settlement)
        {
            _settlement = settlement;

            _lblRace = new Label("lblRace", "CrimsonText-Regular-12", topLeftPosition, HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Left, Color.Orange);
            _lblPopulationGrowth = new Label("lblPopulationGrowth", "CrimsonText-Regular-12", new Vector2(topLeftPosition.X + 516.0f, topLeftPosition.Y), HorizontalAlignment.Right, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Right, Color.Orange);

            _smallFrame = SmallFrame.Create(topLeftPosition + new Vector2(0, 10), new Vector2(500, 80), "GUI_Textures_1");
            //_smallFrame = SmallFrame.Create($"{{\"TopLeftPosition\":\"{topLeftPosition.X}, {topLeftPosition.Y + 10.0f}\",\"Size\":\"500, 80\",\"NumberOfSlots\":0,\"TextureString\":\"GUI_Textures_1\"}}");

            _lblFarmers1 = new Label("lblFarmers1", "CrimsonText-Regular-12", new Vector2(topLeftPosition.X + 20.0f, topLeftPosition.Y + 40.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Left, Color.Orange);
            _lblFarmers2 = new Label("lblFarmers2", "CrimsonText-Regular-12", new Vector2(topLeftPosition.X + 200.0f, topLeftPosition.Y + 40.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Right, Color.Orange);
            _lblWorkers1 = new Label("lblWorkers1", "CrimsonText-Regular-12", new Vector2(topLeftPosition.X + 20.0f, topLeftPosition.Y + 70.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Left, Color.Orange);
            _lblWorkers2 = new Label("lblWorkers2", "CrimsonText-Regular-12", new Vector2(topLeftPosition.X + 200.0f, topLeftPosition.Y + 70.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Right, Color.Orange);
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
            _lblRace.Text = $"{_settlement.RaceType.Name}";
            _lblPopulationGrowth.Text = $"Population: {_settlement.Population} (+{_settlement.GrowthRate})";
            _smallFrame.Update(gameTime, input);
            _lblFarmers1.Text = "Farmers:";
            _lblFarmers2.Text = $"{_settlement.Citizens.SubsistenceFarmers + _settlement.Citizens.AdditionalFarmers}";
            _lblWorkers1.Text = "Workers:";
            _lblWorkers2.Text = $"{_settlement.Citizens.Workers}";
        }

        public void Draw()
        {
            _lblRace.Draw();
            _lblPopulationGrowth.Draw();
            _smallFrame.Draw();
            _lblFarmers1.Draw();
            _lblFarmers2.Draw();
            _lblWorkers1.Draw();
            _lblWorkers2.Draw();
        }
    }
}