﻿using System;
using Microsoft.Owin;
using Owin;
using BL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using BL.Domain;

[assembly: OwinStartupAttribute(typeof(politiekeBarometer.Startup))]
namespace politiekeBarometer
{
  public partial class Startup
  {
    public void Configuration(IAppBuilder app)
    {
      ConfigureAuth(app);
    }

    private void CreateRolesAndUsers()
    {
      var roleManager = new AppRoleManager();
      var UserManager = new AppUserManager();
      // create superadmin role
      if (!roleManager.RoleExists("SuperAdmin"))
      {

        var role = new IdentityRole();
        role.Name = "SuperAdmin";
        roleManager.Create(role);

        // make a superadmin to rule them all                   

        ApplicationUser user = new ApplicationUser();
        user.UserName = "SuperAdmin";
        user.Email = "t13barosuper@gmail.com";

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
        var role = new IdentityRole();
        role.Name = "Admin";
        roleManager.Create(role);
      }

      if (!roleManager.RoleExists("Standard"))
      {
        var role = new IdentityRole();
        role.Name = "Standard";
        roleManager.Create(role);
      }
    }
  }
}