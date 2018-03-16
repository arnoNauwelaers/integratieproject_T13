using BL.Domain;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class NotificationManager
    {
        private NotificationRepository repo;

        public NotificationManager()
        {
            this.repo = new NotificationRepository();
        }

        public Notification AddNotification(string message, User user, string location)
        {
            return repo.CreateNotification(message, user, location);
        }

        public Notification GetNotification(int id)
        {
            return repo.ReadNotification(id);
        }

        public IEnumerable<Notification> GetNotifications()
        {
            return repo.ReadNotifications();
        }
    }
}
