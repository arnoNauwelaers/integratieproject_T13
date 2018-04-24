using BL.Domain;
using DAL;
using DAL.EF;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class SocialMediaManager
    {
        private const int FREQUENTIE = 1;

        private SocialMediaRepository socialMediaRepository;
        private AlertManager alertManager;
        private ItemManager itemManager;
        private Read read;

        public SocialMediaManager()
        {
            socialMediaRepository = RepositoryFactory.CreateSocialMediaRepository();
            itemManager = new ItemManager();
            alertManager = new AlertManager();
            read = new Read();
        }

        public void SynchronizeDatabase()
        {
            List<Item> alteredItems = CreatePosts();
            List<Alert> alerts = new List<Alert>();
            foreach (var item in alteredItems)
            {
                alerts = alertManager.GetAlerts(item);
                foreach (var alert in alerts)
                {
                    alertManager.InspectAlert(alert);
                }
            }

        }

        public List<Item> CreatePosts()
        {
            List<SocialMediaPost> data = (List<SocialMediaPost>)read.ReadData();
            SocialMediaPost tempPost = socialMediaRepository.GetLastQueryDate();
            string date;
            if (tempPost != null)
            {
                date = tempPost.Date.ToString();
            }
            else
            {
                date = "23 Apr 2018 08:49:12";
            }
            date = "24 Apr 2018 08:49:12";
            List<SocialMediaPost> data2 = (List<SocialMediaPost>)read.ReadData2(date);
            foreach (var item in data)
            {
                socialMediaRepository.CreateSocialMediaPost(item);
            }
            return itemManager.GetAllItemsFromPosts(data);
        }

        public Boolean VerifyCondition(Alert alert)
        {
            int tweetAmount = socialMediaRepository.ReadItemParameter(alert, DateTime.Now, DateTime.Now.AddHours(-FREQUENTIE));
            if (alert.CompareItem == null)
            {
                int oldTweetAmount = socialMediaRepository.ReadItemParameter(alert, DateTime.Now.AddHours(-1), DateTime.Now.AddHours(-(FREQUENTIE * 2)));

                if (alert.Condition == ">")
                {
                    //als een politicus 2 maal zoveel tweets stuurt in het laatste uur als in het vorige uur wordt er een notification gestuurd
                    return tweetAmount >= (oldTweetAmount * 2);
                }
                return false;
            }
            else
            {
                int tweetAmount2 = socialMediaRepository.ReadItemParameter(alert, DateTime.Now, DateTime.Now.AddHours(-FREQUENTIE));
                if (alert.Condition == ">")
                {
                    //als er over een politus meer dan 2 maal zveel getweet is in het afgelopen uur als een ander politici word er een notification gestuurd
                    return tweetAmount >= tweetAmount2 * 2;
                }

            }
            return false;
        }
    }
}
