using BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class AlertRepository
    {
        List<Alert> Alerts { get; set; }
        List<Notification> Notifications { get; set; }

        public AlertRepository()
        {
            Alerts = Memory.alerts;

        }

        public Alert ReadAlert(int alertId)
        {
            return Alerts.First(a => a.AlertId == alertId);
        }

        public void CreateAlert(Alert alert)
        {
            Alerts.Add(alert);
        }

        public void DeleteAlert(Alert alert)
        {
            Alerts.Remove(alert);

        }



        public void UpdateAlert(Alert alert)
        {
            Alert a = ReadAlert(alert.AlertId);
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
