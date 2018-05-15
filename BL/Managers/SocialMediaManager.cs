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
//TODO query's in repository?
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
                date = "8 May 2018 08:49:12";
            }


            List<SocialMediaPost> data2 = (List<SocialMediaPost>)read.ReadData(date);
            foreach (var item in data2)
            {
                ArraysToLists(item);
                socialMediaRepository.CreateSocialMediaPost(item);
                socialMediaRepository.addPostToItems(item);
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

        public Dictionary<string, int> GetDataFromPost(DateTime since, ChartValue value, Item item = null)
        {
            List<SocialMediaPost> posts = (List<SocialMediaPost>)socialMediaRepository.ReadSocialMediaPostsSince(since, item);
            List<SocialMediaPost> results = new List<SocialMediaPost>();
            foreach (var post in posts)
            {
                post.ListsToArrays();
                if (item != null)
                {
                    if (post.Persons.Contains(item))
                    {
                        results.Add(post);
                    }
                }
            }
            if (results.Count > 0)
            {
                posts = results;
            }
            if (value == ChartValue.hashtags)
            {
                return GetHashtagData(posts);
            }
            else if (value == ChartValue.persons)
            {
                return GetPersonData(posts);
            }
            else if (value == ChartValue.words)
            {
                return GetWordData(posts);
            }
            return null;
        }

        //TODO uitbreiden voor organisaties en themas
        public Dictionary<Item, int> GetItemsFromChartWithoutItems(DateTime since, ChartValue value)
        {
            List<SocialMediaPost> posts = (List<SocialMediaPost>)socialMediaRepository.ReadSocialMediaPostsSince(since);
            if (value == ChartValue.trendPersons)
            {
                return GetTrendPersonData(posts);
            }
            else if (value == ChartValue.trendOrganizations)
            {

            }
            else if (value == ChartValue.trendThemes)
            {

            }
            return null;
        }

        public Dictionary<Item, int> GetTrendPersonData(List<SocialMediaPost> posts)
        {
            Dictionary<Item, int> list = new Dictionary<Item, int>();
            foreach (var post in posts)
            {
                post.ListsToArrays();
                foreach (var person in post.Persons)
                {
                    if (list.ContainsKey(person))
                    {
                        list[person]++;
                    }
                    else
                    {
                        list.Add(person, 1);
                    }
                }
            }
            return list.OrderByDescending(i => i.Value).Take(AMOUNT_OF_ELEMENTS).ToDictionary(pair => pair.Key, pair => pair.Value).Shuffle();
        }

        public Dictionary<string, int> GetHashtagData(List<SocialMediaPost> posts)
        {
            Dictionary<string, int> list = new Dictionary<string, int>();
            foreach (var post in posts)
            {
                post.ListsToArrays();
                foreach (var hashtag in post.Hashtag)
                {
                    if (list.ContainsKey(hashtag))
                    {
                        list[hashtag]++;
                    }
                    else
                    {
                        list.Add(hashtag, 1);
                    }
                }
            }
            return list.OrderByDescending(w => w.Value).Take(AMOUNT_OF_ELEMENTS).ToDictionary(pair => pair.Key, pair => pair.Value).Shuffle();
        }

        public Dictionary<string, int> GetPersonData(List<SocialMediaPost> posts)
        {
            Dictionary<string, int> list = new Dictionary<string, int>();
            foreach (var post in posts)
            {
                foreach (var person in post.Persons)
                {
                    if (list.ContainsKey(person.Name))
                    {
                        list[person.Name]++;
                    }
                    else
                    {
                        list.Add(person.Name, 1);
                    }
                }
            }
            return list.OrderByDescending(w => w.Value).Take(AMOUNT_OF_ELEMENTS).ToDictionary(pair => pair.Key, pair => pair.Value).Shuffle();
        }

        public Dictionary<string, int> GetWordData(List<SocialMediaPost> posts)
        {
            Dictionary<string, int> list = new Dictionary<string, int>();
            foreach (var post in posts)
            {
                foreach (var word in post.Words)
                {
                    if (list.ContainsKey(word.Value))
                    {
                        list[word.Value]++;
                    }
                    else
                    {
                        list.Add(word.Value, 1);
                    }
                }
            }
            return list.OrderByDescending(w => w.Value).Take(AMOUNT_OF_ELEMENTS).ToDictionary(pair => pair.Key, pair => pair.Value).Shuffle();
        }

        public List<string> getTopTenUrl(Item item)
        {
            List<string> allUrls = new List<string>();
            List<string> topTen = new List<string>();
            foreach (var post in item.SocialMediaPosts)
            {
                foreach (var url in post.Urls)
                {
                    allUrls.Add(url.Value);
                }
            }
            var sortedUrlsGroup = allUrls.GroupBy(s => s).OrderByDescending(g => g.Count());
            if((sortedUrlsGroup.ToList()).Count() < 10)
            {
                foreach(var url in (sortedUrlsGroup.ToList()).GetRange(0, (sortedUrlsGroup.ToList()).Count()))
                {
                    topTen.Add(url.Key);
                }
            }
            else
            {
                foreach (var url in (sortedUrlsGroup.ToList()).GetRange(0, 10))
                {
                    topTen.Add(url.Key);
                }
            }
            return topTen;
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

