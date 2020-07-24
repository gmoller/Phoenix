using System;
using System.Collections.Generic;
using System.Linq;

namespace Utilities
{
    public class PriorityQueue<T> where T : IComparable<T>
    {
        private readonly List<T> _items;

        public int Count => _items.Count;
        public bool IsEmpty => Count == 0;

        public PriorityQueue()
        {
            _items = new List<T>();
        }

        public void Enqueue(T itemToAdd)
        {
            _items.Add(itemToAdd);
            
            var childIndex = _items.Count - 1;
            while (childIndex > 0)
            {
                var parentIndex = (childIndex - 1) / 2;
                var childItem = _items[childIndex];
                var parentItem = _items[parentIndex];
                if (childItem.CompareTo(parentItem) >= 0)  break;
                var tmp = childItem;
                _items[childIndex] = parentItem;
                _items[parentIndex] = tmp;
                childIndex = parentIndex;
            }
        }

        public T Dequeue()
        {
            if (IsEmpty) throw new Exception("Priority queue is empty.");

            // Assumes priority queue isn't empty
            var lastIndex = _items.Count - 1;
            var frontItem = _items[0];
            _items[0] = _items[lastIndex];
            _items.RemoveAt(lastIndex);

            --lastIndex;
            var parentIndex = 0;
            while (true)
            {
                var childIndex = parentIndex * 2 + 1;
                if (childIndex > lastIndex) break;
                var rc = childIndex + 1;
                var childItem = _items[childIndex];
                if (rc <= lastIndex && _items[rc].CompareTo(childItem) < 0)
                {
                    childIndex = rc;
                }

                var parentItem = _items[parentIndex];
                if (parentItem.CompareTo(childItem) <= 0) break;
                var tmp = parentItem;
                _items[parentIndex] = childItem;
                _items[childIndex] = tmp;
                parentIndex = childIndex;
            }

            return frontItem;
        }

        public T RemoveRoot()
        {
            var lastIndex = _items.Count - 1;
            var frontItem = _items[0];
            var tempItem = _items[lastIndex];
            _items.RemoveAt(lastIndex);

            if (_items.Count > 0)
            {
                var i = 0;
                while (i < _items.Count / 2)
                {
                    var j = (2 * i) + 1;
                    if ((j < _items.Count - 1) && (_items[j].CompareTo(_items[j + 1]) > 0)) ++j;
                    if (_items[j].CompareTo(tempItem) >= 0) break;
                    _items[i] = _items[j];
                    i = j;
                }
                _items[i] = tempItem;
            }

            return frontItem;
        }

        public T Peek()
        {
            if (IsEmpty) throw new Exception("Priority queue is empty.");

            var frontItem = _items[0];

            return frontItem;
        }

        public bool IsConsistent()
        {
            if (IsEmpty) return true;

            var lastIndex = _items.Count - 1;
            for (var parentIndex = 0; parentIndex < _items.Count; ++parentIndex)
            {
                var leftChildIndex = 2 * parentIndex + 1;
                var rightChildIndex = 2 * parentIndex + 2;

                var parentItem = _items[parentIndex];

                var leftChild = _items[leftChildIndex];
                if (leftChildIndex <= lastIndex && parentItem.CompareTo(leftChild) > 0) return false;

                var rightChild = _items[rightChildIndex];
                if (rightChildIndex <= lastIndex && parentItem.CompareTo(rightChild) > 0) return false;
            }

            return true; // Passed all checks
        }

        public override string ToString()
        {
            var s = _items.Aggregate(string.Empty, (current, t) => current + $"{t} ");

            s += $"count = {_items.Count}";

            return s;
        }
    }
}