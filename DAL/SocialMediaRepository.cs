using BL.Domain;
using System;
using System.Collections.Generic;

namespace DAL
{
    public class SocialMediaRepository : Memory
    {

        public List<SocialMediaPost> posts;
        public List<SocialMediaProfile> socialmediaprofiles;


        public SocialMediaRepository() : base()
        {
            posts = new List<SocialMediaPost>();
            socialmediaprofiles = base.SocialMediaProfiles;
        }

        public void Add(SocialMediaPost post)
        {
            List<SocialMediaProfile> tempprofiles = this.getProfile(post);
            post.SocialMediaProfiles = tempprofiles;
            foreach (var profile in tempprofiles)
            {
                profile.SocialMediaPosts.Add(post);
            }
            posts.Add(post);
        }

        public List<SocialMediaProfile> getProfile(SocialMediaPost post)
        {
            List<SocialMediaProfile> tempprofiles = new List<SocialMediaProfile>();

            foreach (var profile in socialmediaprofiles)
            {
                if (profile.Item.Name == post.Politician[1])
                {
                    tempprofiles.Add(profile);
                }
            }
            return tempprofiles;
        }

        public int ReadItemParameter(Alert alert, DateTime end, DateTime start)
        {
            int aantal = 0;
            if (alert.Item.GetType() == typeof(Person))
            {
                if (alert.Parameter == AlertParameter.tweets)
                {
                    foreach (var profile in ((Person)alert.Item).socialMediaProfiles)
                    {
                        foreach(var compareprofile in socialmediaprofiles)
                        {
                            if (profile.ProfileId == compareprofile.ProfileId)
                            {
                                foreach (var post in compareprofile.SocialMediaPosts)
                                {
                                    if (post.Date > start && post.Date < end)
                                    {
                                        aantal += 1;
                                    }
                                }
                            }
                        }
                        
                    }
                }
            }
            return aantal;
        }

        //TODO: code van jordi
        /*public IEnumerable<string> trendingPersons;
        public IEnumerable<string> trendingTerms;
        public IEnumerable<string> trendingArticles;

        public void setTrends()
        {
            setTrendingPoliticians();
            setTrendingTerms();
            setTrendingArticles();
        }

        public void setTrendingPoliticians()
        {
            Dictionary<string, int> p = new Dictionary<string, int>();
            foreach (var item in posts)
            {
                string name = string.Join(" ", item.Politician);
                if (!p.ContainsKey(name))
                {
                    p.Add(name, 1);
                }
                else
                {
                    p[name]++;
                }
            }
            p.ToList().Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
            trendingPersons = p.Keys.ToList();
            Debug.Write(trendingPersons);
        }

        public void setTrendingTerms()
        {
            Dictionary<string, int> t = new Dictionary<string, int>();
            foreach (var item in posts)
            {
                foreach (var word in item.Words)
                {
                    if (!t.ContainsKey(word))
                    {
                        t.Add(word, 1);
                    }
                    else
                    {
                        t[word]++;
                    }
                }
                foreach (var hashtag in item.Hashtags)
                {
                    if (!t.ContainsKey(hashtag))
                    {
                        t.Add(hashtag, 1);
                    }
                    else
                    {
                        t[hashtag]++;
                    }
                }
            }
            t.ToList().Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
            trendingTerms = t.Keys.ToList();
            Debug.WriteLine(trendingTerms);
        }

        public void setTrendingArticles()
        {
            Dictionary<string, int> a = new Dictionary<string, int>();
            foreach (var item in posts)
            {
                foreach (var article in item.Verhalen)
                {
                    if (!a.ContainsKey(article))
                    {
                        a.Add(article, 1);
                    }
                    else
                    {
                        a[article]++;
                    }
                }
            }
            a.ToList().Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
            trendingArticles = a.Keys.ToList();
            Debug.Write(trendingArticles);
        }*/

    }
}