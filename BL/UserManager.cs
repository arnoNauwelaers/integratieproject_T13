using BL.Domain;
using DAL;
using DAL.EF;
using System;
using System.Collections.Generic;

namespace BL
{
    public class UserManager
    {
        private UserRepository userRepository;
        private AlertRepository alertRepository;
        private SocialMediaManager socialMediaManager;

        public UserManager()
        {
        }

        public UserManager(SocialMediaManager socialMediaManager)
        {
            alertRepository = new AlertRepository();
            userRepository = new UserRepository();
            this.socialMediaManager = socialMediaManager;
        }

        public List<Alert> GetAlerts(Item item)
        {
           return alertRepository.GetAlerts(item);
        }

        public User GetUser()
        {
            return userRepository.GetUser();
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
                    alertRepository.CreateNotification(notification);
                    alert.Notifications.Add(notification);
                    alertRepository.UpdateAlert(alert);
                    notificationNmr++;
                }
                if (type != AlertType.notification)
                {
                    this.sendMail(alert);
                }
            }
        }

        public void UpdateNotification(Notification notification)
        {
            alertRepository.UpdateNotification(notification);
        }

        private void sendMail(Alert alert)
        {
            //TODO: implementatie send mail
        }
    }
}
