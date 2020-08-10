using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using PhoenixGameLibrary.GameData;
using Utilities;

namespace PhoenixGameLibrary
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Stack : IEnumerable<Unit>
    {
        private readonly Units _units;

        public UnitStatus Status { get; private set; }

        public Unit this[int index] => _units[index];

        public Point Location => _units[0].Location;

        public float MovementPoints => DetermineMovementPoints();
        public EnumerableList<string> MovementTypes => new EnumerableList<string>(DetermineMovementTypes());
        public EnumerableList<string> Actions => new EnumerableList<string>(DetermineActions(_units));

        public int Count => _units.Count;
        public bool IsBusy => Status == UnitStatus.Patrol || Status == UnitStatus.Fortify; // || Status == UnitStatus.Explore;

        public Stack(Units units)
        {
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

        public void SetStatusToNone()
        {
            Status = UnitStatus.None;
            _units.SetStatusToNone();
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
            var movementActions = new List<string>();
            foreach (var actionType in Globals.Instance.ActionTypes)
            {
                if (actionType.AppliesToAll)
                {
                    if (actionType.AppliesToAll)
                    {
                        movementActions.Add(actionType.ButtonName);
                    }
                }
            }

            foreach (var unit in units)
            {
                var unitActions = unit.Actions;
                foreach (var action in unitActions)
                {
                    if (!movementActions.Contains(action))
                    {
                        movementActions.Add(action);
                    }
                }
            }

            return movementActions;
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