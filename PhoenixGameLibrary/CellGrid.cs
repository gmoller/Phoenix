using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PhoenixGameLibrary.GameData;
using Utilities;

namespace PhoenixGameLibrary
{
    public class CellGrid
    {
        private readonly Camera _camera;
        private readonly TerrainTypes _terrainTypes;

        private readonly int _numberOfColumns;
        private readonly int _numberOfRows;
        private readonly Cell[,] _cellGrid;

        public CellGrid(int numberOfColumns, int numberOfRows, Camera camera)
        {
            _camera = camera;
            _terrainTypes = TerrainTypes.Create(TerrainTypesLoader.GetTerrainTypes());
            var map = MapGenerator.Generate(numberOfColumns, numberOfRows, _terrainTypes);

            _numberOfColumns = numberOfColumns;
            _numberOfRows = numberOfRows;
            _cellGrid = new Cell[numberOfColumns, numberOfRows];
            for (int r = 0; r < numberOfRows; ++r)
            {
                for (int q = 0; q < numberOfColumns; ++q)
                {
                    _cellGrid[q, r] = new Cell(q, r, map[q, r], _camera);
                }
            }
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
        }

        public void Draw()
        {
            DeviceManager.Instance.SetViewport(DeviceManager.Instance.MapViewport);

            var spriteBatch = DeviceManager.Instance.GetCurrentSpriteBatch();

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, null, null, null, null, _camera.Transform);
            var depth = 0.0f;

            for (int r = 0; r < _numberOfRows; ++r)
            {
                for (int q = 0; q < _numberOfColumns; ++q)
                {
                    var cell = _cellGrid[q, r];
                    cell.Draw(_camera, depth, _terrainTypes);
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
            //        hex.DrawHexBorder(_camera);
            //        depth += 0.0001f;
            //    }
            //}

            //spriteBatch.End();

            DeviceManager.Instance.ResetViewport();
        }

        public Cell GetCell(int col, int row)
        {
            return _cellGrid[col, row];
        }
    }
}