using BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;
using DAL.EF;
using System.Diagnostics;

namespace DAL.Repositories
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
            return ctx.SocialMediaPosts.Where(i => i.Date > since).Include(a => a.SocialMediaProfiles).Include(a => a.Words).Include(a => a.Hashtags).Include(a => a.Persons).ToList();
        }

        public SocialMediaPost CreateSocialMediaPost(SocialMediaPost socialMediaPost)
        {
            ctx.SocialMediaPosts.Add(socialMediaPost);
            ctx.SaveChanges();
            return socialMediaPost;
        }


        public void UpdateSocialMediaPost(SocialMediaPost socialMediaPost)
        {
            ctx.Set<SocialMediaPost>().AddOrUpdate(socialMediaPost);
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
                if (alert.Parameter == AlertParameter.mentions)
                {
                    foreach (var profile in ((Person)alert.Item).SocialMediaProfiles)
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

        public int ReadNrOfPostsFromItem(Item i, DateTime end, DateTime start)
        {
            if (i is Person) { return ReadNrOfPostsFromPerson((Person)i, end, start); }
            if (i is Organization) { return ReadNrOfPostsFromOrganization((Organization)i, end, start); }
            else { return ReadNrOfPostsFromTheme((Theme)i, end, start); }
        }

        private int ReadNrOfPostsFromPerson(Person p, DateTime end, DateTime start)
        {
            return ctx.SocialMediaPosts.Count(smp => smp.Persons.Contains(p) && end >= smp.Date && start <= smp.Date);

        }

        private int ReadNrOfPostsFromOrganization(Organization o, DateTime end, DateTime start)
        {
            int total = 0;
            foreach (Person p in o.Persons)
            {
                total += ReadNrOfPostsFromPerson(p, end, start);
            }
            return total;
        }

        private int ReadNrOfPostsFromTheme(Theme t, DateTime end, DateTime start)
        {
            int total = 0;
            total += ctx.SocialMediaPosts.Count(smp => smp.Themes.Contains(t));
            foreach (Keyword k in t.Keywords)
            { total += ctx.SocialMediaPosts.Count(smp => !smp.Themes.Contains(t) && smp.Words.Contains(ReadWord(k.Value))); }
            return total;
        }

        public double ReadAverageSentimentFromItem(Item i, DateTime end, DateTime start)
        {
            if (i is Person) { return ReadAverageSentimentFromPerson((Person)i, end, start); }
            if (i is Organization) { return ReadAverageSentimentFromOrganization((Organization)i, end, start); }
            else { return ReadAverageSentimentFromTheme((Theme)i, end, start); }
        }

        private double ReadAverageSentimentFromPerson(Person p, DateTime end, DateTime start)
        {
            List<SocialMediaPost> posts = ctx.SocialMediaPosts.Where(smp => smp.Persons.Contains(p) && end >= smp.Date && start <= smp.Date).Include(smp => smp.PostSentiment).ToList();
            double average = 0.00;
            foreach (SocialMediaPost post in posts)
            {
                average += post.PostSentiment.GetSentiment();
            }
            average /= posts.Count;
            return average;
        }

        private double ReadAverageSentimentFromOrganization(Organization o, DateTime end, DateTime start)
        {
            double average = 0.00;
            foreach (Person p in o.Persons)
            {
                average += ReadAverageSentimentFromPerson(p, end, start);
            }
            return average;
        }

        private double ReadAverageSentimentFromTheme(Theme t, DateTime end, DateTime start)
        {
            double average = 0.00;
            int amount = 0;
            List<SocialMediaPost> posts = ctx.SocialMediaPosts.Where(smp => smp.Themes.Contains(t) && end >= smp.Date && start <= smp.Date).Include(smp => smp.PostSentiment).ToList();
            foreach (SocialMediaPost post in posts)
            {
                average += post.PostSentiment.GetSentiment();
            }
            amount += posts.Count;
            foreach (Keyword k in t.Keywords)
            {
                posts = ctx.SocialMediaPosts.Where(smp => !(smp.Themes.Contains(t) && smp.Words.Contains(ReadWord(k.Value)) && end >= smp.Date && start <= smp.Date)).Include(smp => smp.PostSentiment).ToList();
                foreach (SocialMediaPost post in posts) { average += post.PostSentiment.GetSentiment(); }
                amount += posts.Count;
            }
            return average;

        }



        private Word ReadWord(string value)
        {
            return ctx.Words.Single(w => w.Value == value);
        }



        public List<SocialMediaProfile> ReadProfiles()
        {
            return ctx.SocialMediaProfiles.ToList();
        }

        public SocialMediaProfile ReadProfile(int id)
        {
            return ctx.SocialMediaProfiles.Find(id);
        }

        public void SaveDatabase()
        {
            ctx.SaveChanges();
        }
    }
}
