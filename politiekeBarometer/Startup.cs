using System;
using Microsoft.Owin;
using Owin;
using BL.Managers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.IdentityModel.Tokens;
using BL.Domain;
using DAL.Repositories;
using BL;

[assembly: OwinStartupAttribute(typeof(politiekeBarometer.Startup))]
namespace politiekeBarometer
{
    public partial class Startup
    {
        private UnitOfWorkManager unitOfWorkManager;
        public Startup()
        {
            unitOfWorkManager = new UnitOfWorkManager();
        }
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesAndUsers();
            ActivateApi();
        }


        private void ActivateApi()
        {
            var SocialMediaManager = new SocialMediaManager(unitOfWorkManager);
            //TODO int moet configureerbaar zijn
            //TODO terug aanzetten
            SocialMediaManager.ActivateAPI();
        }

        private void CreateRolesAndUsers()
        {
            var roleManager = new AppRoleManager(unitOfWorkManager);
            var UserManager = new ApplicationUserManager(unitOfWorkManager);
            // create superadmin role
            if (!roleManager.RoleExists("SuperAdmin"))
            {

                var role = new IdentityRole
                {
                    Name = "SuperAdmin"
                };
                roleManager.Create(role);

                // make a superadmin to rule them all                   

                ApplicationUser user = new ApplicationUser
                {
                    UserName = "SuperAdmin",
                    Email = "t13barosuper@gmail.com"
                };

                string userPWD = "superadmin";

                var chkUser = UserManager.Create(user, userPWD);

                //Add to Role SuperAdmin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "SuperAdmin");
                    EmailSender.Instance.SendEmail("Gebruiker", user.Email, userPWD);
                }


            }

            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole
                {
                    Name = "Admin"
                };
                roleManager.Create(role);

                ApplicationUser user2 = new ApplicationUser() { UserName = "TestAdmin", Email = "yagodecuyper@gmail.com" };

                var chkuser2 = UserManager.Create(user2, "testadmin");
                if (chkuser2.Succeeded)
                {
                    UserManager.AddToRole(user2.Id, "Admin");
                }
            }

            if (!roleManager.RoleExists("Standard"))
            {
                var role = new IdentityRole
                {
                    Name = "Standard"
                };
                roleManager.Create(role);
            }
        }
    }
}