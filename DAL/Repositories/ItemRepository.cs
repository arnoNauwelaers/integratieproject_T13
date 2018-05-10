using BL.Domain;
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
        private BarometerDbContext ctx;

        public ItemRepository(BarometerDbContext ctx)
        {
            this.ctx = ctx;
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
            return ctx.Items.OfType<Theme>().Include(k => k.Keywords).ToList<Theme>();
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

        public Item ReadItem(int id)
        {
            return ctx.Items.Find(id);
        }

        public void UpdateItem(Item changedItem)
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
            }
            if (changedItem.GetType().ToString().Contains("Theme"))
            {
                ((Theme)item).Keywords = ((Theme)changedItem).Keywords;
            }
            ctx.Entry(item).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();
        }

    public void DeleteItem(int itemId)
    {
        Item item = ctx.Items.Find(itemId);
            if (item.GetType().ToString().Contains("Theme"))
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
            return ctx.Organizations.Include(a => a.Alerts).Include(a => a.socialMediaProfiles).ToList().Find(u => u.ItemId == id);
        }

        public List<Organization> ReadOrganization(string name)
        {
            return ctx.Organizations.Where(o => o.Name == name).ToList();
        }

        public List<Theme> ReadTheme(string name)
        {
            return ctx.Themes.Where(t => t.Name == name).ToList();
        }

        public Keyword CreateKeyword(Item theme, string keyword)
        {
            Keyword keywordTemp = new Keyword() { Value = keyword, Theme = (Theme)theme};
            ctx.Keywords.Add(keywordTemp);
            ctx.SaveChanges();
            return keywordTemp;
        }

        public void DeleteKeyword(int KeywordId)
        {
            Keyword keyword = ctx.Keywords.Find(KeywordId);
            ctx.Keywords.Remove(keyword);
        }



    //TODO delete
        public Person ReadPerson(int id)
    {
        return ctx.Persons.Include(a => a.Alerts).Include(a => a.SocialMediaProfiles).ToList().Find(u => u.ItemId == id);
    }
   
    public Theme ReadTheme(int id)
    {
        return ctx.Themes.ToList().Find(u => u.ItemId == id);
    }

    public IEnumerable<Item> SearchItems(string SearchValue)
    {
        string s = SearchValue.ToUpper();
        return ctx.Items.ToList().Where(item => (
          item.TypeInt == 1 && (((Person)item).Name.ToUpper().Contains(s))) || (
          item.TypeInt == 2 && (item.Name.ToUpper().Contains(s))) || (
          item.TypeInt == 3 && (item.Name.ToUpper().Contains(s)))
         );
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
      return ctx.Themes.ToList();
    }


  }
}
