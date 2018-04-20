using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using BL.Domain;
using DAL.EF;

namespace BL
{
  public class AppUserManager : UserManager<ApplicationUser>, IAppUserManager
  {
    private UserRepository repo = null;

    public AppUserManager() : base(new UserStore<ApplicationUser>(new BarometerDbContext()))
    {

      repo = new UserRepository();
    }

    public List<ApplicationUser> GetUsers()
    {
      return repo.ReadUsers();
    }

    public static AppUserManager Create(IdentityFactoryOptions<AppUserManager> options, IOwinContext context)
    {
      var manager = new AppUserManager();
      // validation logic for usernames
      manager.UserValidator = new UserValidator<ApplicationUser>(manager)
      {
        AllowOnlyAlphanumericUserNames = false,
        RequireUniqueEmail = true
      };

      // validation logic for passwords
      manager.PasswordValidator = new PasswordValidator
      {
       
        RequiredLength = 5,
        //RequireNonLetterOrDigit = true,
        //RequireDigit = true,
        //RequireLowercase = true,
        //RequireUppercase = true,
      };

      // Configure user lockout defaults
      manager.UserLockoutEnabledByDefault = true;
      manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
      manager.MaxFailedAccessAttemptsBeforeLockout = 5;

      // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
      // You can write your own provider and plug it in here.
      //manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<User>
      //{
      //	MessageFormat = "Your security code is {0}"
      //});
      //manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<User>
      //{
      //	Subject = "Security Code",
      //	BodyFormat = "Your security code is {0}"
      //});
      //manager.EmailService = new EmailService();
      //manager.SmsService = new SmsService();
      var dataProtectionProvider = options.DataProtectionProvider;
      if (dataProtectionProvider != null)
      {
        manager.UserTokenProvider =
            new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
      }
      return manager;
    }

    public ApplicationUser GetUser(string id)
    {
      return repo.ReadUser(id);
    }

    public List<ApplicationUser> GetUsers()
    {
      return repo.ReadUsers();
    }

    public List<ApplicationUser> GetUsersFromRole(string roleName)
    {
      return repo.ReadUsersFromRole(this.GetRoleId(roleName));
    }

    public List<ApplicationUser> GetUsersWithoutRole(string roleName)
    {
      return repo.ReadUsersWithoutRole(this.GetRoleId(roleName));
    }

    public List<IdentityRole> GetRoles()
    {
      return repo.ReadRoles();
    }

    public List<IdentityRole> GetSpecificRole(string roleName)
    {
      return repo.ReadSpecificRole(roleName);
    }

    public List<IdentityRole> GetRolesWithout(string partRoleName)
    {
      return repo.ReadRolesWithout(partRoleName);
    }

    private string GetRoleId(string roleName)
    {
      return repo.ReadRoleId(roleName);
    }
  }
}
