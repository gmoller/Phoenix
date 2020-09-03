using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Hex;
using PhoenixGameLibrary.GameData;
using Utilities;

namespace PhoenixGameLibrary
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Stack : IEnumerable<Unit>
    {
        #region State
        private readonly World _world;
        private readonly Units _units;

        public UnitStatus Status { get; private set; }
        #endregion

        public PointI Location => _units.Count > 0 ?_units[0].Location : PointI.Empty;

        public int SightRange => GetSightRange();

        public float MovementPoints => DetermineMovementPoints();
        public EnumerableList<string> MovementTypes => new EnumerableList<string>(DetermineMovementTypes());
        public EnumerableList<string> Actions => new EnumerableList<string>(DetermineActions(_units));

        public int Count => _units.Count;
        public bool IsBusy => Status == UnitStatus.Patrol || Status == UnitStatus.Fortify;

        public Unit this[int index] => _units[index];

        public Stack(World world, Units units)
        {
            _world = world;
            _units = units;
            Status = UnitStatus.None;
        }

        public void DoPatrolAction()
        {
            Status = UnitStatus.Patrol;
            _units.DoPatrolAction();
        }

        public void DoFortifyAction()
        {
            Status = UnitStatus.Fortify;
            _units.DoFortifyAction();
        }

        public void DoExploreAction()
        {
            Status = UnitStatus.Explore;
            _units.DoExploreAction();
        }

        public void DoBuildAction()
        {
            var builders = _units.GetUnitsByAction("BuildOutpost");
            if (builders.Count == 0) return;

            builders[0].DoBuildAction();
            _units.Remove(builders[0]);
        }

        public void SetStatusToNone()
        {
            Status = UnitStatus.None;
            _units.SetStatusToNone();
        }

        internal void MoveTo(PointI locationToMoveTo)
        {
            var cellToMoveTo = _world.OverlandMap.CellGrid.GetCell(locationToMoveTo.X, locationToMoveTo.Y);
            var movementCost = GetCostToMoveInto(cellToMoveTo);

            foreach (var unit in _units)
            {
                unit.Location = locationToMoveTo;
                unit.SetSeenCells(Location);

                unit.MovementPoints -= movementCost.CostToMoveInto;
                if (unit.MovementPoints < 0.0f)
                {
                    unit.MovementPoints = 0.0f;
                }
            }
        }

        public GetCostToMoveIntoResult GetCostToMoveInto(PointI location)
        {
            var cellToMoveTo = _world.OverlandMap.CellGrid.GetCell(location.X, location.Y);

            return GetCostToMoveInto(cellToMoveTo);
        }

        public GetCostToMoveIntoResult GetCostToMoveInto(Cell cell)
        {
            if (cell == Cell.Empty) return new GetCostToMoveIntoResult(false);
            if (cell.SeenState == SeenState.NeverSeen) return new GetCostToMoveIntoResult(true, 9999999.9f);

            var gameMetadata = CallContext<GameMetadata>.GetData("GameMetadata");
            var terrainTypes = gameMetadata.TerrainTypes;
            var terrainType = terrainTypes[cell.TerrainTypeId];

            return GetCostToMoveInto(terrainType);
        }

        private int GetSightRange()
        {
            var sightRange = 0;
            foreach (var unit in _units)
            {
                if (unit.SightRange > sightRange)
                {
                    sightRange = unit.SightRange;
                }

            }

            return sightRange;
        }

        private GetCostToMoveIntoResult GetCostToMoveInto(TerrainType terrainType)
        {
            var potentialMovementCosts = GetPotentialMovementCosts(terrainType);
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

            if (!foundCost) throw new Exception($"No cost found for Terrain Type [{terrainType}], MovementTypes [{MovementTypes}].");

            return new GetCostToMoveIntoResult(true, costToMoveInto);
        }

        private List<MovementCost> GetPotentialMovementCosts(TerrainType terrainType)
        {
            var potentialMovementCosts = new List<MovementCost>();
            foreach (var unitMovementType in MovementTypes)
            {
                foreach (var movementCost in terrainType.MovementCosts)
                {
                    if (unitMovementType != movementCost.MovementType.Name) continue;
                    if (movementCost.Cost > 0.0)
                    {
                        potentialMovementCosts.Add(movementCost);
                    }
                }
            }

            return potentialMovementCosts;
        }

        internal void BeginTurn()
        {
        }

        internal void EndTurn()
        {
            foreach (var unit in _units)
            {
                unit.EndTurn();
            }
        }

        internal bool CanSeeCell(Cell cell)
        {
            foreach (var unit in _units)
            {
                if (unit.CanSeeCell(cell))
                {
                    return true;
                }
            }

            return false;
        }

        private float DetermineMovementPoints()
        {
            var movementPoints = float.MaxValue;
            foreach (var unit in _units)
            {
                movementPoints = Math.Min(movementPoints, unit.MovementPoints);
            }

            return movementPoints;
        }

        private List<string> DetermineMovementTypes()
        {
            var movementTypes = new List<string>();

            if (IsSwimming(_units)) movementTypes.Add("Swimming");
            if (IsFlying(_units)) movementTypes.Add("Flying");
            if (IsSailing(_units)) movementTypes.Add("Sailing");
            if (IsForester(_units)) movementTypes.Add("Forester");
            if (IsMountaineer(_units)) movementTypes.Add("Mountaineer");
            if (IsPathfinding(_units)) movementTypes.Add("Pathfinding");
            if (IsPlaneShift(_units)) movementTypes.Add("PlaneShift");

            // if none of Swimming,Flying,Sailing,Forester,Mountaineer,Pathfinding then Walking
            if (!(IsSwimming(_units) || IsFlying(_units) || IsSailing(_units) || IsForester(_units) || IsMountaineer(_units) || IsPathfinding(_units))) movementTypes.Add("Walking");

            return movementTypes;
        }

        private List<string> DetermineActions(Units units)
        {
            var gameMetadata = CallContext<GameMetadata>.GetData("GameMetadata");
            var actionTypes = gameMetadata.ActionTypes;

            // first add actions that apply to all
            var filteredActionTypes = (from actionType in actionTypes where actionType.AppliesToAll select actionType).ToList();

            // then add any other actions that apply to units
            foreach (var unit in units)
            {
                var unitActions = unit.Actions;
                foreach (var action in unitActions)
                {
                    var actionType = actionTypes[action];
                    var addAction = false;
                    // don't add if there's already an action added (i.e. no duplicates)
                    if (filteredActionTypes.All(at => at.ButtonName != action))
                    {
                        // do a specific check for this action
                        // TODO: move this into a Func on the ActionType: addAction = actionType.DoSpecificCheck(Location);
                        if (action == "BuildOutpost")
                        {
                            addAction = CanSettleOnTerrain(Location);
                        }
                        else
                        {
                            addAction = true;
                        }
                    }

                    if (addAction)
                    {
                        filteredActionTypes.Add(actionType);
                    }
                }
            }

            var actionsNames = (from action in filteredActionTypes select action.Name).ToList();

            return actionsNames;
        }

        private bool CanSettleOnTerrain(PointI thisLocation)
        {
            var gameMetadata = CallContext<GameMetadata>.GetData("GameMetadata");
            var terrainTypes = gameMetadata.TerrainTypes;

            // if terrain is settle-able
            var cell = _world.OverlandMap.CellGrid.GetCell(thisLocation);
            var terrainType = terrainTypes[cell.TerrainTypeId];

            if (!terrainType.CanSettleOn) return false;
            
            // and not within 4 distance from another settlement
            var settlements = _world.Settlements;
            foreach (var settlement in settlements)
            {
                var distance = HexOffsetCoordinates.GetDistance(thisLocation.X, thisLocation.Y, settlement.Location.X, settlement.Location.Y);
                if (distance >= 4)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsSwimming(Units units)
        {
            // TODO: windwalking

            var everyUnitHasEitherSwimmingOrFlight = true;
            foreach (var unit in units)
            {
                var hasSwimmingOrFlight = unit.UnitTypeMovementTypes.Contains("Swimming") || unit.UnitTypeMovementTypes.Contains("Flying");
                if (!hasSwimmingOrFlight)
                {
                    everyUnitHasEitherSwimmingOrFlight = false;
                }
            }

            var noUnitHasSailing = true;
            foreach (var unit in units)
            {
                var hasSailing = unit.UnitTypeMovementTypes.Contains("Sailing");
                if (hasSailing)
                {
                    noUnitHasSailing = false;
                    break;
                }
            }

            return everyUnitHasEitherSwimmingOrFlight && noUnitHasSailing;
        }

        private bool IsFlying(Units units)
        {
            // TODO: windwalking

            var everyUnitHasFlight = true;
            foreach (var unit in units)
            {
                var flight = unit.UnitTypeMovementTypes.Contains("Flying");
                if (!flight)
                {
                    everyUnitHasFlight = false;
                    break;
                }
            }

            return everyUnitHasFlight;
        }

        private bool IsSailing(Units units)
        {
            // TODO: sufficient transport for ground units
            bool thereIsSufficientTransportForAllGroundUnits = true;
            var anyUnitHasSailing = false;
            foreach (var unit in units)
            {
                var hasSailing = unit.UnitTypeMovementTypes.Contains("Sailing");
                if (hasSailing)
                {
                    anyUnitHasSailing = true;
                    break;
                }
            }

            return anyUnitHasSailing && thereIsSufficientTransportForAllGroundUnits;
        }

        private bool IsForester(Units units)
        {
            // TODO: windwalking

            var anyUnitHasForester = false;
            foreach (var unit in units)
            {
                var hasForester = unit.UnitTypeMovementTypes.Contains("Forester");
                if (hasForester)
                {
                    anyUnitHasForester = true;
                    break;
                }
            }

            var noUnitHasSailingOrMountaineer = true;
            foreach (var unit in units)
            {
                var hasSailingOrMountaineer = unit.UnitTypeMovementTypes.Contains("Sailing") || unit.UnitTypeMovementTypes.Contains("Mountaineer");
                if (hasSailingOrMountaineer)
                {
                    noUnitHasSailingOrMountaineer = false;
                    break;
                }
            }

            return anyUnitHasForester && noUnitHasSailingOrMountaineer;
        }

        private bool IsMountaineer(Units units)
        {
            // TODO: windwalking

            var anyUnitHasMountaineer = false;
            foreach (var unit in units)
            {
                var hasMountaineer = unit.UnitTypeMovementTypes.Contains("Mountaineer");
                if (hasMountaineer)
                {
                    anyUnitHasMountaineer = true;
                    break;
                }
            }

            var noUnitHasSailingOrForester = true;
            foreach (var unit in units)
            {
                var hasSailingOrForester = unit.UnitTypeMovementTypes.Contains("Sailing") || unit.UnitTypeMovementTypes.Contains("Forester");
                if (hasSailingOrForester)
                {
                    noUnitHasSailingOrForester = false;
                    break;
                }
            }

            return anyUnitHasMountaineer && noUnitHasSailingOrForester;
        }

        private bool IsPathfinding(Units units)
        {
            // TODO: non-corporeal

            //not flying or sailing
            var isNotFlyingOrSailing = !(IsFlying(units) || IsSailing(units));

            var anyUnitHasPathfinding = false;
            foreach (var unit in units)
            {
                var hasPathfinding = unit.UnitTypeMovementTypes.Contains("Pathfinding");
                if (hasPathfinding)
                {
                    anyUnitHasPathfinding = true;
                    break;
                }
            }

            var anyUnitHasForester = false;
            foreach (var unit in units)
            {
                var hasForester = unit.UnitTypeMovementTypes.Contains("Forester");
                if (hasForester)
                {
                    anyUnitHasForester = true;
                    break;
                }
            }

            var anyUnitHasMountaineer = false;
            foreach (var unit in units)
            {
                var hasMountaineer = unit.UnitTypeMovementTypes.Contains("Mountaineer");
                if (hasMountaineer)
                {
                    anyUnitHasMountaineer = true;
                    break;
                }
            }

            return isNotFlyingOrSailing && anyUnitHasPathfinding || (anyUnitHasForester && anyUnitHasMountaineer);
        }

        private bool IsPlaneShift(Units units)
        {
            var everyUnitHasPlaneShift = true;
            foreach (var unit in units)
            {
                var planeShift = unit.UnitTypeMovementTypes.Contains("PlaneShift");
                if (!planeShift)
                {
                    everyUnitHasPlaneShift = false;
                    break;
                }
            }

            return everyUnitHasPlaneShift;
        }

        public IEnumerator<Unit> GetEnumerator()
        {
            foreach (var item in _units)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Count={_units.Count}}}";
    }
}