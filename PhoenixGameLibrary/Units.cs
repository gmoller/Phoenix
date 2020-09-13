using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PhoenixGameLibrary
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Units : IEnumerable<Unit>
    {
        #region State
        private List<Unit> UnitsList { get; }
        #endregion End State

        public Unit this[int index] => UnitsList[index];

        internal int Count => UnitsList.Count;

        internal Units()
        {
            UnitsList = new List<Unit>();
        }

        internal void Add(Unit unit)
        {
            UnitsList.Add(unit);
        }

        internal void Remove(Unit unit)
        {
            UnitsList.Remove(unit);
        }

        internal void DoPatrolAction()
        {
            foreach (var unit in UnitsList)
            {
                unit.DoPatrolAction();
            }
        }

        internal void DoFortifyAction()
        {
            foreach (var unit in UnitsList)
            {
                unit.DoFortifyAction();
            }
        }

        internal void DoExploreAction()
        {
            foreach (var unit in UnitsList)
            {
                unit.DoExploreAction();
            }
        }

        internal void SetStatusToNone()
        {
            foreach (var unit in UnitsList)
            {
                unit.SetStatusToNone();
            }
        }

        internal List<Unit> GetUnitsByAction(string actionType)
        {
            return UnitsList.Where(unit => unit.Actions.Contains(actionType)).ToList();
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Count={UnitsList.Count}}}";

        public IEnumerator<Unit> GetEnumerator()
        {
            foreach (var item in UnitsList)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}