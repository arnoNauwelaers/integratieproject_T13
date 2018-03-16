using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class SocialMediaRepository
    {
        private static List<SocialMediaPost> SocialMediaPosts = new List<SocialMediaPost>();
        public SocialMediaRepository()
        {
            SocialMediaPost post1 = new SocialMediaPost() { PostId = 1, Hashtags = new String[] { "an", "hu", "ri" }, Date = DateTime.Now, Politician = new String[] { "Dewever", "Bart" }, Geo = "N/A", Id = 15145456446, User_id = "N/A", Sentiment = new Double[] { -1.0, 0.0 }, Retweet = true, Source = "Twitter", Urls = new string[] { "www.google.com", "www.facebook.com" }, Mentions = new String[] { }, Words = new String[] { "an", "hi", "ru" } };
            SocialMediaPost post2 = new SocialMediaPost() { PostId = 2, Hashtags = new String[] { "adsdn", "hqsdu", "rqsdi" }, Date = DateTime.Now, Politician = new String[] { "Dewever", "Bart" }, Geo = "N/A", Id = 15145456447, User_id = "N/A", Sentiment = new Double[] { 1.0, 0.0 }, Retweet = true, Source = "Twitter", Urls = new string[] { "www.google.com", "www.facebook.com" }, Mentions = new String[] { }, Words = new String[] { "afsdfn", "hfsdfi", "rdqdu" } };
            SocialMediaPosts.Add(post1);
            SocialMediaPosts.Add(post2);
        }

        public SocialMediaPost getSocialMediaPost(int id)
        {
            foreach (SocialMediaPost post in SocialMediaPosts)
            {
                if (post.PostId == id)
                {
                    return post;
                }
            }
            return null;
        }


    }
}
