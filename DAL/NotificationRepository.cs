using BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class NotificationRepository
    {
        private static IList<Notification> Notifications = new List<Notification>();

        public Notification CreateNotification(string message, User user, string location)
        {
            Notification tempNotification = new Notification() { Message = message, User = user, Location = location };
            Notifications.Add(tempNotification);
            return tempNotification;
        }

        public Notification ReadNotification(int id)
        {
            foreach (var n in Notifications)
            {
                if (n.Id == id) return n;
            }
            return null;
        }

        public IEnumerable<Notification> ReadNotifications()
        {
            return Notifications;
        }
    }
}
