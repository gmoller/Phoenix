using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using PhoenixGameLibrary;

namespace PhoenixGamePresentationLibrary
{
    public class UnitsView
    {
        private readonly WorldView _worldView;

        private Texture2D _texture;
        private readonly Units _units;

        public UnitsView(WorldView worldView, Units units)
        {
            _worldView = worldView;
            _units = units;
        }

        public void LoadContent(ContentManager content)
        {
            _texture = AssetsManager.Instance.GetTexture("brutal-helm");
        }

        internal void Update(InputHandler input, float deltaTime)
        {
            foreach (var unit in _units)
            {
                var unitView = new UnitView(_worldView, unit);
                unitView.Update(input, deltaTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var unit in _units)
            {
                var unitView = new UnitView(_worldView, unit);
                unitView.Draw(spriteBatch, _texture);
            }
        }
    }
}