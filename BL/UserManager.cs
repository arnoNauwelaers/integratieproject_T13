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
    public class ApplicationUserManager : UserManager<ApplicationUser>
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

        public ApplicationUserManager(IUserStore<ApplicationUser> store) : base(store)
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

            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<BarometerDbContext>()));

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

            return manager;

        }

        public List<Alert> GetAlerts(Item item)
        {
           return alertRepository.GetAlerts(item);
        }

        public ApplicationUser GetUser(string id)
        {
            return userRepository.GetUser(id);
        }

        public void InspectAlert(Alert alert)
        {
            bool condionAns = socialMediaManager.VerifyCondition(alert);
            int notificationNmr = 1;
            if (condionAns == true)
            {
                AlertType type = alert.Type;
                if (type != AlertType.mail)
                {
                    //TODO controleer of notification al in database is opgenomen
                    if (!alertRepository.NotificationExists(alert.AlertId)) { 
                        Notification notification = new Notification() { NotificationId = notificationNmr, DateTime = DateTime.Now, Alert = alert };
                        alertRepository.CreateNotification(notification);
                        alert.Notifications.Add(notification);
                        //TODO klopt niet, moet een user wel als attribuut in Alert opgenomen worden?
                        alertRepository.UpdateAlert(alert);
                        notificationNmr++;
                    }
                }
                if (type != AlertType.notification)
                {
                    this.SendMail(alert);
                }
            }
        }

        public void UpdateNotification(Notification notification)
        {
            alertRepository.UpdateNotification(notification);
        }

        private void SendMail(Alert alert)
        {
            //TODO: implementatie send mail
        }
    }
}
