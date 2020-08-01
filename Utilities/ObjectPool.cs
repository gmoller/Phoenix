using System;
using System.Collections.Generic;

namespace Utilities
{
    public class ObjectPool<T> : IDisposable
    {
        private readonly Stack<T> _stack;

        /// <summary>
        /// Creates a new object pool.
        /// </summary>
        public ObjectPool()
        {
            _stack = new Stack<T>();
        }

        public bool HasFreeObject => _stack.Count > 0;

        /// <summary>
        /// Puts an object into the pool.
        /// </summary>
        /// <param name="obj"></param>
        public void Add(T obj)
        {
            _stack.Push(obj);
        }

        /// <summary>
        /// Retrieves an object from the pool.
        /// </summary>
        /// <returns></returns>
        public T Get()
        {
            T obj = _stack.Pop();

            return obj;
        }

        public void Dispose()
        {
            _stack.Clear();
        }
    }
}