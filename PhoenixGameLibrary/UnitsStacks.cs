﻿using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace PhoenixGameLibrary
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class UnitsStacks : IEnumerable<UnitsStack>
    {
        private readonly List<UnitsStack> _unitsStacks;

        public UnitsStack this[int index] => _unitsStacks[index];

        public int Count => _unitsStacks.Count;

        internal UnitsStacks()
        {
            _unitsStacks = new List<UnitsStack>();
        }

        internal void Add(UnitsStack unitsStack)
        {
            _unitsStacks.Add(unitsStack);
        }

        internal void BeginTurn()
        {
            foreach (var unitsStack in _unitsStacks)
            {
                unitsStack.BeginTurn();
            }
        }

        internal void EndTurn()
        {
            foreach (var unitsStack in _unitsStacks)
            {
                unitsStack.EndTurn();
            }
        }

        public IEnumerator<UnitsStack> GetEnumerator()
        {
            foreach (var item in _unitsStacks)
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

        private string DebuggerDisplay => $"{{Count={Count}}}";
    }
}