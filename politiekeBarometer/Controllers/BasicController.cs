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
using Newtonsoft.Json.Linq;
//TODO meer code van controller naar managers verplaatsen om inhoud minimaal te houden?
namespace politiekeBarometer.Controllers
{
    public class BasicController : ApiController
    {
        private SocialMediaManager SocialMediaManager;
        private ApplicationUserManager UserManager;
        private AlertManager alertManager;

        protected BasicController()
        {
            SocialMediaManager = new SocialMediaManager();
            UserManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            UserManager.setSocialMediaManager(SocialMediaManager);
        }

        

        public IHttpActionResult Get()
        {
            List<Notification> notifications = new List<Notification>();
            SocialMediaManager.SynchronizeDatabase();
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
                                alertManager.UpdateNotification(notification);
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
            SocialMediaManager.SynchronizeDatabase();
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

        [Route("api/Basic/AddChart")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult EditChart(string json)
        {
            var Charts = JObject.Parse(json).ToObject<List<TempChart>>();

            return Ok();
        }
    }
}