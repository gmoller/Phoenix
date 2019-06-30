using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using GameLogic;
using GuiControls;
using HexLibrary;
using Utilities;

namespace PhoenixGameLibrary.Views
{
    public class OverlandMapView
    {
        private OverlandMap _overlandMap;

        private Button _btnEndTurn;

        public bool IsEnabled { get; set; }

        public OverlandMapView(OverlandMap overlandMap)
        {
            _overlandMap = overlandMap;
        }

        public void LoadContent(ContentManager content)
        {
            var pos = new Vector2(DeviceManager.Instance.MapViewport.X + DeviceManager.Instance.MapViewport.Width, DeviceManager.Instance.MapViewport.Y + DeviceManager.Instance.MapViewport.Height);
            var label = new Label("lblNextTurn", "CrimsonText-Regular-12", pos, HorizontalAlignment.Right, VerticalAlignment.Bottom, new Vector2(245.0f, 56.0f), "Next Turn", HorizontalAlignment.Center, Color.White, Color.Blue);
            _btnEndTurn = new Button("btnEndTurn", pos, HorizontalAlignment.Right, VerticalAlignment.Bottom, new Vector2(245.0f, 56.0f), "GUI_Textures_1", "reg_button_n", "reg_button_a", "reg_button_h", label);
            _btnEndTurn.Click += btnEndTurnClick;
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
            //UpdateCellGrid(gameTime, input);
            _btnEndTurn.Update(gameTime);
        }

        public void Draw()
        {
            DrawCellGrid(_overlandMap.CellGrid);
            _btnEndTurn.Draw();
        }

        private void DrawCellGrid(CellGrid cellGrid)
        {
            DeviceManager.Instance.SetViewport(DeviceManager.Instance.MapViewport);

            var spriteBatch = DeviceManager.Instance.GetCurrentSpriteBatch();

            var camera = Globals.Instance.World.Camera;
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, null, null, null, null, camera.Transform);
            var depth = 0.0f;

            for (int r = 0; r < cellGrid.NumberOfRows; ++r)
            {
                for (int q = 0; q < cellGrid.NumberOfColumns; ++q)
                {
                    var cell = cellGrid.GetCell(q, r);

                    var centerPosition = HexOffsetCoordinates.OffsetCoordinatesToPixel(cell.Column, cell.Row);
                    if (camera.VisibleArea.Contains(centerPosition))
                    {
                        DrawCell(cell, depth);
                        depth += 0.0001f;
                    }
                }
            }

            spriteBatch.End();

            //spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, null, null, null, null, camera.Transform);

            //for (int r = 0; r < _numberOfRows; ++r)
            //{
            //    for (int q = 0; q < _numberOfColumns; ++q)
            //    {
            //        var cell = _cellGrid[q, r];
            //        DrawHexBorder(cell);
            //    }
            //}

            //spriteBatch.End();

            DeviceManager.Instance.ResetViewport();
        }

        private void DrawCell(Cell cell, float layerDepth)
        {
            var centerPosition = HexOffsetCoordinates.OffsetCoordinatesToPixel(cell.Column, cell.Row);

            var spriteBatch = DeviceManager.Instance.GetCurrentSpriteBatch();

            var terrainType = Globals.Instance.TerrainTypes[cell.TerrainTypeId];
            var texture = AssetsManager.Instance.GetTexture(cell.Texture.TexturePalette);
            var spec = AssetsManager.Instance.GetAtlas(cell.Texture.TexturePalette);
            var frame = spec.Frames[cell.Texture.TextureId];
            var sourceRectangle = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);

            //spriteBatch.Draw(texture, centerPosition, sourceRectangle, Color.White, 0.0f, Constants.HEX_ORIGIN, Constants.HEX_SCALE, SpriteEffects.None, layerDepth);

            var destinationRectangle = new Rectangle((int)centerPosition.X, (int)centerPosition.Y, 111, 192);
            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White, 0.0f, Constants.HEX_ORIGIN, SpriteEffects.None, layerDepth);
        }

        private void DrawHexHorder(Cell cell)
        {
            var centerPosition = HexOffsetCoordinates.OffsetCoordinatesToPixel(cell.Column, cell.Row);

            var spriteBatch = DeviceManager.Instance.GetCurrentSpriteBatch();

            var color = Color.PeachPuff;
            var point0 = GetHexCorner(5);
            var point1 = GetHexCorner(0);
            var point2 = GetHexCorner(1);
            var point3 = GetHexCorner(2);
            var point4 = GetHexCorner(3);
            var point5 = GetHexCorner(4);

            spriteBatch.DrawLine(centerPosition + point0, centerPosition + point1, color);
            spriteBatch.DrawLine(centerPosition + point1, centerPosition + point2, color);
            spriteBatch.DrawLine(centerPosition + point2, centerPosition + point3, color);
            spriteBatch.DrawLine(centerPosition + point3, centerPosition + point4, color);
            spriteBatch.DrawLine(centerPosition + point4, centerPosition + point5, color);
            spriteBatch.DrawLine(centerPosition + point5, centerPosition + point0, color);
        }

        private Vector2 GetHexCorner(int i)
        {
            float degrees = 60 * i - 30;
            float radians = MathHelper.ToRadians(degrees);

            var v = new Vector2((float)(HexLibrary.Constants.HEX_SIZE * Math.Cos(radians)), (float)(HexLibrary.Constants.HEX_SIZE * Math.Sin(radians)));

            return v;
        }

        private void btnEndTurnClick(object sender, EventArgs e)
        {
            _overlandMap.EndTurn();
        }
    }
}