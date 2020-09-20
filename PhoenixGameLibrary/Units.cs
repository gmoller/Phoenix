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
            UnitsList.ForEach(unit => unit.DoPatrolAction());
        }

        internal void DoFortifyAction()
        {
            UnitsList.ForEach(unit => unit.DoFortifyAction());
        }

        internal void DoExploreAction()
        {
            UnitsList.ForEach(unit => unit.DoExploreAction());
        }

        internal void SetStatusToNone()
        {
            UnitsList.ForEach(unit => unit.SetStatusToNone());
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