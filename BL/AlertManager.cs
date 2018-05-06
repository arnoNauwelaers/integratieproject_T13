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
      socialMediaRepository = RepositoryFactory.CreateSocialMediaRepository();
            this.alertRepository = RepositoryFactory.CreateALertRepository();
        }

        public void VerifyCondition(Alert alert)
        {

              switch (alert.Parameter)
                {
        case AlertParameter.compared: CompareMentions(alert); break;
        case AlertParameter.comparedSentiment:CompareSentiment(alert); break;
        case AlertParameter.mentions:  CompareMentionsWithSelf(alert); break;
        case AlertParameter.sentiment:  CompareSentimentWithSelf(alert); break;
        
              }
            
    
       
            //int tweetAmount = socialMediaRepository.ReadItemParameter(alert, DateTime.Now, DateTime.Now.AddHours(-FREQUENTIE));
            //if (alert.CompareItem == null)
            //{
            //    int oldTweetAmount = socialMediaRepository.ReadItemParameter(alert, DateTime.Now.AddHours(-1), DateTime.Now.AddHours(-(FREQUENTIE * 2)));

            //    if (alert.Condition == ">")
            //    {
            //        //als een politicus 2 maal zoveel tweets stuurt in het laatste uur als in het vorige uur wordt er een notification gestuurd
            //        return tweetAmount >= (oldTweetAmount * 2);
            //    }
            //    return false;
            //}
            //else
            //{
            //    int tweetAmount2 = socialMediaRepository.ReadItemParameter(alert, DateTime.Now, DateTime.Now.AddHours(-FREQUENTIE));
            //    if (alert.Condition == ">")
            //    {
            //        //als er over een politus meer dan 2 maal zveel getweet is in het afgelopen uur als een ander politici word er een notification gestuurd
            //        return tweetAmount >= tweetAmount2 * 2;
            //    }

            //}
            //return false;
        }

    private void CompareSentimentWithSelf(Alert alert)
    {
      throw new NotImplementedException();
    }

    private void CompareSentiment(Alert alert)
    {
      throw new NotImplementedException();
    }

    private void CompareMentions(Alert alert)
    {
      throw new NotImplementedException();
    }

    private void CompareMentionsWithSelf(Alert alert)
    {
      int tweetAmount = socialMediaRepository.ReadNrOfPostsFromItem(alert.Item, DateTime.Now, DateTime.Now.AddHours(-FREQUENTIE));
      int oldTweetAmount = socialMediaRepository.ReadNrOfPostsFromItem(alert.Item, DateTime.Now.AddHours(-1), DateTime.Now.AddHours(-(FREQUENTIE * 2)));

      if (alert.ConditionPerc > 0)
      {
        if (tweetAmount >= oldTweetAmount + oldTweetAmount * (alert.ConditionPerc / 100))
        {
          if (!alertRepository.NotificationExists(alert.AlertId))
          {
            Notification notification = new Notification() { DateTime = DateTime.Now, Alert = alert };
            notification.Content = String.Format("{0} is populairder geworden", alert.Item.Name);
            alertRepository.CreateNotification(notification);
            alert.Notifications.Add(notification);
            alertRepository.UpdateAlert(alert);
            SendAlert(alert, notification);
          }


        }
          
      }
      else
      {
        if (tweetAmount <= oldTweetAmount - oldTweetAmount * (alert.ConditionPerc / 100))
        {
          if (!alertRepository.NotificationExists(alert.AlertId))
          {
            Notification notification = new Notification() { DateTime = DateTime.Now, Alert = alert };
            notification.Content = String.Format("{0} is minder populair geworden", alert.Item.Name);
            alertRepository.CreateNotification(notification);
            alert.Notifications.Add(notification);
            alertRepository.UpdateAlert(alert);
            SendAlert(alert, notification);
          }


        }
      }

    }

   

        

        

        //public void InspectAlert(Alert alert)
        //{
        //    bool condionAns = VerifyCondition(alert);
        //    int notificationNmr = 1;
        //    if (condionAns == true)
        //    {
        //        AlertType type = alert.Type;
        //        if (type != AlertType.mail)
        //        {
        //            //TODO controleer of notification al in database is opgenomen
        //            if (!alertRepository.NotificationExists(alert.AlertId))
        //            {
        //                Notification notification = new Notification() { NotificationId = notificationNmr, DateTime = DateTime.Now, Alert = alert };                                                
        //                alertRepository.CreateNotification(notification);
        //                alert.Notifications.Add(notification);
        //                //TODO klopt niet, moet een user wel als attribuut in Alert opgenomen worden?
        //                alertRepository.UpdateAlert(alert);
        //                notificationNmr++;
        //            }
        //        }
        //        if (type != AlertType.notification)
        //        {
        //            //TODO Mail versturen naar gebruiker?
        //        }
        //    }
        //}

       public void HandleAlerts(List<Alert> alerts)
    {
      foreach(Alert a in alerts)
      {
        VerifyCondition(a);
      }
    }

        private void SendAlert(Alert a,Notification n)
    {
      foreach (AlertType t in a.Type)
      {
        switch (t)
        {
          case AlertType.mail: SendMail(a.User, n); break;
          case AlertType.notification: sendNotification(a.User, n); break;
          case AlertType.pushbericht: sendPush(a.User, n); break;

        }
      }
    }

    private void sendPush(ApplicationUser u, Notification n)
    {

      // push bericht naar android, TODO
      throw new NotImplementedException();
    }

    private void sendNotification(ApplicationUser u, Notification n)
    {
      throw new NotImplementedException();
    }

    private void SendMail(ApplicationUser u, Notification n)
    {

      Mail.sendMail(u.Email, "Nieuwe social media trend", n.Content);
      
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
