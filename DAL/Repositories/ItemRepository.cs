﻿using BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using DAL.EF;

namespace DAL.Repositories
{
    public class ItemRepository
    {
        private readonly BarometerDbContext ctx;

        public ItemRepository(UnitOfWork uow)
        {
            this.ctx = uow.Context;
        }

        public IEnumerable<Item> ReadItems()
        {
            return ctx.Items.Include(a => a.Alerts).ToList<Item>();
        }

        public List<Item> ReadItems(string s)
        {
            return ctx.Items.Where(i => i.Name.ToUpper()
                              .Contains(s.ToUpper()))
                              .ToList();
        }

        public IEnumerable<Person> GetPersons()
        {
            return ctx.Items.OfType<Person>().ToList<Person>();
        }

        public IEnumerable<Organization> GetOrganizations()
        {
            return ctx.Items.OfType<Organization>().ToList<Organization>();
        }

        public IEnumerable<Theme> GetThemes()
        {
            return ctx.Items.OfType<Theme>().Include(t => t.Keywords).ToList<Theme>();
        }

        public Item CreateItem(Item item)
        {
            ctx.Items.Add(item);
            ctx.SaveChanges();
            return item;
        }

        public Person CreatePersonIfNotExists(string name)
        {
            if (ctx.Persons.Any(p => p.Name == name))
            {
                Person person = ctx.Persons.First(p => p.Name == name);
                return person;
            }
            else
            {
                return new Person(name);
            }
        }

        public SocialMediaProfile CreateSocialmediaprofile(SocialMediaProfile socialMediaProfile)
        {
            ctx.SocialMediaProfiles.Add(socialMediaProfile);
            ctx.SaveChanges();
            return socialMediaProfile;
        }

        public void DeleteProfile(int profileId)
        {
            ctx.SocialMediaProfiles.Remove(ctx.SocialMediaProfiles.Find(profileId));
            ctx.SaveChanges();
        }

        public Item ReadItem(int id)
        {
            return ctx.Items.Find(id);
        }

        public Item UpdateItem(Item changedItem)
        {
            Item item = ctx.Items.Find(changedItem.ItemId);
            if (item.Name != changedItem.Name)
            {
                item.Name = changedItem.Name;
            }
            if (changedItem.GetType().ToString().Contains("Person"))
            {
                if (((Person)item).Organization != ((Person)changedItem).Organization)
                {
                    ((Person)item).Organization = ((Person)changedItem).Organization;
                }
                if (((Person)changedItem).SocialMediaProfiles.Count != 0)
                {
                    ((Person)item).SocialMediaProfiles = ((Person)changedItem).SocialMediaProfiles;
                }
            }
            if (changedItem.GetType().ToString().Contains("Organization"))
            {
                if (((Organization)changedItem).SocialMediaProfiles.Count != 0)
                {
                    ((Organization)item).SocialMediaProfiles = ((Organization)changedItem).SocialMediaProfiles;
                }
                ((Organization)item).Persons = ((Organization)changedItem).Persons;
            }
            if (changedItem.GetType().ToString().Contains("Theme"))
            {
                ((Theme)item).Keywords = ((Theme)changedItem).Keywords;
            }
            ctx.Entry(item).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();
            return item;
        }



        public void DeleteItem(int itemId)
        {
            Item item = ctx.Items.Find(itemId);
            if (item.GetType().ToString().Contains("Person"))
            {
                List<SocialMediaProfile> tempSocialmediaprofiles =  ctx.SocialMediaProfiles.Where(smp => smp.Item.ItemId == item.ItemId).ToList();
                foreach(var profile in tempSocialmediaprofiles)
                {
                    profile.Item = null;
                    ((Person)item).SocialMediaProfiles.Remove(profile);
                }
                ((Person)item).Organization = null;
                (ctx.Organizations.Find(((Person)item).Organization.ItemId)).Persons.Remove((Person)item);
                List<SocialMediaPost> tempSocialemediaposts = (ctx.SocialMediaPosts.Where(smp => smp.Persons.Any(p => p.ItemId == item.ItemId))).ToList();
                foreach(var post in tempSocialemediaposts)
                {
                    post.Persons.Remove((Person)item);
                    ((Person)item).SocialMediaPosts.Remove(post);
                }
                List<Chart> tempCharts = (ctx.Charts.Where(c => c.Items.Any(i => i.ItemId == item.ItemId))).ToList();
                foreach(var chart in tempCharts)
                {
                    chart.Items.Remove(item);
                }
                ctx.SocialMediaProfiles.RemoveRange(ctx.SocialMediaProfiles.Where(smp => smp.Item == null));
                ctx.SaveChanges();
            }
            else if (item.GetType().ToString().Contains("Organization"))
            {
                List<SocialMediaProfile> tempSocialmediaprofiles = ctx.SocialMediaProfiles.Where(smp => smp.Item.ItemId == item.ItemId).ToList();
                foreach (var profile in tempSocialmediaprofiles)
                {
                    profile.Item = null;
                    ((Organization)item).SocialMediaProfiles.Remove(profile);
                }
                ctx.SocialMediaProfiles.RemoveRange(ctx.SocialMediaProfiles.Where(smp => smp.Item.ItemId == item.ItemId));
                foreach(Person person in ((Organization)item).Persons)
                {
                    person.Organization = null;
                }
                ctx.SaveChanges();
            }
            else if (item.GetType().ToString().Contains("Theme"))
            {
                ctx.Keywords.RemoveRange(ctx.Keywords.Where(k => k.Theme.ItemId == item.ItemId));
            }
            ctx.Items.Remove(item);
            ctx.SaveChanges();
        }

