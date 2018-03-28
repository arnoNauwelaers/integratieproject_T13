using BL.Domain;
using DAL;
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
        private SocialMediaRepository SocialMediaRepository;
        private ItemManager ItemManager;
        private Read read;

        public SocialMediaManager()
        {
            SocialMediaRepository = new SocialMediaRepository();
            ItemManager = new ItemManager();
            read = new Read(); 
        }

        public List<Item> CreatePosts()
        {
            List<SocialMediaPost> data = (List<SocialMediaPost>) read.ReadData();
            foreach (var item in data)
            {
                SocialMediaRepository.Add(item);
            }
            return ItemManager.GetAllItemsFromPosts(data);
        }

        //TODO VerifyCondition
        public Boolean VerifyCondition(Alert alert)
        {
            if (alert.Parameter == AlertParameter.tweets)
            {
                int tweetAmount = SocialMediaRepository.ReadItemParameter(alert.Item, DateTime.Now, DateTime.Now.AddHours(-1));
                int oldTweetAmount = SocialMediaRepository.ReadItemParameter(alert.Item, DateTime.Now.AddHours(-1), DateTime.Now.AddHours(-3));
                if (alert.Condition == '>')
                {
                    
                }
                else if (alert.Condition == '<')
                {

                }
            }
        }
    }
}
