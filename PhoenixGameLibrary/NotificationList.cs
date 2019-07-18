using System.Collections;
using System.Collections.Generic;

namespace PhoenixGameLibrary
{
    public class NotificationList : IEnumerable<string>
    {
        private List<string> _notifications;

        public NotificationList()
        {
            _notifications = new List<string>();
        }

        public void Add(string notification)
        {
            _notifications.Add(notification);
        }

        public void Clear()
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