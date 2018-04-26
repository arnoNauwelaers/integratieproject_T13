using BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EF
{
    public class SocialMediaRepository
    {
        private BarometerDbContext ctx;

        public SocialMediaRepository(BarometerDbContext ctx)
        {
      this.ctx = ctx;
        }

        public IEnumerable<SocialMediaPost> ReadSocialMediaPosts()
        {
            return ctx.SocialMediaPosts.Include(a => a.SocialMediaProfiles).ToList<SocialMediaPost>();
        }

        public IEnumerable<SocialMediaPost> ReadSocialMediaPostsSince(DateTime since)
        {
            return ctx.SocialMediaPosts.Include(a => a.SocialMediaProfiles).ToList().FindAll(i => i.Date > since);
        }

        public SocialMediaPost CreateSocialMediaPost(SocialMediaPost socialMediaPost)
        {
            ctx.SocialMediaPosts.Add(socialMediaPost);
            ctx.SaveChanges();
            return socialMediaPost;
        }

        public void UpdateSocialMediaPost(SocialMediaPost socialMediaPost)
        {
            ctx.Entry(socialMediaPost).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();
        }

        public void DeleteSocialMediaPost(int postId)
        {
            SocialMediaPost socialMediaPost = ctx.SocialMediaPosts.Find(postId);
            ctx.SocialMediaPosts.Remove(socialMediaPost);
            ctx.SaveChanges();
        }
        public SocialMediaPost GetLastQueryDate()
        {
            return ctx.SocialMediaPosts.ToList().LastOrDefault<SocialMediaPost>();

        }

        public List<SocialMediaProfile> GetProfile(SocialMediaPost post)
        {
            List<SocialMediaProfile> tempprofiles = new List<SocialMediaProfile>();

            foreach (var profile in ctx.SocialMediaProfiles.ToList<SocialMediaProfile>())
            {
                if (profile.Item.Name == post.Person[1])
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
                        foreach (var compareprofile in ctx.SocialMediaProfiles.ToList<SocialMediaProfile>())
                        {
                            if (profile.Id == compareprofile.Id)
                            {
                                foreach (var post in compareprofile.SocialMediaPosts)
                                {
                                    int result = post.Date.CompareTo(start);
                                    int result2 = post.Date.CompareTo(end);
                                    if (result > 0 && result2 < 0)
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

        //public int ReadAmountHashtags(string hashtag)
        //{
        //    return ctx.SocialMediaPosts.ToList<SocialMediaPost>().Count(i => i.Hashtags.Contains(hashtag));
        //}
        //public int ReadAmountWords(string word)
        //{
        //    return ctx.SocialMediaPosts.ToList<SocialMediaPost>().Count(i => i.Words.Contains(word));
        //}
        //public int ReadAmountPersons(string person)
        //{
        //    return ctx.SocialMediaPosts.ToList<SocialMediaPost>().Count(i => i.Person.Contains(person));
        //}
    }
}
