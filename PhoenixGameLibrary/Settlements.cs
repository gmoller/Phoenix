using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;

namespace PhoenixGameLibrary
{
    public class Settlements
    {
        private Texture2D _texture;

        public void LoadContent(ContentManager content)
        {
            AssetsManager.Instance.AddTexture("VillageSmall00", "Textures\\villageSmall00");
            _texture = AssetsManager.Instance.GetTexture("VillageSmall00");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}