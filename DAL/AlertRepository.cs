using BL.Domain;
using System;
using System.Collections.Generic;

namespace DAL
{
    public class AlertRepository
    {
        List<Alert> Alerts { get; set; }
        //List<Notification> Notifications { get; set; }

        public AlertRepository()
        {
            Alerts = Memory.alerts;
        }

        public List<Alert> GetAlerts(Item item)
        {
            List<Alert> usedAlerts = new List<Alert>();
            foreach (var alert in Alerts)
            {
                if (alert.Item.ItemId == item.ItemId)
                {
                    usedAlerts.Add(alert);
                }
            }
            return usedAlerts;
        }
    }
}
