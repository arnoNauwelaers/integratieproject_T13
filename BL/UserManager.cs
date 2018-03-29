using BL.Domain;
using DAL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class UserManager
    {
        private AlertRepository alertRepository;
        private SocialMediaManager socialMediaManager;

        public UserManager(SocialMediaManager socialMediaManager)
        {
            alertRepository = new AlertRepository();
            this.socialMediaManager = socialMediaManager;
        }

        public List<Alert> GetAlerts(Item item)
        {
           return alertRepository.GetAlerts(item);
        }

        public void InspectAlert(Alert alert)
        {
            bool condionAns = socialMediaManager.VerifyCondition(alert);
            int notificationNmr = 1;
            if (condionAns == true)
            {
                AlertType type = alert.Type;
                if(type != AlertType.mail)
                {
                    Notification notification = new Notification() { NotificationId = notificationNmr, DateTime = DateTime.Now, Alert = alert};
                    notificationNmr++;
                    Debug.WriteLine("LOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOL");
                    Debug.WriteLine("new not made");
                    Debug.WriteLine("LOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOL");
                }
                if (type != AlertType.notification)
                {
                    this.sendMail(alert);
                }
            }
        }

        private void sendMail(Alert alert)
        {
            //TODO: implementeer send mail
        }
    }
}
