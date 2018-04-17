using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Domain;
using DAL.EF;
using DAL;

namespace DAL.EF
{
    internal class BarometerDbInitializer : DropCreateDatabaseIfModelChanges<BarometerDbContext>
    {
        public static List<ApplicationUser> users;
        public static List<SocialMediaProfile> SocialMediaProfiles;
        public static List<Item> items;
        public static List<Alert> alerts;
        public static List<Person> persons;
        public static List<Organization> organizations;

        protected override void Seed(BarometerDbContext context)
        {
            GenerateData();
            foreach (SocialMediaProfile profile in SocialMediaProfiles)
            {
                context.SocialMediaProfiles.Add(profile);
            }

            //foreach (ApplicationUser user in Memory.users)
            //{
            //    context.Users.Add(user);
            //}

            foreach (Person person in persons)
            {
                context.Persons.Add(person);
            }

            foreach (Organization organization in organizations)
            {
                context.Organizations.Add(organization);
            }

            foreach (Item item in items)
            {
                context.Items.Add(item);
            }

            foreach (Alert alert in alerts)
            {
                context.Alerts.Add(alert);
            }



            context.SaveChanges();
        }
        public static void GenerateData()
        {
            ApplicationUser user1 = new ApplicationUser() { Id = "1", Admin = false, UserName = "user1", Mail = "user1.test@hotmail.com" };
            ApplicationUser user2 = new ApplicationUser() { Id = "2", Admin = false, UserName = "user2", Mail = "user2.test@hotmail.com" };

            SocialMediaProfile socialMediaProfile1 = new SocialMediaProfile() { ProfileId = 1, Url = "http://www.twitter.be/AnnouriImade", Source = "twitter" };
            SocialMediaProfile socialMediaProfile2 = new SocialMediaProfile() { ProfileId = 2, Url = "http://www.twitter.be/BastiaensCaroline", Source = "twitter" };
            SocialMediaProfile socialMediaProfile3 = new SocialMediaProfile() { ProfileId = 3, Url = "http://www.twitter.be/BertelsJan", Source = "twitter" };
            SocialMediaProfile socialMediaProfile4 = new SocialMediaProfile() { ProfileId = 4, Url = "http://www.twitter.be/DeRidderAnnick", Source = "twitter" };

            Organization organization1 = new Organization() { ItemId = 1, Name = "NVA" };

            Person person1 = new Person() { ItemId = 2, Name = "Annouri", FirstName = "Imade", Organization = organization1 };
            person1.socialMediaProfiles.Add(socialMediaProfile1);
            socialMediaProfile1.Item = person1;
            Person person2 = new Person() { ItemId = 3, Name = "Bastiaens", FirstName = "Caroline", Organization = organization1 };
            person2.socialMediaProfiles.Add(socialMediaProfile2);
            socialMediaProfile2.Item = person2;
            Person person3 = new Person() { ItemId = 4, Name = "Bertels", FirstName = "Jan", Organization = organization1 };
            person3.socialMediaProfiles.Add(socialMediaProfile3);
            socialMediaProfile3.Item = person3;
            Person person4 = new Person() { ItemId = 5, Name = "De Ridder", FirstName = "Annick", Organization = organization1 };
            person4.socialMediaProfiles.Add(socialMediaProfile4);
            socialMediaProfile4.Item = person4;

            organization1.persons.Add(person1);
            organization1.persons.Add(person2);
            organization1.persons.Add(person3);
            organization1.persons.Add(person4);

            Alert alert1 = new Alert(1, AlertType.notification, AlertParameter.tweets, ">", user1, person1);
            Alert alert2 = new Alert(2, AlertType.notification, AlertParameter.tweets, ">", user1, person2);
            Alert alert3 = new Alert(3, AlertType.notification, AlertParameter.tweets, ">", user2, person1);
            Alert alert4 = new Alert(4, AlertType.notification, AlertParameter.tweets, ">", user2, person4);
            person1.Alerts.Add(alert1);
            person1.Alerts.Add(alert3);
            person2.Alerts.Add(alert2);
            person4.Alerts.Add(alert4);
            user1.Alerts.Add(alert1);
            user1.Alerts.Add(alert2);
            user2.Alerts.Add(alert3);
            user2.Alerts.Add(alert4);

            users = new List<ApplicationUser>
            {
                user1,
                user2
            };

            SocialMediaProfiles = new List<SocialMediaProfile>
            {
                socialMediaProfile1,
                socialMediaProfile2,
                socialMediaProfile3,
                socialMediaProfile4
            };

            items = new List<Item>
            {
                organization1,
                person1,
                person2,
                person3,
                person4
            };

            organizations = new List<Organization>
            {
                organization1
            };

            persons = new List<Person>
            {
                person1,
                person2,
                person3,
                person4
            };

            alerts = new List<Alert>
            {
                alert1,
                alert2,
                alert3,
                alert4
            };

        }
    }
}
