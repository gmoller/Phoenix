using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using PhoenixGameData;
using PhoenixGameData.Enumerations;
using PhoenixGameData.StrongTypes;
using PhoenixGameData.Tuples;
using Zen.Hexagons;
using Zen.Utilities;

namespace PhoenixGameLibrary
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Stack : IEnumerable<Unit>
    {
        private readonly GameConfigCache _gameConfigCache;
        private readonly GameDataRepository _gameDataRepository;

        private StackRecord _stackRecord;
        private Units Units { get; }

        public UnitStatus Status => _stackRecord.Status.Value;
        public bool OrdersGiven => _stackRecord.HaveOrdersBeenGivenThisTurn.Value;
        public PointI LocationHex => _stackRecord.LocationHex.Value;
        public int SightRange => GetSightRange();
        public float MovementPoints => DetermineMovementPoints();
        public EnumerableList<string> MovementTypes => new EnumerableList<string>(DetermineMovementTypes());
        public EnumerableList<string> Actions => new EnumerableList<string>(DetermineActions(Units));
        public int Count => Units.Count;
        public Unit this[int index] => Units[index];

        public Stack(int stackId)
        {
            _gameConfigCache = CallContext<GameConfigCache>.GetData("GameConfigCache");
            _gameDataRepository = CallContext<GameDataRepository>.GetData("GameDataRepository");
            _stackRecord = _gameDataRepository.GetStackById(stackId);
            var unitRecords = _gameDataRepository.GetUnitsByStackId(stackId);
            Units = new Units();
            foreach (var unitRecord in unitRecords)
            {
                Units.Add(new Unit(unitRecord.Id));
            }

            _gameDataRepository.StackUpdated += StackUpdated;
        }

        private void StackUpdated(object sender, StackRecord stackRecord)
        {
            if (stackRecord.Id == _stackRecord.Id)
            {
                _stackRecord = stackRecord;
            }
        }

        public void DoAction(string action)
        {
            //TODO: ToDictionary, de-hardcode
            switch (action)
            {
                case "Done":
                    DoDoneAction();
                    break;
                case "Patrol":
                    DoPatrolAction();
                    break;
                case "Fortify":
                    DoFortifyAction();
                    break;
                case "Explore":
                    DoExploreAction();
                    break;
                case "BuildOutpost":
                    DoBuildAction();
                    break;
                default:
                    throw new Exception($"Action [{action}] is not implemented.");
            }
        }

        private void DoDoneAction()
        {
            var updatedStack = new StackRecord(_stackRecord, _stackRecord.LocationHex, new Status(UnitStatus.None), new HaveOrdersBeenGivenThisTurn(true));
            _gameDataRepository.Update(updatedStack);
        }

        private void DoPatrolAction()
        {
            var updatedStack = new StackRecord(_stackRecord, _stackRecord.LocationHex, new Status(UnitStatus.Patrol), new HaveOrdersBeenGivenThisTurn(true));
            _gameDataRepository.Update(updatedStack);
            Units.DoPatrolAction();
        }

        private void DoFortifyAction()
        {
            var updatedStack = new StackRecord(_stackRecord, _stackRecord.LocationHex, new Status(UnitStatus.Fortify), new HaveOrdersBeenGivenThisTurn(true));
            _gameDataRepository.Update(updatedStack);
            Units.DoFortifyAction();
        }

        private void DoExploreAction()
        {
            var updatedStack = new StackRecord(_stackRecord, _stackRecord.LocationHex, new Status(UnitStatus.Explore), new HaveOrdersBeenGivenThisTurn(true));
            _gameDataRepository.Update(updatedStack);
            Units.DoExploreAction();
        }

        private void DoBuildAction()
        {
            var builders = Units.GetUnitsByAction("BuildOutpost");
            if (builders.Count == 0) return;

            builders[0].DoBuildAction();
            Units.Remove(builders[0]);
        }

        public void SetStatusToNone()
        {
            var updatedStack = new StackRecord(_stackRecord, _stackRecord.LocationHex, new Status(UnitStatus.None), new HaveOrdersBeenGivenThisTurn(false));
            _gameDataRepository.Update(updatedStack);
            Units.SetStatusToNone();
        }

        internal void MoveTo(PointI locationToMoveTo)
        {
            var world = CallContext<World>.GetData("GameWorld");
            var cellToMoveTo = world.OverlandMap.CellGrid.GetCell(locationToMoveTo);
            var movementCost = Helpers.MovementCosts.GetCostToMoveInto(cellToMoveTo, MovementTypes, MovementPoints);

            foreach (var unit in Units)
            {
                var updatedStack = new StackRecord(_stackRecord, new LocationHex(locationToMoveTo));
                _gameDataRepository.Update(updatedStack);
                unit.SetSeenCells(LocationHex);

                //var newMovementPoints = unit.MovementPoints - movementCost.CostToMoveInto;
                //var unitRecord = gameDataRepository.GetUnitById(unit.Id);
                //var updatedUnit = new UnitRecord(updatedUnit, updatedUnit.Stackid, newMovementPoints);
                //gameDataRepository.Update(updatedUnit);
                //if (unit.MovementPoints <= 0.0f)
                //{
                //    unit.MovementPoints = 0.0f;
                //    updatedStack = new StackRecord(_stackRecord, _stackRecord.LocationHex, _stackRecord.Status, true);
                //    gameDataRepository.Update(updatedStack);
                //}
            }
        }

        private int GetSightRange()
        {
            // Could be cached, could be moved to units
            var sightRange = 0;
            foreach (var unit in Units)
            {
                if (unit.SightRange > sightRange)
                {
                    sightRange = unit.SightRange;
                }
            }

            return sightRange;
        }

        internal void BeginTurn()
        {
            var ordersGiven = Status == UnitStatus.Fortify || Status == UnitStatus.Patrol; // TODO: de-hardcode these, a persistent flag?
            var updatedStack = new StackRecord(_stackRecord, _stackRecord.LocationHex, _stackRecord.Status, new HaveOrdersBeenGivenThisTurn(ordersGiven));
            _gameDataRepository.Update(updatedStack);
        }

        internal void EndTurn()
        {
            if (!OrdersGiven) throw new Exception("Can not end turn, Stack still requires orders.");

            foreach (var unit in Units)
            {
                unit.EndTurn();
            }
        }

        internal bool CanSeeCell(Cell cell)
        {
            foreach (var unit in Units)
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
            foreach (var unit in Units)
            {
                movementPoints = Math.Min(movementPoints, unit.MovementPoints);
            }

            return movementPoints;
        }

        private List<string> DetermineMovementTypes()
        {
            var movementTypes = new List<string>();

            if (IsSwimming(Units)) movementTypes.Add("Swimming");
            if (IsFlying(Units)) movementTypes.Add("Flying");
            if (IsSailing(Units)) movementTypes.Add("Sailing");
            if (IsForester(Units)) movementTypes.Add("Forester");
            if (IsMountaineer(Units)) movementTypes.Add("Mountaineer");
            if (IsPathfinding(Units)) movementTypes.Add("Pathfinding");
            if (IsPlaneShift(Units)) movementTypes.Add("PlaneShift");

            // if none of Swimming,Flying,Sailing,Forester,Mountaineer,Pathfinding then Walking
            if (!(IsSwimming(Units) || IsFlying(Units) || IsSailing(Units) || IsForester(Units) || IsMountaineer(Units) || IsPathfinding(Units))) movementTypes.Add("Walking");

            return movementTypes;
        }

        private List<string> DetermineActions(Units units)
        {
            // first add actions that apply to all
            var filteredActions = Helpers.Actions.GetActionsThatApplyToAll();

            // then add any other actions that apply to units
            foreach (var unit in units)
            {
                var unitActions = unit.Actions;
                foreach (var unitAction in unitActions)
                {
                    // don't add if there's already an action added (i.e. no duplicates)
                    var alreadyThere = false;
                    foreach (var item in filteredActions)
                    {
                        if (item.ButtonName == unitAction)
                        {
                            alreadyThere = true;
                            break;
                        }
                    }

                    if (alreadyThere) continue;

                    // do a specific check for this action
                    // TODO: move this into a Func on the ActionType: addAction = actionType.DoSpecificCheck(Location);
                    bool addAction;
                    if (unitAction == "BuildOutpost")
                    {
                        addAction = CanSettleOnTerrain(LocationHex);
                    }
                    else
                    {
                        addAction = true;
                    }

                    if (addAction)
                    {
                        var action = _gameConfigCache.GetActionConfigByName(unitAction);
                        filteredActions.Add(action);
                    }
                }
            }

            var actionNames = new List<string>();
            foreach (var item in filteredActions)
            {
                actionNames.Add((string)item.Name);
            }

            return actionNames;
        }

        private bool CanSettleOnTerrain(PointI thisLocationHex)
        {
            // if terrain is settle-able
            var world = CallContext<World>.GetData("GameWorld");
            var cell = world.OverlandMap.CellGrid.GetCell(thisLocationHex);
            var terrain = _gameConfigCache.GetTerrainConfigById(cell.TerrainId);

            if (!terrain.CanSettleOn) return false;
            
            // and not within 4 distance from another settlement
            var settlements = world.Settlements;
            foreach (var settlement in settlements)
            {
                var distance = world.HexLibrary.GetDistance(new HexOffsetCoordinates(thisLocationHex.X, thisLocationHex.Y), new HexOffsetCoordinates(settlement.Location.X, settlement.Location.Y));
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
            foreach (var item in Units)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString() => DebuggerDisplay;

        private string DebuggerDisplay => $"{{Count={Units.Count}}}";
    }
}