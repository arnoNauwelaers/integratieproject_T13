using BL.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;
using DAL.EF;

namespace DAL.Repositories
{
    public class AlertRepository
    {
        private readonly BarometerDbContext ctx;

        public AlertRepository(UnitOfWork uow)
        {
      this.ctx = uow.Context;
        }

        public List<Alert> ReadAlerts()
        {
            return ctx.Alerts.Include(a => a.Notifications).ToList<Alert>();
        }

        public Alert CreateAlert(Alert alert)
        {
            ctx.Alerts.Add(alert);
            ctx.SaveChanges();
            return alert;
        }

        public Boolean NotificationExists(int id)
        {
            var todayDate = DateTime.Today;
            if (ctx.Notifications.Where(n => (n.Alert.AlertId == id) && (n.DateTime.Day == todayDate.Day && n.DateTime.Month == todayDate.Month && n.DateTime.Year == todayDate.Year)).Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
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
            //ctx.Entry(notification).State = System.Data.Entity.EntityState.Modified;
            ctx.Set<Notification>().AddOrUpdate(notification);
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
