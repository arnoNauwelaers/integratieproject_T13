using BL.Domain;
using DAL;
using DAL.EF;
using DAL.Repositories;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BL.Managers
{
    public class SocialMediaManager
    {
        private const int FREQUENTIE = 1;
        private const int AMOUNT_OF_ELEMENTS = 20;
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
                alerts.AddRange(item.Alerts);
            }

          alertManager.HandleAlerts(alerts);

        }

        public List<Item> CreatePosts()
        {
            SocialMediaPost tempPost = socialMediaRepository.GetLastQueryDate();
            string date;
            if (tempPost != null)
            {
                date = tempPost.Date.Day + " " + GetMonthFromInt(tempPost.Date.Month) + " " + tempPost.Date.Year + " " + tempPost.Date.Hour + ":" + tempPost.Date.Minute + ":" + tempPost.Date.Second;
            }
            else
            {
                //TODO vanaf vorige maand?
                date = "28 Apr 2018 08:49:12";
            }

            List<SocialMediaPost> data2 = (List<SocialMediaPost>)read.ReadData(date);
            foreach (var item in data2)
            {
                ArraysToLists(item);
                socialMediaRepository.CreateSocialMediaPost(item);
            }
            return itemManager.GetAllItemsFromPosts(data2);
        }

        private string GetMonthFromInt(int month)
        {
            switch (month)
            {
                case 1: return "Jan";
                case 2: return "Feb";
                case 3: return "Mar";
                case 4: return "Apr";
                case 5: return "May";
                case 6: return "Jun";
                case 7: return "Jul";
                case 8: return "Aug";
                case 9: return "Sep";
                case 10: return "Oct";
                case 11: return "Nov";
                case 12: return "Dec";
                default: return "";
            }
        }

        public Dictionary<string, int> GetDataFromPost(DateTime since, ChartValue value, Item item)
        {
            Dictionary<string, int> tempList = new Dictionary<string, int>();
            List<SocialMediaPost> posts = (List<SocialMediaPost>)socialMediaRepository.ReadSocialMediaPostsSince(since, item);
            if (value == ChartValue.hashtags)
            {
                foreach (var post in posts)
                {
                    post.ListsToArrays();
                    foreach (var hashtag in post.Hashtag)
                    {
                        if (tempList.ContainsKey(hashtag))
                        {
                            tempList[hashtag]++;
                        }
                        else
                        {
                            tempList.Add(hashtag, 1);
                        }
                    }
                }
                return tempList.OrderByDescending(w => w.Value).Take(AMOUNT_OF_ELEMENTS).ToDictionary(pair => pair.Key, pair => pair.Value).Shuffle();
            }
            else if (value == ChartValue.persons)
            {
                foreach (var post in posts)
                {
                    foreach (var person in post.Person)
                    {
                        if (tempList.ContainsKey(person))
                        {
                            tempList[person]++;
                        }
                        else
                        {
                            tempList.Add(person, 1);
                        }
                    }
                }
                return tempList.OrderByDescending(w => w.Value).Take(AMOUNT_OF_ELEMENTS).ToDictionary(pair => pair.Key, pair => pair.Value).Shuffle();
            }
            else if (value == ChartValue.words)
            {
                foreach (var post in posts)
                {
                    foreach (var word in post.Words)
                    {
                        if (tempList.ContainsKey(word.Value))
                        {
                            tempList[word.Value]++;
                        }
                        else
                        {
                            tempList.Add(word.Value, 1);
                        }
                    }
                }
                return tempList.OrderByDescending(w => w.Value).Take(AMOUNT_OF_ELEMENTS).ToDictionary(pair => pair.Key, pair => pair.Value).Shuffle();
            }
            return null;
        }

        public List<SocialMediaProfile> GetSocialMediaProfiles()
        {
            return socialMediaRepository.ReadProfiles();
        }

        public SocialMediaProfile GetSocialMediaProfile(int id)
        {

            return socialMediaRepository.ReadProfile(id);

        }

        public void ActivateAPI(int minutes)
        {
            //TODO moet in panel configureerbaar zijn
            var timer = new System.Threading.Timer(
            e => SynchronizeDatabase(),
            null,
            TimeSpan.Zero,
            TimeSpan.FromMinutes(minutes));
        }

        public void ArraysToLists(SocialMediaPost post)
        {
            if (post.Sentiment.Count() > 0)
            {
                post.PostSentiment = new Sentiment(post.Sentiment[0], post.Sentiment[1]);
            }
            
            foreach (var item in post.Hashtag)
            {
                if (item != null)
                {
                    post.Hashtags.Add(new Hashtag(item));
                }
            }
            foreach (var item in post.Verhaal)
            {
                if (item != null)
                {
                    post.Urls.Add(new Url(item));
                }
            }
            foreach (var item in post.Word)
            {
                if (item != null)
                {
                    post.Words.Add(new Word(item));
                }
            }
            foreach (var item in post.Person)
            {
                if (item != null)
                {
                    post.Persons.Add(itemManager.CreatePersonIfNotExists(item));
                }
            }
            foreach (var item in post.Theme)
            {
                if (item != null)
                {
                    post.Themes.Add(new Theme(item));
                }
            }
        }
    }
}

