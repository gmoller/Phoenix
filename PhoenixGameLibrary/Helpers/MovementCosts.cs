using System;
using System.Collections.Generic;
using PhoenixGameConfig;
using PhoenixGameData;
using Zen.Utilities;

namespace PhoenixGameLibrary.Helpers
{
    public static class MovementCosts
    {
        public static GetCostToMoveIntoResult GetCostToMoveInto(PointI location, EnumerableList<string> movements, float movementPoints)
        {
            var world = CallContext<World>.GetData("GameWorld");
            var cellToMoveTo = world.OverlandMap.CellGrid.GetCell(location);

            return GetCostToMoveInto(cellToMoveTo, movements, movementPoints);
        }

        public static GetCostToMoveIntoResult GetCostToMoveInto(Cell cell, EnumerableList<string> movements, float movementPoints)
        {
            if (movementPoints <= 0.0f) return new GetCostToMoveIntoResult(false);
            if (cell == Cell.Empty) return new GetCostToMoveIntoResult(false);
            if (cell.SeenState == SeenState.NeverSeen) return new GetCostToMoveIntoResult(true, 9999999.9f);

            return GetCostToMoveInto(cell.TerrainId, movements);
        }

        private static GetCostToMoveIntoResult GetCostToMoveInto(int terrainId, EnumerableList<string> movements)
        {
            var potentialMovementCosts = GetPotentialMovementCosts(terrainId, movements);
            var canMoveInto = potentialMovementCosts.Count > 0;

            if (!canMoveInto) return new GetCostToMoveIntoResult(false);

            float costToMoveInto = float.MaxValue;
            bool foundCost = false;
            foreach (var potentialMovementCost in potentialMovementCosts)
            {
                if (potentialMovementCost.Cost < costToMoveInto)
                {
                    costToMoveInto = potentialMovementCost.Cost;
                    foundCost = true;
                }
            }

            if (!foundCost) throw new Exception($"No cost found for Terrain [{terrainId}], MovementTypes [{movements}].");

            return new GetCostToMoveIntoResult(true, costToMoveInto);
        }

        private static List<MovementCost> GetPotentialMovementCosts(int terrainId, EnumerableList<string> movements)
        {
            var gameConfigCache = CallContext<GameConfigCache>.GetData("GameConfigCache");
            var terrain = gameConfigCache.GetTerrainConfigById(terrainId);

            var potentialMovementCosts = new List<MovementCost>();
            foreach (var unitMovement in movements)
            {
                foreach (var terrainMovement in terrain.Movements)
                {
                    var movement = gameConfigCache.GetMovementConfigById((int)terrainMovement.MovementId);

                    if (unitMovement != movement.Name) continue;
                    if ((float)terrainMovement.Cost > 0.0f)
                    {
                        var movementCost = new MovementCost(movement.Name, (float)terrainMovement.Cost);
                        potentialMovementCosts.Add(movementCost);
                    }
                }
            }

            return potentialMovementCosts;
        }
    }
}