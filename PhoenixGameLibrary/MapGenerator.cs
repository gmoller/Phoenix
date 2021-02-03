using System;
using System.Collections.Generic;
using PhoenixGameConfig;
using Zen.Noise;
using Zen.Utilities;

namespace PhoenixGameLibrary
{
    public static class MapGenerator
    {
        internal static int[,] Generate(int numberOfColumns, int numberOfRows)
        {
            // make some noise!
            float[,] noise = MakeNoise(numberOfColumns, numberOfRows);

            int[,] terrain = TurnNoiseIntoTerrain(noise);

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

        private static int[,] TurnNoiseIntoTerrain(float[,] noise)
        {
            var numberOfColumns = noise.GetLength(0);
            var numberOfRows = noise.GetLength(1);
            var terrain = new int[numberOfColumns, numberOfRows];

            for (var y = 0; y < numberOfRows; y++)
            {
                for (var x = 0; x < numberOfColumns; x++)
                {
                    var val = noise[x, y];
                    var terrainId = DetermineTerrainId(val);
                    terrain[x, y] = terrainId;
                }
            }

            return terrain;
        }

        private static int DetermineTerrainId(float val)
        {
            var ranges = new List<(float from, float to, int terrainId)>
            {
                (float.MinValue, 0.0f, 12), // "Ocean"
                (0.0f, 0.1f, 1), // "Grassland"
                (0.1f, 0.2f, 1),
                (0.2f, 0.3f, 1),
                (0.3f, 0.35f, 1),
                (0.35f, 0.38f, 1),
                (0.38f, 0.4f, 1),
                (0.4f, 0.5f, 2), // "Forest"
                (0.5f, 0.7f, 7), // "Hill"
                (0.7f, float.MaxValue, 8) // "Mountain"
            };

            var terrainId = GetTerrainId(val, ranges);

            return terrainId;
        }

        private static int GetTerrainId(float val, List<(float from, float to, int terrainId)> ranges)
        {
            foreach (var range in ranges)
            {
                if (val >= range.from && val < range.to)
                {
                    return range.terrainId;
                }
            }

            throw new Exception($"That was unexpected! Val of {val} not supported.");
        }
    }
}