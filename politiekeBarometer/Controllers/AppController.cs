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
using System.Security.Claims;
using System.Text;
using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using Newtonsoft.Json;

namespace politiekeBarometer.Controllers
{
    //JWT BEARER NOG INSTELLEN
    /*[Authorize]
      public class AppController : ApiController {
      private const string SECURITY_KEY = "q4rgq4rj8rsq4jqds64hsgd64jd";

      private SocialMediaManager SocialMediaManager;
      private ApplicationUserManager UserManager;
      private AlertManager AlertManager;
      private ChartManager ChartManager;

      protected AppController() {
        SocialMediaManager = new SocialMediaManager();
        AlertManager = new AlertManager();
        UserManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
        UserManager.SetSocialMediaManager(SocialMediaManager);
      }

      [HttpGet]
      [Route("api/TestApi")]
      [AllowAnonymous] //!TIJDELIJK!
      public IHttpActionResult Test() {
        var userName = User.Identity.Name;
        return Ok("Jow");
      }

      [HttpGet]
      [Route("api/GetNotifications")]
      [AllowAnonymous] //!TIJDELIJK!
      public IHttpActionResult GetNotifications() {
        List<Notification> notifications = new List<Notification>();
        SocialMediaManager.SynchronizeDatabase();
        //if (User.Identity.GetUserId() != null) {
        //ApplicationUser user = UserManager.GetUser(User.Identity.GetUserId());
          foreach(var user in UserManager.GetUsers()) {
            if (user.Alerts.Count > 0) {
              foreach (var alert in user.Alerts) {
                foreach (var notification in alert.Notifications) {
                  notifications.Add(notification);
                }
              }
            }
           }
        //}
        return Ok(notifications);
      }

      [AllowAnonymous]
      [HttpPost]
      [Route("api/RequestToken")]
      //TODO: instellen in startup.cs, blijkbaar
      //TODO: id van user toevoegen aan claim
      public IHttpActionResult RequestToken([FromBody] TokenRequest request) {
        if (request.Username == "Jon" && request.Password == "Test") {
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

          return Ok(new {
            token = new JwtSecurityTokenHandler().WriteToken(token)
          });
        }

        return BadRequest("Could not verify username and password");
      }

      public IHttpActionResult GetChartDate() {
        //UserManager.GetUser()
        return Ok();
      }


    }

    public class TokenRequest {
      public string Username { get; set; }
      public string Password { get; set; }
    }*/

    public class AppController : ApiController
    {
        private const string SECURITY_KEY = "q4rgq4rj8rsq4jqds64hsgd64jd";

        private SocialMediaManager SocialMediaManager;
        private ApplicationUserManager UserManager;
        private AlertManager AlertManager;
        private ChartManager ChartManager;

        protected AppController()
        {
            SocialMediaManager = new SocialMediaManager();
            AlertManager = new AlertManager();
            UserManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            UserManager.SetSocialMediaManager(SocialMediaManager);
            ChartManager = new ChartManager();
        }

        [HttpGet]
        [Route("api/test")]
        public IHttpActionResult Test()
        {
            return Ok("API Call Succesfull (" + Request.Headers.GetValues("Authorization").First() + ")");
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
        [Route("api/GetHomeCharts")]
        public IHttpActionResult GetHomeCharts()
        {
            return Ok(ChartManager.GetStandardChart());
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
}