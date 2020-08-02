using System;
using System.Diagnostics;

namespace Utilities
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class NamedDataList<T> : DataList<T> where T : IIdentifiedById, IIdentifiedByIdAndName
    {
        public T this[string name]
        {
            get
            {
                foreach (var item in this)
                {
                    if (item.Name == name)
                    {
                        return item;
                    }
                }

                throw new IndexOutOfRangeException($"Index out of range. Item with name [{name}] not found.");
            }
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Count={Items.Count}}}";
    }
}