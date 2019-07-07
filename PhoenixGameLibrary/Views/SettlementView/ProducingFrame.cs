using Microsoft.Xna.Framework;
using GameLogic;
using GuiControls;

namespace PhoenixGameLibrary.Views.SettlementView
{
    internal class ProducingFrame
    {
        private Settlement _settlement;

        private readonly SmallFrame _smallFrame;
        private readonly Label _lblProducing;
        private readonly Label _lblCurrent;

        public ProducingFrame(Vector2 topLeftPosition, Settlement settlement)
        {
            _settlement = settlement;

            _smallFrame = SmallFrame.Create(topLeftPosition + new Vector2(0, 10), new Vector2(500, 160), "GUI_Textures_1");
            _lblProducing = new Label("lblProducing", "CrimsonText-Regular-12", topLeftPosition, HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Left, Color.Orange, Color.Red);
            _lblCurrent = new Label("lblCurrent", "CrimsonText-Regular-12", new Vector2(topLeftPosition.X + 20.0f, topLeftPosition.Y + 40.0f), HorizontalAlignment.Left, VerticalAlignment.Top, Vector2.Zero, string.Empty, HorizontalAlignment.Left, Color.Orange);
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
            _lblProducing.Text = "Producing";
            if (_settlement.CurrentlyBuilding.BuildingId == -1)
            {
                _lblCurrent.Text = "Current: <nothing>";
            }
            else
            {
                var building = Globals.Instance.BuildingTypes[_settlement.CurrentlyBuilding.BuildingId];
                _lblCurrent.Text = $"Current: {building.Name} ({_settlement.CurrentlyBuilding.ProductionAccrued}/{building.ConstructionCost})";
            }
        }

        public void Draw()
        {
            _smallFrame.Draw();
            _lblProducing.Draw();
            _lblCurrent.Draw();
        }
    }
}