using BL.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EF
{
    public class AlertRepository
    {
        private BarometerDbContext ctx;

        public AlertRepository()
        {
            ctx = new BarometerDbContext();
            ctx.Database.Initialize(false);
        }

        public IEnumerable<Alert> ReadAlerts()
        {
            return ctx.Alerts.Include(a => a.Notifications).ToList<Alert>();
        }

        public Alert CreateAlert(Alert alert)
        {
            ctx.Alerts.Add(alert);
            ctx.SaveChanges();
            return alert;
        }

        public void UpdateAlert(Alert alert)
        {
            ctx.Entry(alert).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();
        }

        public void DeleteAlert(int alertId)
        {
            Alert alert = ctx.Alerts.Find(alertId);
            ctx.Alerts.Remove(alert);
            ctx.SaveChanges();
        }

        public List<Alert> GetAlerts(Item item)
        {
            List<Alert> usedAlerts = new List<Alert>();
            foreach (var bla in ctx.Alerts.ToList<Alert>())
            {
                if (bla.Item == null)
                {
                    Debug.WriteLine("NULL");
                }
                else
                {
                    Debug.WriteLine(bla.Item.ItemId + " ID");
                }
            }
            foreach (var alert in ctx.Alerts.ToList<Alert>())
            {
                if (alert.Item.ItemId == item.ItemId)
                {
                    usedAlerts.Add(alert);
                }
            }
            return usedAlerts;
        }

        public IEnumerable<Notification> ReadNotifications()
        {
            return ctx.Notifications.ToList<Notification>();
        }

        public Notification ReadNotification(int notificationId)
        {
            return ctx.Notifications.Find(notificationId);
        }

        public Notification CreateNotification(Notification notification)
        {
            ctx.Notifications.Add(notification);
            ctx.SaveChanges();
            return notification;
        }

        public void UpdateNotification(Notification notification)
        {
            ctx.Entry(notification).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();
        }

        public void DeleteNotification(int notificationId)
        {
            Notification notification = ctx.Notifications.Find(notificationId);
            ctx.Notifications.Remove(notification);
            ctx.SaveChanges();
        }
    }
}
