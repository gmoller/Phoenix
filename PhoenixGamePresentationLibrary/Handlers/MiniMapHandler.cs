using System.Runtime.Remoting.Messaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhoenixGameLibrary;

namespace PhoenixGamePresentationLibrary.Handlers
{
    public static class MinimapHandler
    {
        internal static Texture2D Create(World world)
        {
            var context = (GlobalContext)CallContext.LogicalGetData("AmbientGlobalContext");

            var graphicsDevice = (GraphicsDevice)context.GraphicsDevice;
            var terrainTypes = context.GameMetadata.TerrainTypes;

            var scalingFactor = 2;
            var cellGrid = world.OverlandMap.CellGrid;
            var width = cellGrid.NumberOfColumns;
            var scaledWidth = width * scalingFactor;
            var height = cellGrid.NumberOfRows;
            var scaledHeight = height * scalingFactor;
            var minimap = new Texture2D(graphicsDevice, scaledWidth, scaledHeight, false, SurfaceFormat.Color);

            var colors = new Color[scaledWidth * scaledHeight];
            var i = 0;
            for (var row = 0; row < height; row++)
            {
                for (var column = 0; column < width; column++)
                {
                    var cell = cellGrid.GetCell(column, row);
                    var terrainTypeId = cell.TerrainTypeId;
                    var color = cell.SeenState == SeenState.NeverSeen ? Utilities.Color.Black :  terrainTypes[terrainTypeId].MinimapColor;

                    var index = i;
                    colors[index] = new Color(color.R, color.G, color.B, color.A);
                    index = i + 1;
                    colors[index] = new Color(color.R, color.G, color.B, color.A);

                    index = i + scaledWidth;
                    if (index < colors.Length)
                    {
                        colors[index] = new Color(color.R, color.G, color.B, color.A);
                        index = i + 1 + scaledWidth;
                        colors[index] = new Color(color.R, color.G, color.B, color.A);
                    }

                    i += scalingFactor;
                }

                i += scaledWidth + (row % 2 == 0 ? 1 : 0);
            }

            minimap.SetData(colors);

            return minimap;
        }
    }
}