﻿using BL;
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
            UnitOfWorkManager unitOfWorkManager = new UnitOfWorkManager();
            SocialMediaManager = new SocialMediaManager(unitOfWorkManager);
            AlertManager = new AlertManager(unitOfWorkManager);
            UserManager = new ApplicationUserManager(unitOfWorkManager);
            UserManager.SetSocialMediaManager(SocialMediaManager);
            ChartManager = new ChartManager(unitOfWorkManager);
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

        [Route("api/Basic/GetNotifications")]
        [Authorize]
        public IHttpActionResult GetNotifications()
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

        [Route("api/Basic/Login")]
        [HttpPost]
        public IHttpActionResult Login(string username, string password, string email = "")
        {
            var user = UserManager.FindAsync(username, password);
            if (user != null)
            {
                //UserManager.Sig(user, true);
            }
            else
            {
            }
            return Ok();
        }

        [Route("api/Basic/LoginFacebook")]
        [HttpPost]
        public IHttpActionResult LoginFacebook()
        {
            return Ok();
        }

        [Route("api/Basic/LoginGoogle")]
        [HttpPost]
        public IHttpActionResult LoginGoogle()
        {
            return Ok();
        }
    }
}