using BL.Domain;
using DAL.EF;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Managers
{
    public class AlertManager
    {
        private const int FREQUENTIE = 1;

        private AlertRepository alertRepository;
        private SocialMediaRepository socialMediaRepository;


        public AlertManager()
        {
      socialMediaRepository = RepositoryFactory.CreateSocialMediaRepository();
            this.alertRepository = RepositoryFactory.CreateAlertRepository();
        }

        public void VerifyCondition(Alert alert)
        {

              switch (alert.Parameter)
                {
        case AlertParameter.compared: CompareNrOfPosts(alert); break;
        case AlertParameter.comparedSentiment:CompareSentiment(alert); break;
        case AlertParameter.mentions:  CompareNrOfPostsWithSelf(alert); break;
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
      double newSentiment = socialMediaRepository.ReadAverageSentimentFromItem(alert.Item, DateTime.Now, DateTime.Now.AddHours(-FREQUENTIE));
      double oldSentiment = socialMediaRepository.ReadAverageSentimentFromItem(alert.Item, DateTime.Now.AddHours(-FREQUENTIE), DateTime.Now.AddHours(-(FREQUENTIE * 2)));
      if (newSentiment >= oldSentiment * (alert.ConditionPerc / 100))
      {
        Notification n = new Notification() { DateTime = DateTime.Now, Alert = alert };
        n.Content = String.Format("{0} wordt nu {1}% positiever gezien dan eerder", alert.Item.Name, alert.ConditionPerc);
        alertRepository.CreateNotification(n);
        alert.Notifications.Add(n);
        alertRepository.UpdateAlert(alert);
        SendAlert(alert, n);
      } ;
    }

    private void CompareSentiment(Alert alert)
    {
      double s1 = socialMediaRepository.ReadAverageSentimentFromItem(alert.Item, DateTime.Now, DateTime.Now.AddHours(-FREQUENTIE));
      double s2 = socialMediaRepository.ReadAverageSentimentFromItem(alert.CompareItem, DateTime.Now, DateTime.Now.AddHours(-FREQUENTIE));
      if (s1 >= s2 * (alert.ConditionPerc / 100))
      {
        Notification n = new Notification() { DateTime = DateTime.Now, Alert = alert };
        n.Content = String.Format("{0} wordt nu {1}% zo positief als {2} gezien", alert.Item.Name, alert.ConditionPerc, alert.CompareItem.Name);
        alertRepository.CreateNotification(n);
        alert.Notifications.Add(n);
        alertRepository.UpdateAlert(alert);
        SendAlert(alert, n);
      }
    }

    private void CompareNrOfPosts(Alert alert)
    {
      int tweetAmountItem1 = socialMediaRepository.ReadNrOfPostsFromItem(alert.Item, DateTime.Now, DateTime.Now.AddHours(-FREQUENTIE));
      int tweetAmountItem2 = socialMediaRepository.ReadNrOfPostsFromItem(alert.CompareItem, DateTime.Now, DateTime.Now.AddHours(-FREQUENTIE));
      if(tweetAmountItem1 >= tweetAmountItem2 * (alert.ConditionPerc / 100))
      {
        Notification n = new Notification() { DateTime = DateTime.Now, Alert = alert };
        n.Content = String.Format("{0} is nu meer dan {1}% zo populair als {2}",alert.Item.Name,alert.ConditionPerc,alert.CompareItem.Name);
        alertRepository.CreateNotification(n);
        alert.Notifications.Add(n);
        alertRepository.UpdateAlert(alert);
        SendAlert(alert, n);
      }
    }

    private void CompareNrOfPostsWithSelf(Alert alert)
    {
      int tweetAmount = socialMediaRepository.ReadNrOfPostsFromItem(alert.Item, DateTime.Now, DateTime.Now.AddHours(-FREQUENTIE));
      int oldTweetAmount = socialMediaRepository.ReadNrOfPostsFromItem(alert.Item, DateTime.Now.AddHours(-FREQUENTIE), DateTime.Now.AddHours(-(FREQUENTIE * 2)));

      if (alert.ConditionPerc > 100)
      {
        if (tweetAmount >=oldTweetAmount * (alert.ConditionPerc / 100))
        {
          if (!alertRepository.NotificationExists(alert.AlertId))
          {
            Notification notification = new Notification() { DateTime = DateTime.Now, Alert = alert };
            notification.Content = String.Format("Er wordt meer over {0} gepraat op sociale media", alert.Item.Name);
            alertRepository.CreateNotification(notification);
            alert.Notifications.Add(notification);
            alertRepository.UpdateAlert(alert);
            SendAlert(alert, notification);
          }


        }
          
      }
      else
      {
        if (tweetAmount <= oldTweetAmount  * (alert.ConditionPerc / 100))
        {
          if (!alertRepository.NotificationExists(alert.AlertId))
          {
            Notification notification = new Notification() { DateTime = DateTime.Now, Alert = alert };
            notification.Content = String.Format("Er wordt minder over {0} gepraat op sociale media", alert.Item.Name);
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
          case AlertType.notification: SendNotification(a.User, n); break;
          case AlertType.pushbericht: SendPush(a.User, n); break;

        }
      }
    }

    private void SendPush(ApplicationUser u, Notification n)
    {

      // push bericht naar android, TODO
      throw new NotImplementedException();
    }

    private void SendNotification(ApplicationUser u, Notification n)
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
