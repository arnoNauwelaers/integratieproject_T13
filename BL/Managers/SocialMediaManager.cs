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
        private SettingsManager settingsManager;
        private Read read;
        private static DateTime lastQuery;
        private static Dictionary<string, List<SocialMediaPost>> postsSaved = new Dictionary<string, List<SocialMediaPost>>();


        public SocialMediaManager(UnitOfWorkManager unitOfWorkManager)
        {
            socialMediaRepository = new SocialMediaRepository(unitOfWorkManager.UnitOfWork);
            itemManager = new ItemManager(unitOfWorkManager);
            alertManager = new AlertManager(unitOfWorkManager);
            settingsManager = new SettingsManager(unitOfWorkManager);
            read = new Read(unitOfWorkManager);
        }


        public void SynchronizeDatabase()
        {
            List<Item> alteredItems = CreatePosts();
            Read.LastRead = DateTime.Now;
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
                DateTime start = DateTime.Now.AddDays(-(settingsManager.GetSettings().DataLifetime));
                date = start.Day + " " + GetMonthFromInt(start.Month) + " " + start.Year + " " + start.Hour + ":" + start.Minute + ":" + start.Second;
            }

            List<SocialMediaPost> data2 = (List<SocialMediaPost>)read.ReadData(date);
            foreach (var item in data2)
            {
                ArraysToLists(item);
                socialMediaRepository.CreateSocialMediaPost(item);
                socialMediaRepository.AddPostToItems(item);
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
            List<SocialMediaPost> results = GetPostsFromItem(item, since);
            if (value == ChartValue.hashtags)
            {
                return GetHashtagData(results);
            }
            else if (value == ChartValue.persons)
            {
                return GetPersonData(results);
            }
            else if (value == ChartValue.words)
            {
                return GetWordData(results);
            }
            return null;
        }

        public List<SocialMediaPost> GetPostsFromItem(Item item, DateTime since)
        {
            List<SocialMediaPost> posts = ReadPostsSince(since).ToList();
            List<SocialMediaPost> results = new List<SocialMediaPost>();
            foreach (var post in posts)
            {
                if (item != null)
                {
                    if (item.GetType().ToString().Contains("Organization") && IsPostFromOrganization(post, (Organization)item))
                    {
                        results.Add(post);
                    }
                    else if (item.GetType().ToString().Contains("Theme") && IsPostFromTheme(post, (Theme)item))
                    {
                        results.Add(post);
                    }
                    else if (item.GetType().ToString().Contains("Person") && IsPostFromPerson(post, (Person)item))
                    {
                        results.Add(post);
                    }
                }
            }
            if (item == null)
            {
                results = posts;
            }
            return results;
        }

        public Boolean IsPostFromPerson(SocialMediaPost post, Person item)
        {
            if (post.Persons.Any(p => p.Name.Contains(item.Name)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<SocialMediaPost> ReadPostsSince(DateTime since)
        {
            if (postsSaved.Where(p => p.Key.Contains(since.ToShortDateString())).Any())
            {
                DateTime now = DateTime.Now;
                if (lastQuery != null)
                {
                    TimeSpan dateDifference = now.Subtract(lastQuery);
                    if (dateDifference.TotalMinutes <= 2)
                    {
                        return postsSaved.Where(p => p.Key.Contains(since.ToShortDateString())).First().Value;
                    }
                }
            }
            else
            {
                List<SocialMediaPost> postsTemp = (List<SocialMediaPost>)socialMediaRepository.ReadSocialMediaPostsSince(since);
                lastQuery = DateTime.Now;
                postsSaved.Add(since.ToShortDateString(), postsTemp);
                return postsTemp;
            }
            List<SocialMediaPost> posts = (List<SocialMediaPost>)socialMediaRepository.ReadSocialMediaPostsSince(since);
            lastQuery = DateTime.Now;
            postsSaved[since.ToShortDateString()] = posts;
            return posts;
        }

        public Dictionary<string, int> GetAmountPostsPerItem(DateTime since, Item item)
        {
            List<SocialMediaPost> posts = GetPostsFromItem(item, since).OrderByDescending(p => p.Date).ToList();
            Dictionary<string, int> result = new Dictionary<string, int>();
            //for loop en elke datum optellen per item
            foreach (var post in posts)
            {
                if (item.GetType().ToString().Contains("Person"))
                {
                    foreach (var person in post.Persons.ToList())
                    {
                        if (person.Name.Equals(item.Name))
                        {
                            string date = post.Date.ToShortDateString();
                            result = AddToResultsDictionary(result, date);
                        }
                    }
                }
                else if (item.GetType().ToString().Contains("Organization"))
                {
                    if (IsPostFromOrganization(post, (Organization)item))
                    {
                        string date = post.Date.ToShortDateString();
                        result = AddToResultsDictionary(result, date);
                    }
                }
                else if (item.GetType().ToString().Contains("Theme"))
                {
                    if (IsPostFromTheme(post, (Theme)item))
                    {
                        string date = post.Date.ToShortDateString();
                        result = AddToResultsDictionary(result, date);
                    }
                }
            }
            return result;
        }

        public Dictionary<string, int> AddToResultsDictionary(Dictionary<string, int> list, string date)
        {
            if (list.Any(p => p.Key == date))
            {
                list[date]++;
            }
            else
            {
                list.Add(date, 1);
            }
            return list;
        }

        public Boolean IsPostFromTheme(SocialMediaPost post, Theme item)
        {
            if (post.Themes.Contains(item))
            {
                return true;
            }
            else
            {
                foreach (var word in item.Keywords)
                {
                    if (post.Words.Any(w => w.Value == word.Value) || post.Hashtags.Any(w => w.Value == word.Value))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public Boolean IsPostFromOrganization(SocialMediaPost post, Organization item)
        {
            foreach (var person in item.Persons)
            {
                if (person.Organization != null && person.Organization == item)
                {
                    return true;
                }
            }
            return false;
        }

        public Dictionary<Item, int> GetItemsFromChartWithoutItems(DateTime since, ChartValue value, string itemType)
        {
            List<SocialMediaPost> posts = ReadPostsSince(since).ToList();
            if (value == ChartValue.trendPersons)
            {
                return GetTrendPersonData(posts);
            }
            else if (value == ChartValue.trendOrganizations)
            {
                return GetTrendOrganizationData(posts);
            }
            else if (value == ChartValue.trendThemes)
            {
                return GetTrendThemeData(posts);
            }
            else if (value == ChartValue.trendMostPositive)
            {
                return GetTrendSentimentData(posts, itemType, true);
            }
            else if (value == ChartValue.trendMostNegative)
            {
                return GetTrendSentimentData(posts, itemType, false);
            }
            return null;
        }

        public Dictionary<Item, int> GetTrendSentimentData(List<SocialMediaPost> posts, string itemType, Boolean positive = true)
        {
            Dictionary<Item, List<int>> list = new Dictionary<Item, List<int>>();
            Dictionary<Item, int> definitiveList = new Dictionary<Item, int>();
            foreach (var post in posts)
            {
                if (post.PostSentiment == null)
                {
                    continue;
                }
                if (itemType.Contains("Person"))
                {
                    foreach (var person in post.Persons)
                    {
                        if (!list.Any(p => p.Key == person))
                        {
                            list.Add(person, new List<int>());
                        }
                        list[person].Add((int)(post.PostSentiment.Polarity * 100));
                    }
                }
                else if (itemType.Contains("Organization"))
                {
                    foreach (var person in post.Persons)
                    {
                        if (person.Organization != null)
                        {
                            if (!list.Any(p => p.Key == person.Organization))
                            {
                                list.Add(person.Organization, new List<int>());
                            }
                            list[person.Organization].Add((int)(post.PostSentiment.Polarity * 100));
                        }
                    }
                }
                else if (itemType.Contains("Theme"))
                {
                    foreach (var theme in itemManager.GetThemes().ToList())
                    {
                        if (IsPostFromTheme(post, theme))
                        {
                            if (!list.Any(p => p.Key == theme))
                            {
                                list.Add(theme, new List<int>());
                            }
                            list[theme].Add((int)(post.PostSentiment.Polarity * 100));
                        }
                    }
                }
            }
            foreach (var item in list)
            {
                definitiveList.Add(item.Key, (int)item.Value.Average());
            }
            return definitiveList;
        }

        public Dictionary<Item, int> GetTrendThemeData(List<SocialMediaPost> posts)
        {
            Dictionary<Item, int> list = new Dictionary<Item, int>();
            foreach (var post in posts)
            {
                foreach (var theme in post.Themes)
                {
                    if (theme.Name != null)
                    {
                        if (list.ContainsKey(theme))
                        {
                            list[theme]++;
                        }
                        else
                        {
                            list.Add(theme, 1);
                        }
                    }
                }
                foreach (var theme in itemManager.GetThemes().ToList())
                {
                    foreach (var keyword in theme.Keywords)
                    {
                        foreach (var word in post.Words)
                        {
                            if (keyword.Value.Equals(word.Value))
                            {
                                if (list.ContainsKey(theme))
                                {
                                    list[theme]++;
                                }
                                else
                                {
                                    list.Add(theme, 1);
                                }
                            }
                        }
                    }
                } 
            }
            return list;
        }

        public Dictionary<Item, int> GetTrendPersonData(List<SocialMediaPost> posts)
        {
            Dictionary<Item, int> list = new Dictionary<Item, int>();
            foreach (var post in posts)
            {
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
            return list;
        }

        public Dictionary<Item, int> GetTrendOrganizationData(List<SocialMediaPost> posts)
        {
            Dictionary<Item, int> list = new Dictionary<Item, int>();
            foreach (var post in posts)
            {
                foreach (var person in post.Persons)
                {
                    if (person.Organization != null)
                    {
                        if (list.ContainsKey(person.Organization))
                        {
                            list[person.Organization]++;
                        }
                        else
                        {
                            list.Add(person.Organization, 1);
                        }
                    }
                }
            }
            return list;
        }

        public Dictionary<string, int> GetHashtagData(List<SocialMediaPost> posts)
        {
            Dictionary<string, int> list = new Dictionary<string, int>();
            foreach (var post in posts)
            {
                post.ListsToArrays();
                foreach (var hashtag in post.Hashtags)
                {
                    if (list.ContainsKey(hashtag.Value))
                    {
                        list[hashtag.Value]++;
                    }
                    else
                    {
                        list.Add(hashtag.Value, 1);
                    }
                }
            }
            return list;
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
            return list;
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
            return list.OrderByDescending(w => w.Value).Take(50).ToDictionary(pair => pair.Key, pair => pair.Value).Shuffle();
        }

        public List<string> GetTopTenUrlPerson(Person person)
        {
            List<string> allUrls = new List<string>();
            List<string> topTen = new List<string>();
            foreach (var post in person.SocialMediaPosts)
            {
                foreach (var url in post.Urls)
                {
                    allUrls.Add(url.Value);
                }
            }
            var sortedUrlsGroup = allUrls.GroupBy(s => s).OrderByDescending(g => g.Count());
            if ((sortedUrlsGroup.ToList()).Count() < 10)
            {
                foreach (var url in (sortedUrlsGroup.ToList()).GetRange(0, (sortedUrlsGroup.ToList()).Count()))
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

        public List<String> GetTopTenUrlOrganization(Organization organization)
        {
            List<string> allUrls = new List<string>();
            List<string> topTen = new List<string>();
            foreach (Person person in organization.Persons)
            {
                allUrls.AddRange(GetTopTenUrlPerson(person));
            }
            var sortedUrlsGroup = allUrls.GroupBy(s => s).OrderByDescending(g => g.Count());
            if ((sortedUrlsGroup.ToList()).Count() < 10)
            {
                foreach (var url in (sortedUrlsGroup.ToList()).GetRange(0, (sortedUrlsGroup.ToList()).Count()))
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

        public List<string> GetTopTenWordsPerson(Person person)
        {
            List<string> allWords = new List<string>();
            List<string> topTen = new List<string>();
            foreach (var post in person.SocialMediaPosts)
            {
                foreach (var word in post.Words)
                {
                    allWords.Add(word.Value);
                }
            }
            var sortedWordsGroup = allWords.GroupBy(s => s).OrderByDescending(g => g.Count());
            if ((sortedWordsGroup.ToList()).Count() < 10)
            {
                foreach (var word in (sortedWordsGroup.ToList()).GetRange(0, (sortedWordsGroup.ToList()).Count()))
                {
                    topTen.Add(word.Key);
                }
            }
            else
            {
                foreach (var word in (sortedWordsGroup.ToList()).GetRange(0, 10))
                {
                    topTen.Add(word.Key);
                }
            }
            return topTen;
        }

        public List<string> GetTopTenWordsOrganization(Organization organization)
        {
            List<string> allWords = new List<string>();
            List<string> topTen = new List<string>();
            foreach (Person person in organization.Persons)
            {
                allWords.AddRange(GetTopTenWordsPerson(person));
            }
            var sortedWordsGroup = allWords.GroupBy(s => s).OrderByDescending(g => g.Count());
            if ((sortedWordsGroup.ToList()).Count() < 10)
            {
                foreach (var word in (sortedWordsGroup.ToList()).GetRange(0, (sortedWordsGroup.ToList()).Count()))
                {
                    topTen.Add(word.Key);
                }
            }
            else
            {
                foreach (var url in (sortedWordsGroup.ToList()).GetRange(0, 10))
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

        public void ActivateAPI()
        {
            int minutes = settingsManager.GetSettings().ApiFrequency;
            if (minutes == 0)
            {
                minutes = 10;
            }
            var timer = new System.Threading.Timer(
            e => SynchronizeDatabase(),
            null,
            TimeSpan.Zero,
            TimeSpan.FromMinutes(minutes));
        }

        public Boolean ArraysToLists(SocialMediaPost post)
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
            return true;
        }
    }
}

