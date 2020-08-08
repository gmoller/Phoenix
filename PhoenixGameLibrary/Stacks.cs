using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace PhoenixGameLibrary
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Stacks : IEnumerable<Stack>
    {
        private readonly List<Stack> _stacks;

        public Stack this[int index] => _stacks[index];

        public int Count => _stacks.Count;

        internal Stacks()
        {
            _stacks = new List<Stack>();
        }

        internal void Add(Stack stack)
        {
            _stacks.Add(stack);
        }

        internal void BeginTurn()
        {
            foreach (var stack in _stacks)
            {
                stack.BeginTurn();
            }
        }

        internal void EndTurn()
        {
            foreach (var stack in _stacks)
            {
                stack.EndTurn();
            }
        }

        public IEnumerator<Stack> GetEnumerator()
        {
            foreach (var item in _stacks)
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