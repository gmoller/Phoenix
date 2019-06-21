using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PhoenixGameLibrary.GameData;
using Utilities;

namespace PhoenixGameLibrary
{
    public class HexGrid
    {
        private readonly Camera _camera;
        private readonly TerrainTypes _terrainTypes;

        private readonly int _numberOfColumns;
        private readonly int _numberOfRows;
        private readonly Hex[,] _hexGrid;

        public HexGrid(int numberOfColumns, int numberOfRows)
        {
            _camera = new Camera(new Viewport(0, 0, 1500, 755)); // 550
            _terrainTypes = TerrainTypes.Create(TerrainTypesLoader.GetTerrainTypes());
            var map = MapGenerator.Generate(numberOfColumns, numberOfRows, _terrainTypes);

            _numberOfColumns = numberOfColumns;
            _numberOfRows = numberOfRows;
            _hexGrid = new Hex[numberOfColumns, numberOfRows];
            for (int r = 0; r < numberOfRows; ++r)
            {
                for (int q = 0; q < numberOfColumns; ++q)
                {
                    _hexGrid[q, r] = new Hex(q, r, map[q, r], _camera);
                }
            }
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
            _camera.UpdateCamera(gameTime, input);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var original = DeviceManager.Instance.GraphicsDevice.Viewport;
            DeviceManager.Instance.GraphicsDevice.Viewport = DeviceManager.Instance.MapViewport;

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, null, null, null, null, _camera.Transform);
            var depth = 0.0f;

            for (int r = 0; r < _numberOfRows; ++r)
            {
                for (int q = 0; q < _numberOfColumns; ++q)
                {
                    var hex = _hexGrid[q, r];
                    hex.Draw(spriteBatch, _camera, depth, _terrainTypes);
                    depth += 0.0001f;
                }
            }

            spriteBatch.End();

            //spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, null, null, null, null, _camera.Transform);

            //for (int r = 0; r < _numberOfRows; ++r)
            //{
            //    for (int q = 0; q < _numberOfColumns; ++q)
            //    {
            //        var hex = _hexGrid[q, r];
            //        hex.DrawHexBorder(spriteBatch, q, r, _camera);
            //        depth += 0.0001f;
            //    }
            //}

            //spriteBatch.End();

            DeviceManager.Instance.GraphicsDevice.Viewport = original;
        }
    }
}