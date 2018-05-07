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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace politiekeBarometer.Controllers {
  [Authorize]
  [Route("api")]
  public class AppController : ApiController {
    private const string SECURITY_KEY = "q4rgq4rj8rsq4jqds64hsgd64jd";

    private SocialMediaManager SocialMediaManager;
    private ApplicationUserManager UserManager;
    private AlertManager AlertManager;
    private ChartManager ChartManager;

    public static string SECURITY_KEY1 => SECURITY_KEY;

    protected AppController() {
      SocialMediaManager = new SocialMediaManager();
      AlertManager = new AlertManager();
      UserManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
      UserManager.SetSocialMediaManager(SocialMediaManager);
    }

    [HttpGet]
    public IHttpActionResult Test() {
      var userName = User.Identity.Name;
      return Ok($"Super secret content, I hope you've got clearance for this {userName}...");
    }

    [AllowAnonymous]
    [HttpPost]
    public IHttpActionResult RequestToken([FromBody] TokenRequest request) {
      if (request.Username == "Jon" && request.Password == "Again, not for production use, DEMO ONLY!") {
        var claims = new[]
        {
                    new Claim(ClaimTypes.Name, request.Username),
                    new Claim("CompletedBasicTraining", "")
                };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECURITY_KEY1));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "yourdomain.com",
            audience: "yourdomain.com",
            claims: claims,
            signingCredentials: creds);

        return Ok(new {
          token = new JwtSecurityTokenHandler().WriteToken(token)
        });
      }

      return BadRequest("Could not verify username and password");
    }

  }

  public class TokenRequest {
    public string Username { get; set; }
    public string Password { get; set; }
  }
}