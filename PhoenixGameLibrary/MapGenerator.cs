using System;
using System.Collections.Generic;
using PhoenixGameLibrary.GameData;
using Utilities;

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
            for (float y = 0.0f; y < numberOfRows; ++y)
            {
                for (float x = 0.0f; x < numberOfColumns; ++x)
                {
                    float val = GetNoise(x, y, FastNoise.Interp.Linear);
                    noise[(int)x, (int)y] = val;
                }
            }

            return noise;
        }

        private static float GetNoise(float x, float y, FastNoise.Interp interp)
        {
            var fastNoise = new FastNoise();
            fastNoise.SetNoiseType(FastNoise.NoiseType.Value);
            fastNoise.SetInterp(interp);
            fastNoise.SetSeed(1336);
            fastNoise.SetFrequency(0.5f);
            float val = fastNoise.GetNoise(x, y);

            return val;
        }

        private static TerrainType[,] TurnNoiseIntoTerrain(float[,] noise)
        {
            var numberOfColumns = noise.GetLength(0);
            var numberOfRows = noise.GetLength(1);
            var terrain = new TerrainType[numberOfColumns, numberOfRows];

            for (var y = 0; y < numberOfRows; ++y)
            {
                for (var x = 0; x < numberOfColumns; ++x)
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
            var context = CallContext<GlobalContext>.GetData("AmbientGlobalContext");
            var terrainTypes = context.GameMetadata.TerrainTypes;

            var ranges = new List<(float from, float to, string terrainTypeName)>
            {
                (float.MinValue, 0.0f, "Ocean"),
                (0.0f, 0.4f, "Grassland"),
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

        private static bool IsOcean(float val)
        {
            return val < -0.3f;
        }

        private static bool IsGrassland(float val)
        {
            return val >= -0.3f && val < 0.4f;
        }

        private static bool IsForest(float val)
        {
            return val >= 0.4f && val < 0.5f;
        }

        private static bool IsHill(float val)
        {
            return val >= 0.5f && val < 0.7f;
        }

        private static bool IsMountain(float val)
        {
            return val >= 0.7f && val <= 1.0f;
        }
    }
}