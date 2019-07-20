using System.Collections.Generic;

namespace PhoenixGameLibrary
{
    public class MessageQueue
    {
        private Queue<string> _queue;

        internal MessageQueue()
        {
            _queue = new Queue<string>();
        }

        internal int Count => _queue.Count;

        public void Enqueue(string message)
        {
            _queue.Enqueue(message);
        }

        internal string Dequeue()
        {
            return _queue.Dequeue();
        }
    }
}