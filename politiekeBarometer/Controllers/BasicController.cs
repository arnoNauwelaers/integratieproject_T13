using BL;
using BL.Domain;
using System.Collections.Generic;
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

        private List<Notification> notifications = new List<Notification>();
        public IHttpActionResult Get()
        {
            SynchronizeDatabase();
            User user = UserManager.GetUser();
            foreach (var alert in user.Alerts)
            {
                foreach (var notification in alert.Notifications)
                {
                    if (notification.Read == false)
                    {
                        notifications.Add(notification);
                        notification.Read = true;
                        UserManager.UpdateNotification(notification);
                    }
                }
            }
            return Ok(notifications);
        }

        //TODO return al gelezen notifications van vandaag en ongelezen notifications andere dagen. ReadDate toevoegen aan Notification naast Read boolean?
        [Route("api/Basic/GetNotifications")]
        public IHttpActionResult GetNotifications()
        {
            return Get();
        }
    }
}