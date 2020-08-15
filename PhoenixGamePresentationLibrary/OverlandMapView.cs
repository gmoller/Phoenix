﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using HexLibrary;
using Input;
using PhoenixGameLibrary;
using Utilities;
using Utilities.ExtensionMethods;

namespace PhoenixGamePresentationLibrary
{
    internal class OverlandMapView
    {
        private readonly WorldView _worldView;
        private readonly OverlandMap _overlandMap;

        internal OverlandMapView(WorldView worldView, OverlandMap overlandMap)
        {
            _worldView = worldView;
            _overlandMap = overlandMap;
        }

        internal void Update(InputHandler input, float deltaTime)
        {
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            DrawCellGrid(spriteBatch, _overlandMap.CellGrid, _worldView.Camera);
        }

        private void DrawCellGrid(SpriteBatch spriteBatch, CellGrid cellGrid, Camera camera)
        {
            var center = camera.ScreenToWorld(new Vector2(1920.0f / 2.0f, 1080.0f / 2.0f));
            var centerHex = HexOffsetCoordinates.FromPixel((int)center.X, (int)center.Y);

            var fromColumn = centerHex.Col - camera.NumberOfHexesToLeft;
            if (fromColumn < 0) fromColumn = 0;
            var toColumn = centerHex.Col + camera.NumberOfHexesToRight;
            if (toColumn > PhoenixGameLibrary.Constants.WORLD_MAP_COLUMNS) toColumn = PhoenixGameLibrary.Constants.WORLD_MAP_COLUMNS;

            var fromRow = centerHex.Row - camera.NumberOfHexesAbove;
            if (fromRow < 0) fromRow = 0;
            var toRow = centerHex.Row + camera.NumberOfHexesBelow;
            if (toRow > PhoenixGameLibrary.Constants.WORLD_MAP_ROWS) toRow = PhoenixGameLibrary.Constants.WORLD_MAP_ROWS;

            for (int r = fromRow; r <= toRow; ++r)
            {
                for (int q = fromColumn; q <= toColumn; ++q)
                {
                    var cell = cellGrid.GetCell(q, r);

                    if (cell.SeenState == SeenState.NeverSeen)
                    {
                        DrawCell(spriteBatch, cell, Color.White);
                    }
                    else if (cell.IsSeenByPlayer(_worldView.World))
                    {
                        DrawCell(spriteBatch, cell, Color.White);
                    }  
                    else
                    {
                        DrawCell(spriteBatch, cell, Color.DarkGray);
                    }

                    //DrawHexBorder(spriteBatch, cell);
                    DrawBorders(spriteBatch, cell);
                }
            }
        }

        private void DrawBorders(SpriteBatch spriteBatch, Cell cell)
        {
            if (cell.Borders.IsBitSet((byte)Direction.East)) DrawBorder(spriteBatch, cell, HexVertexDirection.NorthEast,  HexVertexDirection.SouthEast);
            if (cell.Borders.IsBitSet((byte)Direction.SouthEast)) DrawBorder(spriteBatch, cell, HexVertexDirection.SouthEast,  HexVertexDirection.South);
            if (cell.Borders.IsBitSet((byte)Direction.SouthWest)) DrawBorder(spriteBatch, cell, HexVertexDirection.South,  HexVertexDirection.SouthWest);
            if (cell.Borders.IsBitSet((byte)Direction.West)) DrawBorder(spriteBatch, cell, HexVertexDirection.SouthWest,  HexVertexDirection.NorthWest);
            if (cell.Borders.IsBitSet((byte)Direction.NorthWest)) DrawBorder(spriteBatch, cell, HexVertexDirection.NorthWest, HexVertexDirection.North);
            if (cell.Borders.IsBitSet((byte)Direction.NorthEast)) DrawBorder(spriteBatch, cell, HexVertexDirection.North, HexVertexDirection.NorthEast);
        }

        private void DrawBorder(SpriteBatch spriteBatch, Cell cell, HexVertexDirection vertexDirection1, HexVertexDirection vertexDirection2)
        {
            var centerPosition = HexOffsetCoordinates.ToPixel(cell.Column, cell.Row);
            var point1 = Hex.GetCorner(vertexDirection1);
            var point2 = Hex.GetCorner(vertexDirection2);

            spriteBatch.DrawLine(centerPosition + point1, centerPosition + point2, cell.ControlledByFaction == 1 ? Color.DarkBlue : Color.Red, 5.0f, 0.5f);
        }

        private void DrawCell(SpriteBatch spriteBatch, Cell cell, Color color)
        {
            //var terrainType = Globals.Instance.TerrainTypes[cell.TerrainTypeId];
            bool neverSeen = cell.SeenState == SeenState.NeverSeen;
            var texture = AssetsManager.Instance.GetTexture(neverSeen ? cell.TextureFogOfWar.TexturePalette  : cell.Texture.TexturePalette);
            var spec = AssetsManager.Instance.GetAtlas(neverSeen ? cell.TextureFogOfWar.TexturePalette : cell.Texture.TexturePalette);
            var frame = spec.Frames[neverSeen ? cell.TextureFogOfWar.TextureId : cell.Texture.TextureId];
            var sourceRectangle = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);

            var centerPosition = HexOffsetCoordinates.ToPixel(cell.Column, cell.Row);
            var destinationRectangle = new Rectangle((int)centerPosition.X, (int)centerPosition.Y, 111, 192);
            var layerDepth = cell.Index / 10000.0f;
            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color, 0.0f, Constants.HEX_ORIGIN, SpriteEffects.None, layerDepth);

            //var size = new Vector2(111, 192);
            //var imgTile = new Image("imgTile", centerPosition - PhoenixGameLibrary.Constants.HEX_ORIGIN / 2 + new Vector2(10.0f, 0.0f), ContentAlignment.MiddleCenter, size, cell.Texture.TexturePalette, cell.Texture.TextureId, layerDepth);
            //imgTile.Draw(spriteBatch);
        }

        private void DrawHexBorder(SpriteBatch spriteBatch, Cell cell)
        {
            var centerPosition = HexOffsetCoordinates.ToPixel(cell.Column, cell.Row);

            var color = Color.PeachPuff;
            var point0 = Hex.GetCorner(HexVertexDirection.North);
            var point1 = Hex.GetCorner(HexVertexDirection.NorthEast);
            var point2 = Hex.GetCorner(HexVertexDirection.SouthEast);
            var point3 = Hex.GetCorner(HexVertexDirection.South);
            var point4 = Hex.GetCorner(HexVertexDirection.SouthWest);
            var point5 = Hex.GetCorner(HexVertexDirection.NorthWest);

            spriteBatch.DrawLine(centerPosition + point0, centerPosition + point1, color, 1.0f);
            spriteBatch.DrawLine(centerPosition + point1, centerPosition + point2, color, 1.0f);
            spriteBatch.DrawLine(centerPosition + point2, centerPosition + point3, color, 1.0f);
            spriteBatch.DrawLine(centerPosition + point3, centerPosition + point4, color, 1.0f);
            spriteBatch.DrawLine(centerPosition + point4, centerPosition + point5, color, 1.0f);
            spriteBatch.DrawLine(centerPosition + point5, centerPosition + point0, color, 1.0f);
        }
    }
}