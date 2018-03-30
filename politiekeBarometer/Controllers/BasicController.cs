using BL;
using BL.Domain;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Http;

namespace politiekeBarometer.Controllers
{
    public class BasicController : ApiController
    {
        private SocialMediaManager SocialMediaManager;
        private UserManager UserManager;

        protected BasicController()
        {
            SocialMediaManager = new SocialMediaManager();
            UserManager = new UserManager(SocialMediaManager);
        }

        public void SynchronizeDatabase()
        {
            List<Item> alteredItems = SocialMediaManager.CreatePosts();
            List<Alert> alerts = new List<Alert>();
            foreach (var item in alteredItems)
            {
                alerts = UserManager.GetAlerts(item);
                foreach (var alert in alerts)
                {
                    UserManager.InspectAlert(alert);
                }
            }

        }

        //------------------------------------------------------------------------------------------------------------------------------------------------
        //WEB API
        private static List<Notification> notifications = new List<Notification>();
        public IHttpActionResult Get()
        {
            SynchronizeDatabase();
            User user = UserManager.GetUser();
            List<Notification> tempNotifications = new List<Notification>();
            foreach (var alert in UserManager.GetUser().Alerts)
            {
                foreach (var notification in alert.Notifications)
                {
                    foreach (var notification2 in notifications)
                    {
                        if (notification.NotificationId != notification2.NotificationId)
                        {
                            tempNotifications.Add(notification);
                            notifications.Add(notification);
                        }
                    }
                    if (notifications.Count == 0)
                    {
                        tempNotifications.Add(notification);
                        notifications.Add(notification);
                    }
                    //notification.Read = true;
                }
            }
            return Ok(tempNotifications);
        }

        [Route("api/Basic/GetNotifications")]
        public IHttpActionResult GetNotifications()
        {
            return Ok(notifications);
        }
    }
}