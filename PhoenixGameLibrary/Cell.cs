﻿using System.Diagnostics;
using PhoenixGameLibrary.GameData;
using Utilities;

namespace PhoenixGameLibrary
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public struct Cell
    {
        public int Column => Index % Constants.WORLD_MAP_COLUMNS;
        public int Row => Index / Constants.WORLD_MAP_COLUMNS;
        public int TerrainTypeId { get; }
        public Texture Texture { get; }
        public int Index { get; }

        public Cell(int col, int row, TerrainType terrainType)
        {
            Index = (row * Constants.WORLD_MAP_COLUMNS) + col;
            TerrainTypeId = terrainType.Id;
            Texture = terrainType.PossibleTextures[RandomNumberGenerator.Instance.GetRandomInt(0, 3)];
        }

        private string DebuggerDisplay => $"{{Col={Column},Row={Row},TerrainTypeId={TerrainTypeId}}}";
    }
}