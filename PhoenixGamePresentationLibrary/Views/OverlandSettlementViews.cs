using Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PhoenixGameLibrary;

namespace PhoenixGamePresentationLibrary.Views
{
    public class OverlandSettlementViews
    {
        private readonly WorldView _worldView;
        private readonly Settlements _settlements;

        private readonly OverlandSettlementView _overlandSettlementView;

        public OverlandSettlementViews(WorldView worldView, Settlements settlements)
        {
            _worldView = worldView;
            _settlements = settlements;

            _overlandSettlementView = new OverlandSettlementView(_worldView);
        }

        public void LoadContent(ContentManager content)
        {
            _overlandSettlementView.LoadContent(content);
        }

        public void Update(InputHandler input, float deltaTime)
        {
            if (_worldView.GameStatus != GameStatus.OverlandMap) return;

            // Causes

            // Actions

            foreach (var settlement in _settlements)
            {
                _overlandSettlementView.Settlement = settlement;
                _overlandSettlementView.Update(input, deltaTime);
            }

            // Status change?
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var settlement in _settlements)
            {
                _overlandSettlementView.Settlement = settlement;
                _overlandSettlementView.Draw(spriteBatch);
            }
        }
    }
}