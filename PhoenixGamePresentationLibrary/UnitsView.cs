using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using PhoenixGameLibrary;

namespace PhoenixGamePresentationLibrary
{
    public class UnitsView
    {
        private Texture2D _texture;
        private readonly Units _units;

        public UnitsView(Units units)
        {
            _units = units;
        }

        public void LoadContent(ContentManager content)
        {
            _texture = AssetsManager.Instance.GetTexture("brutal-helm");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var unit in _units)
            {
                var unitView = new UnitView(unit);
                unitView.Draw(spriteBatch, _texture);
            }
        }
    }
}