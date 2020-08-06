using System.Collections;
using System.Collections.Generic;

namespace Utilities
{
    public class EnumerableDictionary<T> : IEnumerable<T>
    {
        private readonly Dictionary<string, T> _dict;

        public T this[string key] => _dict[key];

        public int Count => _dict.Count;

        public EnumerableDictionary(Dictionary<string, T> list)
        {
            _dict = list;
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in _dict)
            {
                yield return item.Value;
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