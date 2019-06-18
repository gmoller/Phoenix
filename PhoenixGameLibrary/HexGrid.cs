using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using Utilities;
using Microsoft.Xna.Framework;

namespace PhoenixGameLibrary
{
    public class HexGrid
    {
        private Camera _camera;

        private Texture2D _texture;
        private AtlasSpec2 _spec;

        private readonly Hex[,] _hexGrid;

        public HexGrid(int numberofcolumns, int numberOfRows)
        {
            _camera = new Camera(new Viewport(0, 0, 1500, 550));
            var map = MapGenerator.Generate(numberofcolumns, numberOfRows);

            float depth = 0.0f;
            _hexGrid = new Hex[numberofcolumns, numberOfRows];
            for (int r = 0; r < numberOfRows; ++r)
            {
                for (int q = 0; q < numberofcolumns; ++q)
                {
                    //_hexGrid[q, r] = new Hex(q, r, RandomNumberGenerator.Instance.GetRandomInt(0, 39), depth, _camera);
                    _hexGrid[q, r] = new Hex(q, r, map[q, r], depth, _camera);
                    depth += 0.0001f;
                }
            }
        }

        public void LoadContent()
        {
            _texture = AssetsManager.Instance.GetTexture("terrain_hextiles_basic_1");
            _spec = AssetsManager.Instance.GetAtlas("terrain_hextiles_basic_1");
        }

        public void Update(GameTime gameTime)
        {
            _camera.UpdateCamera(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var original = DeviceManager.Instance.GraphicsDevice.Viewport;
            DeviceManager.Instance.GraphicsDevice.Viewport = new Viewport(50, 50, 1500, 550, 0, 1);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, null, null, null, null, _camera.Transform);
            foreach (Hex hex in _hexGrid)
            {
                hex.Draw(spriteBatch, _texture, _spec, _camera);
            }
            spriteBatch.End();

            //spriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack, blendState: BlendState.NonPremultiplied);
            //foreach (Hex hex in _hexGrid)
            //{
            //    hex.DrawHexBorder(spriteBatch, _camera);
            //}
            //spriteBatch.End();

            DeviceManager.Instance.GraphicsDevice.Viewport = original;
        }
    }
}