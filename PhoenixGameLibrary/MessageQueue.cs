using System.Collections.Generic;
using PhoenixGameLibrary.Commands;

namespace PhoenixGameLibrary
{
    public class MessageQueue
    {
        private Queue<Command> _queue;

        internal MessageQueue()
        {
            _queue = new Queue<Command>();
        }

        internal int Count => _queue.Count;

        public void Enqueue(Command command)
        {
            _queue.Enqueue(command);
        }

        internal Command Dequeue()
        {
            return _queue.Dequeue();
        }
    }
}