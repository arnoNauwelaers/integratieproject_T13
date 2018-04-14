using BL.Domain;
using System;
using System.Collections.Generic;

namespace DAL
{
    //TO-DO verwijder klasse en bekijk code van Jordi
    public class SocialMediaRepository2
    {

        public List<SocialMediaPost> posts;
        public List<SocialMediaProfile> socialmediaprofiles;


        public SocialMediaRepository2()
        {
            posts = new List<SocialMediaPost>();
            socialmediaprofiles = Memory.SocialMediaProfiles;
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
                                    int result = post.Date.CompareTo(start);
                                    int result2 = post.Date.CompareTo(end);
                                    if ( result > 0 && result2 < 0)
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
    }
}