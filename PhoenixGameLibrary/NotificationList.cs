using System.Collections;
using System.Collections.Generic;

namespace PhoenixGameLibrary
{
    public class NotificationList : IEnumerable<string>
    {
        private readonly List<string> _notifications;

        internal NotificationList()
        {
            _notifications = new List<string>();
        }

        internal void Add(string notification)
        {
            _notifications.Add(notification);
        }

        internal void Clear()
        {
            if (_notifications.Count > 0)
            {
                _notifications.Clear();
            }
        }

        public IEnumerator<string> GetEnumerator()
        {
            foreach (string item in _notifications)
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