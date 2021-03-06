﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PhoenixGameLibrary;
using Zen.Assets;
using Zen.Input;
using Zen.MonoGameUtilities;
using Zen.Utilities.ExtensionMethods;
using Zen.Hexagons;

namespace PhoenixGamePresentation.Views
{
    internal class OverlandMapView : ViewBase, IDisposable
    {
        #region State
        private OverlandMap OverlandMap { get; }
        #endregion

        internal OverlandMapView(WorldView worldView, OverlandMap overlandMap, InputHandler input)
        {
            WorldView = worldView;
            OverlandMap = overlandMap;

            SetupViewport(0, 0, WorldView.Camera.GetViewport.Width, WorldView.Camera.GetViewport.Height);

            Input = input;
        }

        internal void LoadContent(ContentManager content)
        {
        }

        internal void Update(float deltaTime)
        {
        }

        internal void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            var originalViewport = spriteBatch.GraphicsDevice.Viewport;
            spriteBatch.GraphicsDevice.Viewport = Viewport;
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.Transform * ViewportAdapter.GetScaleMatrix()); // FrontToBack

            DrawCellGrid(spriteBatch, OverlandMap.CellGrid, WorldView.Camera);

            spriteBatch.End();
            spriteBatch.GraphicsDevice.Viewport = originalViewport;
        }

        private void DrawCellGrid(SpriteBatch spriteBatch, CellGrid cellGrid, Camera camera)
        {
            var fromRow = camera.CameraTopLeftHex.Y - 1;
            var toRow = camera.CameraBottomRightHex.Y + 1;

            var fromColumn = camera.CameraTopLeftHex.X - 1;
            var toColumn = camera.CameraBottomRightHex.X + 1;

            for (var r = fromRow; r <= toRow; r++)
            {
                for (var q = fromColumn; q <= toColumn; q++)
                {
                    var cell = cellGrid.GetCell(q, r);

                    if (cell.SeenState == SeenState.NeverSeen)
                    {
                        DrawCell(spriteBatch, cell, Color.White);
                    }
                    else if (cell.IsSeenByPlayer(WorldView.Settlements, WorldView.Stacks))
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
            var points = WorldView.HexLibrary.GetCorners();

            if (WorldView.HexLibrary.HexType == HexType.PointyTopped)
            {
                if (cell.Borders.IsBitSet((byte)Direction.NorthEast)) DrawBorder(spriteBatch, cell, new Vector2(points.Point0.X, points.Point0.Y), new Vector2(points.Point1.X, points.Point1.Y));
                if (cell.Borders.IsBitSet((byte)Direction.East)) DrawBorder(spriteBatch, cell, new Vector2(points.Point1.X, points.Point1.Y), new Vector2(points.Point2.X, points.Point2.Y));
                if (cell.Borders.IsBitSet((byte)Direction.SouthEast)) DrawBorder(spriteBatch, cell, new Vector2(points.Point2.X, points.Point2.Y), new Vector2(points.Point3.X, points.Point3.Y));
                if (cell.Borders.IsBitSet((byte)Direction.SouthWest)) DrawBorder(spriteBatch, cell, new Vector2(points.Point3.X, points.Point3.Y), new Vector2(points.Point4.X, points.Point4.Y));
                if (cell.Borders.IsBitSet((byte)Direction.West)) DrawBorder(spriteBatch, cell, new Vector2(points.Point4.X, points.Point4.Y), new Vector2(points.Point5.X, points.Point5.Y));
                if (cell.Borders.IsBitSet((byte)Direction.NorthWest)) DrawBorder(spriteBatch, cell, new Vector2(points.Point5.X, points.Point5.Y), new Vector2(points.Point0.X, points.Point0.Y));
            }
            else
            {
                if (cell.Borders.IsBitSet((byte)Direction.North)) DrawBorder(spriteBatch, cell, new Vector2(points.Point5.X, points.Point5.Y), new Vector2(points.Point0.X, points.Point0.Y));
                if (cell.Borders.IsBitSet((byte)Direction.NorthEast)) DrawBorder(spriteBatch, cell, new Vector2(points.Point0.X, points.Point0.Y), new Vector2(points.Point1.X, points.Point1.Y));
                if (cell.Borders.IsBitSet((byte)Direction.SouthEast)) DrawBorder(spriteBatch, cell, new Vector2(points.Point1.X, points.Point1.Y), new Vector2(points.Point2.X, points.Point2.Y));
                if (cell.Borders.IsBitSet((byte)Direction.South)) DrawBorder(spriteBatch, cell, new Vector2(points.Point2.X, points.Point2.Y), new Vector2(points.Point3.X, points.Point3.Y));
                if (cell.Borders.IsBitSet((byte)Direction.SouthWest)) DrawBorder(spriteBatch, cell, new Vector2(points.Point3.X, points.Point3.Y), new Vector2(points.Point4.X, points.Point4.Y));
                if (cell.Borders.IsBitSet((byte)Direction.NorthWest)) DrawBorder(spriteBatch, cell, new Vector2(points.Point4.X, points.Point4.Y), new Vector2(points.Point5.X, points.Point5.Y));
            }
        }

        private void DrawBorder(SpriteBatch spriteBatch, Cell cell, Vector2 point1, Vector2 point2)
        {
            var pixel = WorldView.HexLibrary.FromOffsetCoordinatesToPixel(new HexOffsetCoordinates(cell.Column, cell.Row));
            var centerPosition = new Vector2(pixel.X, pixel.Y);

            spriteBatch.DrawLine(centerPosition + point1, centerPosition + point2, cell.ControlledByFaction == 1 ? Color.Yellow : Color.Red, 5.0f, 0.5f); // DarkGreen
        }

        private void DrawCell(SpriteBatch spriteBatch, Cell cell, Color color)
        {
            var neverSeen = cell.SeenState == SeenState.NeverSeen;
            //var foo = new PhoenixGameLibrary.GameData.Texture("NewTerrain", 13);
            var texture = AssetsManager.Instance.GetTexture(neverSeen ? "NewTerrain" : cell.Texture.TexturePalette);
            var spec = AssetsManager.Instance.GetAtlas(neverSeen ? "NewTerrain" : cell.Texture.TexturePalette);
            var frame = spec.Frames[neverSeen ? 13 : cell.Texture.TextureId];
            var sourceRectangle = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);

            var centerPosition = WorldView.HexLibrary.FromOffsetCoordinatesToPixel(new HexOffsetCoordinates(cell.Column, cell.Row));
            var destinationRectangle = new Rectangle((int)centerPosition.X, (int)centerPosition.Y, (int)WorldView.HexLibrary.Width, (int)WorldView.HexLibrary.Height); // + 64
            var layerDepth = cell.Index / 10000.0f;
            //var origin = new Vector2(sourceRectangle.Width * 0.5f, (sourceRectangle.Height * 2/3.0f + sourceRectangle.Height * 1/3.0f + sourceRectangle.Height * 1/3.0f) * 0.5f);
            var origin = new Vector2(sourceRectangle.Width * 0.5f, sourceRectangle.Height * 0.5f);
            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color, 0.0f, origin, SpriteEffects.None, layerDepth);
        }

        private void DrawHexBorder(SpriteBatch spriteBatch, Cell cell)
        {
            var pixel = WorldView.HexLibrary.FromOffsetCoordinatesToPixel(new HexOffsetCoordinates(cell.Column, cell.Row));
            var centerPosition = new Vector2(pixel.X, pixel.Y);

            var color = Color.PeachPuff;

            var points = WorldView.HexLibrary.GetCorners();

            var point0 = new Vector2(points.Point0.X, points.Point0.Y);
            var point1 = new Vector2(points.Point1.X, points.Point1.Y);
            var point2 = new Vector2(points.Point2.X, points.Point2.Y);
            var point3 = new Vector2(points.Point3.X, points.Point3.Y);
            var point4 = new Vector2(points.Point4.X, points.Point4.Y);
            var point5 = new Vector2(points.Point5.X, points.Point5.Y);

            spriteBatch.DrawLine(centerPosition + point0, centerPosition + point1, color, 1.0f);
            spriteBatch.DrawLine(centerPosition + point1, centerPosition + point2, color, 1.0f);
            spriteBatch.DrawLine(centerPosition + point2, centerPosition + point3, color, 1.0f);
            spriteBatch.DrawLine(centerPosition + point3, centerPosition + point4, color, 1.0f);
            spriteBatch.DrawLine(centerPosition + point4, centerPosition + point5, color, 1.0f);
            spriteBatch.DrawLine(centerPosition + point5, centerPosition + point0, color, 1.0f);
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                // dispose managed state (managed objects)

                // set large fields to null
                ViewportAdapter = null;

                IsDisposed = true;
            }
        }
    }
}