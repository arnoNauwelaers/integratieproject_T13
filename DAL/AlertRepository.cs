using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class AlertRepository
    {
        private IList<Alert> alerts = new List<Alert>();


        Alert alert = new Alert("OnlyNotification", "AantalTweets", 1);

        public AlertRepository()
        {
            alerts.Add(alert);
        }

        public Alert getAlert(int alertId)
        {
            foreach (Alert alert in alerts)
            {
                if (alert.Id == alertId)
                {
                    return alert;
                }
            }
            return null;
        }
    }
}
