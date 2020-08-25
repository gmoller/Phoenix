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

            var cellGrid = world.OverlandMap.CellGrid;
            var width = cellGrid.NumberOfColumns;
            var height = cellGrid.NumberOfRows;
            var minimap = new Texture2D(graphicsDevice, width, height, false, SurfaceFormat.Color);

            var colors = new Color[width * height];
            var i = 0;
            for (var row = 0; row < height; row++)
            {
                for (var column = 0; column < width; column++)
                {
                    var cell = cellGrid.GetCell(column, row);
                    var terrainTypeId = cell.TerrainTypeId;
                    var color = cell.SeenState == SeenState.NeverSeen ? Utilities.Color.Black :  terrainTypes[terrainTypeId].MinimapColor;
                    colors[i] = new Color(color.R, color.G, color.B, color.A);
                    i++;
                }
            }

            minimap.SetData(colors);

            return minimap;
        }
    }
}