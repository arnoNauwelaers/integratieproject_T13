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
using BL.Managers;
//TODO meer code van controller naar managers verplaatsen om inhoud minimaal te houden?
namespace politiekeBarometer.Controllers
{
    public class BasicController : ApiController
    {
        private SocialMediaManager SocialMediaManager;
        private ApplicationUserManager UserManager;
        private AlertManager AlertManager;
        private ChartManager ChartManager;

        protected BasicController()
        {
            SocialMediaManager = new SocialMediaManager();
            AlertManager = new AlertManager();
            UserManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            UserManager.SetSocialMediaManager(SocialMediaManager);
            ChartManager = new ChartManager();
        }


        [Authorize]
        public IHttpActionResult Get()
        {
            List<Notification> notifications = new List<Notification>();
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
                                AlertManager.UpdateNotification(notification);
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
        [Authorize]
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
        //TODO 27/04 values worden niet gepost?
        [HttpPost]
        [Authorize]
        public IHttpActionResult AddChart(string json)
        {
            var Chart = JObject.Parse(json).ToObject<TempChartAdd>();
            ApplicationUser user = UserManager.GetUser(User.Identity.GetUserId());
            Chart chart = ChartManager.CreateChartFromDashboard(Chart.Items, Chart.ChartType, Chart.ChartValue, Chart.DateFrequency);
            user.Dashboard.Add(chart);
            UserManager.Update(user);
            return Ok(chart.ChartItemData);
        }

        [Route("api/Basic/EditChart")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult EditChart([FromBody] string json)
        {
            var Charts = JArray.Parse(json).ToObject<List<TempChartEdit>>();
            ChartManager.EditCharts(Charts);
            return Ok();
        }
  }
}