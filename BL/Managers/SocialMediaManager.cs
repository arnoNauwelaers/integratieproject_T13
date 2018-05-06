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
                LinkPersons(item);
                socialMediaRepository.CreateSocialMediaPost(item);
            }
            return itemManager.GetAllItemsFromPosts(data2);
        }

        private void LinkPersons(SocialMediaPost post)
    {
      //foreach(var v in post.Person)
      //{
      //  Person p = (Person)itemManager.GetItem(v);
      //  if (!p.Equals(null))
      //  {
      //    post.Persons.Add(p);
          
      //  }
        
      //}
      socialMediaRepository.UpdateSocialMediaPost(post);
    }
        // dubbel? staat ook in alertmanager
        //public Boolean VerifyCondition(Alert alert)
        //{
        //    int tweetAmount = socialMediaRepository.ReadItemParameter(alert, DateTime.Now, DateTime.Now.AddHours(-FREQUENTIE));
        //    if (alert.CompareItem == null)
        //    {
        //        int oldTweetAmount = socialMediaRepository.ReadItemParameter(alert, DateTime.Now.AddHours(-1), DateTime.Now.AddHours(-(FREQUENTIE * 2)));

        //        if (alert.Condition == ">")
        //        {
        //            //als een politicus 2 maal zoveel tweets stuurt in het laatste uur als in het vorige uur wordt er een notification gestuurd
        //            return tweetAmount >= (oldTweetAmount * 2);
        //        }
        //        return false;
        //    }
        //    else
        //    {
        //        int tweetAmount2 = socialMediaRepository.ReadItemParameter(alert, DateTime.Now, DateTime.Now.AddHours(-FREQUENTIE));
        //        if (alert.Condition == ">")
        //        {
        //            //als er over een politus meer dan 2 maal zveel getweet is in het afgelopen uur als een ander politici word er een notification gestuurd
        //            return tweetAmount >= tweetAmount2 * 2;
        //        }

        //    }
        //    return false;
        //}

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
                return tempList;
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
                return tempList;
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
                return tempList;
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
            foreach (var item in post.Sentiment)
            {
                post.Sentiments.Add(new Sentiment(item));
            }
            foreach (var item in post.Hashtag)
            {
                post.Hashtags.Add(new Hashtag(item));
            }
            foreach (var item in post.Verhaal)
            {
                post.Urls.Add(new Url(item));
            }
            foreach (var item in post.Word)
            {
                post.Words.Add(new Word(item));
            }
            foreach (var item in post.Person)
            {
                post.Persons.Add(itemManager.CreatePersonIfNotExists(item));
            }
            foreach (var item in post.Theme)
            {
                post.Themes.Add(new Theme(item));
            }
        }
    }
}

