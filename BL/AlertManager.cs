using BL.Domain;
using DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class AlertManager
    {
        private SocialMediaManager socialMediaManager;
        private AlertRepository alertRepository;

        public AlertManager()
        {
            this.socialMediaManager = new SocialMediaManager();
            this.alertRepository = new AlertRepository();
        }

        public void InspectAlert(Alert alert)
        {
            bool condionAns = socialMediaManager.VerifyCondition(alert);
            int notificationNmr = 1;
            if (condionAns == true)
            {
                AlertType type = alert.Type;
                if (type != AlertType.mail)
                {
                    //TODO controleer of notification al in database is opgenomen
                    if (!alertRepository.NotificationExists(alert.AlertId))
                    {
                        Notification notification = new Notification() { NotificationId = notificationNmr, DateTime = DateTime.Now, Alert = alert };
                        alertRepository.CreateNotification(notification);
                        alert.Notifications.Add(notification);
                        //TODO klopt niet, moet een user wel als attribuut in Alert opgenomen worden?
                        alertRepository.UpdateAlert(alert);
                        notificationNmr++;
                    }
                }
                if (type != AlertType.notification)
                {
                    //TODO Mail versturen naar gebruiker?
                }
            }
        }

        public List<Alert> GetAlerts(Item item)
        {
            return alertRepository.GetAlerts(item);
        }

        public void UpdateNotification(Notification notification)
        {
            alertRepository.UpdateNotification(notification);
        }
    }
}
