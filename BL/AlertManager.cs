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
        private const int FREQUENTIE = 1;

        private AlertRepository alertRepository;
        private SocialMediaRepository socialMediaRepository;


        public AlertManager()
        {
            socialMediaRepository = new SocialMediaRepository();
            this.alertRepository = new AlertRepository();
        }

        public Boolean VerifyCondition(Alert alert)
        {
            int tweetAmount = socialMediaRepository.ReadItemParameter(alert, DateTime.Now, DateTime.Now.AddHours(-FREQUENTIE));
            if (alert.CompareItem == null)
            {
                int oldTweetAmount = socialMediaRepository.ReadItemParameter(alert, DateTime.Now.AddHours(-1), DateTime.Now.AddHours(-(FREQUENTIE * 2)));

                if (alert.Condition == ">")
                {
                    //als een politicus 2 maal zoveel tweets stuurt in het laatste uur als in het vorige uur wordt er een notification gestuurd
                    return tweetAmount >= (oldTweetAmount * 2);
                }
                return false;
            }
            else
            {
                int tweetAmount2 = socialMediaRepository.ReadItemParameter(alert, DateTime.Now, DateTime.Now.AddHours(-FREQUENTIE));
                if (alert.Condition == ">")
                {
                    //als er over een politus meer dan 2 maal zveel getweet is in het afgelopen uur als een ander politici word er een notification gestuurd
                    return tweetAmount >= tweetAmount2 * 2;
                }

            }
            return false;
        }

        public void InspectAlert(Alert alert)
        {
            bool condionAns = VerifyCondition(alert);
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