        public Item ReadItem(string s)
        {
            return ctx.Items.FirstOrDefault(i => i.Name.Contains(s));
        }



        public List<Item> ReadItems(SocialMediaPost post)
        {
            post.ListsToArrays();
            List<Item> usedItems = new List<Item>();
            foreach (var item in ctx.Items.ToList<Item>())
            {
                if (item.Name.ToUpper() == post.Person[0].ToUpper())
                {
                    usedItems.Add(item);
                }
                foreach (var hashtag in post.Hashtag)
                {
                    if (item.Name.ToUpper() == hashtag.ToUpper())
                        if (item.Name.ToUpper() == post.Persons.First().ToString().ToUpper())

                        {
                            usedItems.Add(item);
                        }
                }
                foreach (var word in post.Word)
                {
                    if (item.Name.ToUpper() == word.ToUpper())
                    {
                        usedItems.Add(item);
                    }
                }
            }
            return usedItems;
        }

        public Organization ReadOrganization(int id)
        {
            return ctx.Organizations.Include(a => a.Alerts).Include(a => a.SocialMediaProfiles).ToList().Find(u => u.ItemId == id);
        }

        public List<Organization> ReadOrganization(string name)
        {
            return ctx.Organizations.Include(a => a.StandardCharts).Where(o => o.Name == name).ToList();
        }

        public List<Theme> ReadTheme(string name)
        {
            return ctx.Themes.Where(t => t.Name == name).ToList();
        }

        public List<SocialMediaProfile> ReadProfiles(Item item)
        {
            return ctx.SocialMediaProfiles.Where(smp => smp.Item.ItemId == item.ItemId).ToList();
        }

        public SocialMediaProfile ReadProfiles(int profileId)
        {
            return ctx.SocialMediaProfiles.Find(profileId);
        }

        public SocialMediaProfile UpdateProfile(SocialMediaProfile profile)
        {
            SocialMediaProfile tempProfile = ctx.SocialMediaProfiles.Find(profile.Id);
            tempProfile.Url = profile.Url;
            ctx.Entry(tempProfile).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();
            return tempProfile;
        }

        public Keyword CreateKeyword(Item theme, string keyword)
        {
            Keyword keywordTemp = new Keyword() { Value = keyword, Theme = (Theme)theme };
            ctx.Keywords.Add(keywordTemp);
            ctx.SaveChanges();
            return keywordTemp;
        }

        public void DeleteKeyword(int KeywordId)
        {
            Keyword keyword = ctx.Keywords.Find(KeywordId);
            ctx.Keywords.Remove(keyword);
        }

        public Person ReadPerson(int id)
        {
            return ctx.Persons.Include(a => a.Alerts).Include(a => a.SocialMediaProfiles).Include(a => a.StandardCharts).ToList().Find(u => u.ItemId == id);
        }

        public Theme ReadTheme(int id)
        {
            return ctx.Themes.Include(t => t.Keywords).Include(a => a.StandardCharts).ToList().Find(u => u.ItemId == id);
        }

        public IEnumerable<Item> SearchItems(string SearchValue)
        {
            string s = SearchValue.ToUpper();
            return ctx.Items.ToList().Where(item => item.Name.ToUpper().Contains(s));
        }

        public void DeleteItem(Item i)
        {
            ctx.Items.Remove(i);
            ctx.SaveChanges();
        }

        public List<Organization> ReadOrganizations()
        {
            return ctx.Organizations.ToList();
        }

        public List<Person> ReadPersons()
        {
            return ctx.Persons.ToList();
        }


        public List<Theme> ReadThemes()
        {
            return ctx.Themes.Include(t => t.Keywords).ToList();
        }
    }
}
