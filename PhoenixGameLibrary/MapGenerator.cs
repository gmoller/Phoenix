using System;
using System.Collections.Generic;
using PhoenixGameLibrary.GameData;
using Zen.Noise;
using Zen.Utilities;

namespace PhoenixGameLibrary
{
    public static class MapGenerator
    {
        internal static TerrainType[,] Generate(int numberOfColumns, int numberOfRows)
        {
            // make some noise!
            float[,] noise = MakeNoise(numberOfColumns, numberOfRows);

            TerrainType[,] terrain = TurnNoiseIntoTerrain(noise);

            return terrain;
        }

        private static float[,] MakeNoise(int numberOfColumns, int numberOfRows)
        {
            var noise = new float[numberOfColumns, numberOfRows];
            for (float y = 0.0f; y < numberOfRows; y++)
            {
                for (float x = 0.0f; x < numberOfColumns; x++)
                {
                    float val = GetNoise(x, y, InterpolationMethod.Linear);
                    noise[(int)x, (int)y] = val;
                }
            }

            return noise;
        }

        private static float GetNoise(float x, float y, InterpolationMethod interpolationMethod)
        {
            var val = ValueNoise.GetSingleValue((int)x, (int)y, 0.5f, 1336, InterpolationMethod.Linear);

            return val;
        }

        private static TerrainType[,] TurnNoiseIntoTerrain(float[,] noise)
        {
            var numberOfColumns = noise.GetLength(0);
            var numberOfRows = noise.GetLength(1);
            var terrain = new TerrainType[numberOfColumns, numberOfRows];

            for (var y = 0; y < numberOfRows; y++)
            {
                for (var x = 0; x < numberOfColumns; x++)
                {
                    var val = noise[x, y];
                    var terrainType = DetermineTerrainTypeId(val);
                    terrain[x, y] = terrainType;
                }
            }

            return terrain;
        }

        private static TerrainType DetermineTerrainTypeId(float val)
        {
            var gameMetadata = CallContext<GameMetadata>.GetData("GameMetadata");
            var terrainTypes = gameMetadata.TerrainTypes;

            var ranges = new List<(float from, float to, string terrainTypeName)>
            {
                (float.MinValue, 0.0f, "Ocean"),
                (0.0f, 0.1f, "Grassland"),
                (0.1f, 0.2f, "Grassland"),
                (0.2f, 0.3f, "Grassland"),
                (0.3f, 0.35f, "Grassland"),
                (0.35f, 0.38f, "Grassland"),
                (0.38f, 0.4f, "Grassland"),
                (0.4f, 0.5f, "Forest"),
                (0.5f, 0.7f, "Hill"),
                (0.7f, float.MaxValue, "Mountain")
            };

            var terrainTypeName = GetTerrainTypeName(val, ranges);
            var terrainType = terrainTypes[terrainTypeName];

            return terrainType;
        }

        private static string GetTerrainTypeName(float val, List<(float from, float to, string terrainTypeName)> ranges)
        {
            foreach (var range in ranges)
            {
                if (val >= range.from && val < range.to)
                {
                    return range.terrainTypeName;
                }
            }

            throw new Exception($"That was unexpected! Val of {val} not supported.");
        }
    }
}