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
using System.Security.Claims;
using System.Text;
using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using Newtonsoft.Json;

namespace politiekeBarometer.Controllers
{
    public class AppController : ApiController
    {
        private const string SECURITY_KEY = "q4rgq4rj8rsq4jqds64hsgd64jd";

        private SocialMediaManager SocialMediaManager;
        private ApplicationUserManager UserManager;
        private AlertManager AlertManager;
        private ChartManager ChartManager;

        protected AppController()
        {
            UnitOfWorkManager unitOfWorkManager = new UnitOfWorkManager();
            SocialMediaManager = new SocialMediaManager(unitOfWorkManager);
            AlertManager = new AlertManager(unitOfWorkManager);
            UserManager = new ApplicationUserManager(unitOfWorkManager);
            UserManager.SetSocialMediaManager(SocialMediaManager);
            ChartManager = new ChartManager(unitOfWorkManager);
        }

        [HttpGet]
        [Route("api/test")]
        public IHttpActionResult Test()
        {
            return Ok("API Call Succesfull");
        }

        [HttpPost]
        [Route("api/RequestToken")]
        public IHttpActionResult RequestToken([FromBody] TokenRequest request)
        {
            var user = UserManager.Find(request.Username, request.Password);
            Debug.WriteLine(request);
            if (user != null)
            {
                var claims = new[]
                {
            new Claim(ClaimTypes.Name, request.Username),
            new Claim("AppUser", "")
        };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECURITY_KEY));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    issuer: "kdg.t13.polbar.com",
                    audience: "kdg.t13.polbar.com",
                    claims: claims,
                    signingCredentials: creds);
                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                user.AppToken = tokenString;
                UserManager.Update(user);
                return Ok(tokenString);
            }
            return BadRequest("Could not verify username and password");
        }

        [HttpGet]
        [Route("api/GetUserInfo")]
        public IHttpActionResult GetUserData()
        {
            string token = Request.Headers.GetValues("Authorization").First();
            ApplicationUser user = UserManager.GetUserByToken(token);
            if (user == null) return BadRequest("Security access token not found in user database.");
            return Ok(new SimpleUser() { Id = user.Id, Username = user.UserName });
        }

        [HttpGet]
        [Route("api/GetNotifications")]
        public IHttpActionResult GetNotifications()
        {
            string token = Request.Headers.GetValues("Authorization").First();
            ApplicationUser user = UserManager.GetUserByToken(token);
            if (user == null) return BadRequest("Security access token not found in user database.");
            List<Notification> notifications = new List<Notification>();
            if (user != null)
            {
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

        [HttpGet]
        [Route("api/GetChartData")]
        public IHttpActionResult GetChartData()
        {
            string token = Request.Headers.GetValues("Authorization").First();
            ApplicationUser user = UserManager.GetUserByToken(token);
            if (user == null) return BadRequest("Security access token not found in user database.");
            return Ok(user.Dashboard);
        }

        [HttpGet]
        [Route("api/GetHomeCharts")]
        public IHttpActionResult GetHomeCharts()
        {
            List<SimpleChart> charts = new List<SimpleChart>();
            foreach(KeyValuePair<string, Chart> entry in ChartManager.GetStandardChart()) {
                SimpleChart sc = new SimpleChart
                {
                    Name = entry.Key,
                    Type = entry.Value.ChartType,
                    Data = entry.Value.ChartItemData
                };
                charts.Add(sc);
            }
            return Ok(charts);
        }

        [HttpGet]
        [Route("api/GetDashboardCharts")]
        public IHttpActionResult GetDashboardCharts()
        {
            string token = Request.Headers.GetValues("Authorization").First();
            ApplicationUser user = UserManager.GetUserByToken(token);
            if (user == null) return BadRequest("Security access token not found in user database.");
            List<Chart> userCharts = new List<Chart>();
            foreach(Chart c in user.Dashboard) {
              ChartManager.RetrieveDataChart(c);
              userCharts.Add(c);
            }
            List<SimpleChart> charts = new List<SimpleChart>();
            foreach(Chart c in userCharts) {
                SimpleChart sc = new SimpleChart
                {
                    Name = "Grafiek",
                    Type = c.ChartType,
                    Data = c.ChartItemData
                };
                charts.Add(sc);
            }
            return Ok(charts);
        }
    }

    public class TokenRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class SimpleUser
    {
        public string Id { get; set; }
        public string Username { get; set; }
    }

    public class SimpleChart
    {
        public string Name { get; set; }
        public ChartType Type { get; set; }
        public ICollection<ChartItemData> Data { get; set; }
    }
}