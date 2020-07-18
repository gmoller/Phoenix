using System;
using PhoenixGameLibrary.GameData;

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
            int numberOfColumns = noise.GetLength(0);
            int numberOfRows = noise.GetLength(1);
            var terrain = new TerrainType[numberOfColumns, numberOfRows];

            for (int y = 0; y < numberOfRows; ++y)
            {
                for (int x = 0; x < numberOfColumns; ++x)
                {
                    float val = noise[x, y];
                    TerrainType terrainType = DetermineTerrainTypeId(val);
                    terrain[x, y] = terrainType;
                }
            }

            return terrain;
        }

        private static TerrainType DetermineTerrainTypeId(float val)
        {
            var terrainTypes = Globals.Instance.TerrainTypes;
            TerrainType terrainType;

            if (IsOcean(val))
            {
                terrainType = terrainTypes[11];
            }
            else if (IsGrassland(val))
            {
                terrainType = terrainTypes[0];
            }
            //else if (IsForest(val))
            //{
            //    terrainType = terrainTypes[1];
            //}
            else if (IsHill(val))
            {
                terrainType = terrainTypes[6];
            }
            else if (IsMountain(val))
            {
                terrainType = terrainTypes[7];
            }
            else
            {
                throw new Exception($"That was unexpected! Val of {val} not supported.");
            }

            return terrainType;
        }

        private static bool IsOcean(float val)
        {
            return val < -0.3f;
        }

        private static bool IsGrassland(float val)
        {
            return val >= -0.3f && val < 0.4f;
        }

        //private static bool IsForest(float val)
        //{
        //    return val >= 0.4f && val < 0.6f;
        //}

        private static bool IsHill(float val)
        {
            return val >= 0.4f && val < 0.7f;
        }

        private static bool IsMountain(float val)
        {
            return val >= 0.7f && val <= 1.0f;
        }
    }
}