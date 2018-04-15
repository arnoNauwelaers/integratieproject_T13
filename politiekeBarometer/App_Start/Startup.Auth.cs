using System.Web;
using System.Web.Mvc;
 
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Microsoft.Owin.Security.Google;

namespace politiekeBarometer
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            // clientId: "",
            // clientSecret: "");

            //app.UseTwitterAuthentication(
            // consumerKey: "",
            // consumerSecret: "");

            //app.UseFacebookAuthentication(
            // appId: "",
            // appSecret: "");

            app.UseFacebookAuthentication(
               appId: "228996747837020",
               appSecret: "735b24040c0bbef71590809c7abfbd1c");

            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "1006596235238-ckdb3qfiq73832o41kpd161e1f0enp5h.apps.googleusercontent.com",
                ClientSecret = "BwYFD_y1T-WtG7RVjD8X71-Y"
            });
        }
    }
}