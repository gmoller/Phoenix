using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using Utilities;

namespace PhoenixGameLibrary
{
    public class HexGrid
    {
        private Texture2D _texture;
        private AtlasSpec2 _spec;

        private readonly Hex[,] _hexGrid;

        public HexGrid(int numberofcolumns, int numberOfRows)
        {
            _hexGrid = new Hex[numberofcolumns, numberOfRows];
            for (int q = 0; q < numberofcolumns; ++q)
            {
                for (int r = 0; r < numberOfRows; ++r)
                {
                    _hexGrid[q, r] = new Hex(q, r, RandomNumberGenerator.Instance.GetRandomInt(0, 15));
                }
            }
        }

        public void LoadContent()
        {
            _texture = AssetsManager.Instance.GetTexture("fantasyhextiles_v4");
            _spec = AssetsManager.Instance.GetAtlas("fantasyhextiles_v4");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Hex hex in _hexGrid)
            {
                hex.Draw(spriteBatch, _texture, _spec);
            }
        }
    }
}