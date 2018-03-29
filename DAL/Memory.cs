using BL.Domain;
using System.Collections.Generic;
namespace DAL
{
    public class Memory
    {
        public List<User> users;
        public List<SocialMediaProfile> SocialMediaProfiles;
        public List<Item> items;
        public List<Alert> alerts;

        public Memory()
        {
            this.GenerateDummyData();
        }

        private void GenerateDummyData()
        {
            User user1 = new User() { UserId = 1, Admin = false, UserName = "user1", Password = "poc", Mail = "user1.test@hotmail.com"};
            User user2 = new User() { UserId = 1, Admin = false, UserName = "user2", Password = "poc", Mail = "user2.test@hotmail.com"};

            SocialMediaProfile socialMediaProfile1 = new SocialMediaProfile() { ProfileId = 1, Url = "http://www.twitter.be/AnnouriImade" , Source ="twitter" };
            SocialMediaProfile socialMediaProfile2 = new SocialMediaProfile() { ProfileId = 2, Url = "http://www.twitter.be/BastiaensCaroline", Source = "twitter" };
            SocialMediaProfile socialMediaProfile3 = new SocialMediaProfile() { ProfileId = 3, Url = "http://www.twitter.be/BertelsJan", Source = "twitter" };
            SocialMediaProfile socialMediaProfile4 = new SocialMediaProfile() { ProfileId = 4, Url = "http://www.twitter.be/DeRidderAnnick", Source = "twitter" };

            Organization organization1 = new Organization() { ItemId = 1, Name = "NVA" };

            Person person1 = new Person() { ItemId = 2, Name = "Annouri", FirstName = "Imade", Organization = organization1};
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

            Alert alert1 = new Alert() { AlertId = 1, Type = AlertType.notification, Parameter = AlertParameter.tweets, Condition = '>', User = user1, Item = person1 };
            Alert alert2 = new Alert() { AlertId = 2, Type = AlertType.notification, Parameter = AlertParameter.tweets, Condition = '>', User = user1, Item = person2 };
            Alert alert3 = new Alert() { AlertId = 3, Type = AlertType.notification, Parameter = AlertParameter.tweets, Condition = '>', User = user2, Item = person1 };
            Alert alert4 = new Alert() { AlertId = 4, Type = AlertType.notification, Parameter = AlertParameter.tweets, Condition = '>', User = user2, Item = person4 };
            person1.Alerts.Add(alert1);
            person1.Alerts.Add(alert3);
            person2.Alerts.Add(alert2);
            person4.Alerts.Add(alert4);
            user1.Alerts.Add(alert1);
            user1.Alerts.Add(alert2);
            user2.Alerts.Add(alert3);
            user2.Alerts.Add(alert4);

            users = new List<User>();
            users.Add(user1);
            users.Add(user2);

            SocialMediaProfiles = new List<SocialMediaProfile>();
            SocialMediaProfiles.Add(socialMediaProfile1);
            SocialMediaProfiles.Add(socialMediaProfile2);
            SocialMediaProfiles.Add(socialMediaProfile3);
            SocialMediaProfiles.Add(socialMediaProfile4);

            items = new List<Item>();
            items.Add(organization1);
            items.Add(person1);
            items.Add(person2);
            items.Add(person3);
            items.Add(person4);

            alerts = new List<Alert>();
            alerts.Add(alert1);
            alerts.Add(alert2);
            alerts.Add(alert3);
            alerts.Add(alert4);
        }

        
    }
}