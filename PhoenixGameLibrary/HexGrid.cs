using Microsoft.Xna.Framework.Graphics;
using Utilities;
using Microsoft.Xna.Framework;
using PhoenixGameLibrary.GameData;

namespace PhoenixGameLibrary
{
    public class HexGrid
    {
        private Camera _camera;

        private TerrainTypes _terrainTypes;

        private readonly Hex[,] _hexGrid;

        public HexGrid(int numberofcolumns, int numberOfRows)
        {
            _camera = new Camera(new Viewport(0, 0, 1500, 550));
            _terrainTypes = TerrainTypes.Create(TerrainTypesLoader.GetTerrainTypes());
            var map = MapGenerator.Generate(numberofcolumns, numberOfRows, _terrainTypes);

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

        public void Update(GameTime gameTime)
        {
            _camera.UpdateCamera(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var original = DeviceManager.Instance.GraphicsDevice.Viewport;
            DeviceManager.Instance.GraphicsDevice.Viewport = DeviceManager.Instance.MapViewport;

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, null, null, null, null, _camera.Transform);
            foreach (Hex hex in _hexGrid)
            {
                hex.Draw(spriteBatch, _camera);
            }
            spriteBatch.End();

            //spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, null, null, null, null, _camera.Transform);
            //foreach (Hex hex in _hexGrid)
            //{
            //    hex.DrawHexBorder(spriteBatch, _camera);
            //}
            //spriteBatch.End();

            DeviceManager.Instance.GraphicsDevice.Viewport = original;
        }
    }
}