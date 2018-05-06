using System.Web;
using System.Web.Mvc;
 
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Microsoft.Owin.Security.Google;
using BL;
using DAL.EF;
using Microsoft.AspNet.Identity.Owin;
using BL.Domain;
using System;
using BL.Managers;

namespace politiekeBarometer
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext(BarometerDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
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
                ClientId = "493495399898-e194hr67o9dmin0bhs4gpcl98b99rihd.apps.googleusercontent.com",
                ClientSecret = "i6TEuiVPv1rcJoljZG8a17ut"
            });

            { 
                }
            }
        }
    }
