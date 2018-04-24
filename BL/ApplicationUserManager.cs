using BL.Domain;
using DAL;
using DAL.EF;
using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;


namespace BL
{
    public class ApplicationUserManager : UserManager<ApplicationUser>, IAppUserManager
    {
        private UserRepository userRepository;
        private AlertRepository alertRepository;
        private SocialMediaManager socialMediaManager;

        //public ApplicationUserManager(SocialMediaManager socialMediaManager)
        //{
        //    alertRepository = new AlertRepository();
        //    userRepository = new UserRepository();
        //    this.socialMediaManager = socialMediaManager;
        //}

        public ApplicationUserManager() : base(new UserStoreRepository())
        {
            this.alertRepository = new AlertRepository();
            userRepository = new UserRepository();
        }

        public void setSocialMediaManager(SocialMediaManager socialMediaManager)
        {
            this.socialMediaManager = socialMediaManager;
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {

            var manager = new ApplicationUserManager();

            //USERNAME VALIDATION
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };


            //PASSWORD VALIDATION
            manager.PasswordValidator = new PasswordValidator
            {
                RequireDigit = false,
                RequiredLength = 6,
                RequireLowercase = false,
                RequireUppercase = false,
                RequireNonLetterOrDigit = false
            };


            //CONFIGURE LOCKOUT
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(15);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

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
            return userRepository.ReadUser(id);
        }


    public List<ApplicationUser> GetUsers()
    {
      return userRepository.ReadUsers();
    }

    public List<ApplicationUser> GetUsersFromRole(string roleName)
    {
      return userRepository.ReadUsersFromRole(this.GetRoleId(roleName));
    }

    public List<ApplicationUser> GetUsersWithoutRole(string roleName)
    {
      return userRepository.ReadUsersWithoutRole(this.GetRoleId(roleName));
    }

    public List<IdentityRole> GetRoles()
    {
      return userRepository.ReadRoles();
    }

    public List<IdentityRole> GetSpecificRole(string roleName)
    {
      return userRepository.ReadSpecificRole(roleName);
    }

    public List<IdentityRole> GetRolesWithout(string partRoleName)
    {
      return userRepository.ReadRolesWithout(partRoleName);
    }

    private string GetRoleId(string roleName)
    {
      return userRepository.ReadRoleId(roleName);
    }

    public void SendMail(Alert alert)
        {
          Mail.sendMail(alert.User.Email, "Nieuwe melding", alert.Content);
        }
    }
}
