using BL;
using BL.Domain;
using System.Collections.Generic;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace politiekeBarometer.Controllers
{
    public class BasicController : ApiController
    {
        private SocialMediaManager SocialMediaManager;
        private ApplicationUserManager UserManager;

        protected BasicController()
        {
            SocialMediaManager = new SocialMediaManager();
            UserManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            UserManager.setSocialMediaManager(SocialMediaManager);
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

        public IHttpActionResult Get()
        {
            List<Notification> notifications = new List<Notification>();
            SynchronizeDatabase();
            if (User.Identity.GetUserId() != null)
            {
                ApplicationUser user = UserManager.GetUser(User.Identity.GetUserId());
                if (user.Alerts.Count > 0)
                {
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

                    
                }
                return Ok(notifications);
            }
            else
            {
                return Ok();
            }
        }

        //TODO return al gelezen notifications van vandaag en ongelezen notifications andere dagen. ReadDate toevoegen aan Notification naast Read boolean?
        [Route("api/Basic/GetNotifications")]
        public IHttpActionResult GetNotifications()
        {
            List<Notification> notifications = new List<Notification>();
            SynchronizeDatabase();
            if (User.Identity.GetUserId() != null)
            {
                ApplicationUser user = UserManager.GetUser(User.Identity.GetUserId());
                if (user.Alerts.Count > 0)
                {
                    foreach (var alert in user.Alerts)
                    {
                        foreach (var notification in alert.Notifications)
                        {
                                notifications.Add(notification);
                            }
                        }
                    }


                }
                return Ok(notifications);
        }
    }
}