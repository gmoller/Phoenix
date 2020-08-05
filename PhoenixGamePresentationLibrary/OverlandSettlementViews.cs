using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Input;
using PhoenixGameLibrary;

namespace PhoenixGamePresentationLibrary
{
    public class OverlandSettlementViews
    {
        private readonly WorldView _worldView;
        private readonly Settlements _settlements;

        private OverlandSettlementView _overlandSettlementView;

        public OverlandSettlementViews(WorldView worldView, Settlements settlements)
        {
            _worldView = worldView;
            _settlements = settlements;
        }

        public void LoadContent(ContentManager content)
        {
            _overlandSettlementView = new OverlandSettlementView(_worldView);
            _overlandSettlementView.LoadContent(content);
        }

        public void Update(InputHandler input, float deltaTime)
        {
            foreach (var settlement in _settlements)
            {
                _overlandSettlementView.Settlement = settlement;
                _overlandSettlementView.Update(input, deltaTime);
            }
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