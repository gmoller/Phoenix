using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameUtilities.ExtensionMethods;
using PhoenixGameLibrary;
using PhoenixGamePresentation.Views;
using Utilities;
using Utilities.ExtensionMethods;

namespace PhoenixGamePresentation.Handlers
{
    public static class MinimapHandler
    {
        public static Rectangle GetViewedRectangle(WorldView worldView, PointI minimapSize)
        {
            var normalized = new Vector2(worldView.Camera.CameraFocusPointInWorld.X / worldView.WorldWidthInPixels, worldView.Camera.CameraFocusPointInWorld.Y / worldView.WorldHeightInPixels);
            var minimapViewedRectangleCenter = new Vector2(normalized.X * minimapSize.X, normalized.Y * minimapSize.Y);

            var percentageOfEntireWorldCameraIsLookingAt = new Vector2(worldView.Camera.CameraRectangleInWorld.Width / (float)worldView.WorldWidthInPixels, worldView.Camera.CameraRectangleInWorld.Height / (float)worldView.WorldHeightInPixels);
            var minimapViewedRectangleSize = new Vector2(minimapSize.X * percentageOfEntireWorldCameraIsLookingAt.X, minimapSize.Y * percentageOfEntireWorldCameraIsLookingAt.Y);

            var minimapViewedRectangle = new Rectangle(
                (int)minimapViewedRectangleCenter.X - (int)(minimapViewedRectangleSize.X * Constants.ONE_HALF),
                (int)minimapViewedRectangleCenter.Y - (int)(minimapViewedRectangleSize.Y * Constants.ONE_HALF),
                (int)minimapViewedRectangleSize.X,
                (int)minimapViewedRectangleSize.Y);

            return minimapViewedRectangle;
        }

        internal static Texture2D Create(CellGrid cellGrid)
        {
            var gameMetadata = CallContext<GameMetadata>.GetData("GameMetadata");
            var context = CallContext<GlobalContextPresentation>.GetData("GlobalContextPresentation");

            var graphicsDevice = context.GraphicsDevice;
            var terrainTypes = gameMetadata.TerrainTypes;

            var scalingFactorX = 7;
            var scalingFactorY = 6;
            var width = cellGrid.NumberOfColumns;
            var height = cellGrid.NumberOfRows;
            var scaledWidth = (width * scalingFactorX) + 4; // Math.Ceiling(scalingFactorX * Constants.ONE_HALF)
            var scaledHeight = (height * scalingFactorY) + 4; // scalingFactorY * Constants.ONE_HALF

            var colors = new Color[scaledWidth, scaledHeight];
            for (var row1 = 0; row1 < height; row1++)
            {
                var evenLine = row1.IsEven();
                for (var column1 = 0; column1 < width; column1++)
                {
                    var cell = cellGrid.GetCell(column1, row1);
                    var terrainTypeId = cell.TerrainTypeId;
                    var color = terrainTypes[terrainTypeId].MinimapColor.ToColor();

                    var hexColors = GetHexColors(color, Color.DarkSlateGray, cell.SeenState);

                    for (var row2 = 0; row2 < 8; row2++)
                    {
                        for (var column2 = 0; column2 < 7; column2++)
                        {
                            var col = (column1 * 7) + column2 + (evenLine ? 0 : 3);
                            var row = (row1 * 6) + row2;

                            if (colors[col, row] == Color.Transparent)
                            {
                                colors[col, row] = hexColors[column2, row2];
                            }
                        }
                    }
                }
            }

            var colors1D = colors.To1DArray();
            var minimap = new Texture2D(graphicsDevice, scaledWidth, scaledHeight, false, SurfaceFormat.Color);
            minimap.SetData(colors1D);

            return minimap;
        }

        private static Color[,] GetHexColors(Color seenColor, Color unseenColor, SeenState seenState)
        {
            Color[,] colors = new Color[7, 8];
            for (var row = 0; row < 8; row++)
            {
                for (var column = 0; column < 7; column++)
                {
                    Color colorToSet;
                    if (seenState == SeenState.NeverSeen)
                    {
                        colorToSet = unseenColor;
                    }
                    else if (row >= 2 && row <= 5)
                    {
                        colorToSet = seenColor;
                    }
                    else if (row == 0 || row == 7)
                    {
                        colorToSet = column == 3 ? seenColor : Color.Transparent;
                    }
                    else if (row == 1 || row == 6)
                    {
                        if (column == 0 || column == 6)
                        {
                            colorToSet = Color.Transparent;
                        }
                        else
                        {
                            colorToSet = seenColor;
                        }
                    }
                    else
                    {
                        throw new Exception();
                    }

                    colors[column, row] = colorToSet;
                }
            }

            return colors;
        }
    }
}