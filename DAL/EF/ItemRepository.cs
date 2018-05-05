using BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace DAL.EF
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

        public IEnumerable<Person> ReadPersons()
        {
            return ctx.Items.OfType<Person>().ToList<Person>();
        }

        public IEnumerable<Organization> ReadOrganizations()
        {
            return ctx.Items.OfType<Organization>().ToList<Organization>();
        }

        public IEnumerable<Theme> ReadThemes()
        {
            return ctx.Items.OfType<Theme>().Include(k => k.Keywords).ToList<Theme>();
        }

        public Item CreateItem(Item item)
        {
            ctx.Items.Add(item);
            ctx.SaveChanges();
            return item;
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



    public List<Item> ReadItems(SocialMediaPost post)
    {
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
          item.typeInt == 1 && (((Person)item).Name.ToUpper().Contains(s))) || (
          item.typeInt == 2 && (item.Name.ToUpper().Contains(s))) || (
          item.typeInt == 3 && (item.Name.ToUpper().Contains(s)))
         );
    }

    public void DeleteItem(Item i)
    {
        ctx.Items.Remove(i);
        ctx.SaveChanges();
    }
}
}
