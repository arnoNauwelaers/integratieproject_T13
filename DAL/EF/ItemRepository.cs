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

        public Item CreateItem(Item item)
        {
            ctx.Items.Add(item);
            ctx.SaveChanges();
            return item;
        }

        public void UpdateItem(Item item)
        {
            ctx.Entry(item).State = EntityState.Modified;
            ctx.SaveChanges();
        }

        public void DeleteItem(int itemId)
        {
            Item item = ctx.Items.Find(itemId);
            ctx.Items.Remove(item);
            ctx.SaveChanges();
        }

        public List<Item> ReadItems(SocialMediaPost post) 
        {
            List<Item> usedItems = new List<Item>();
            foreach (var item in ctx.Items.ToList<Item>())
            {
                if (item.Name.ToUpper() == post.Persons.First().ToString().ToUpper())
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

    public Person ReadPerson(int id) {
      return ctx.Persons.Include(a => a.Alerts).Include(a => a.SocialMediaProfiles).ToList().Find(u => u.ItemId == id);
    }
    public Organization ReadOrganization(int id) {
      return ctx.Organizations.Include(a => a.Alerts).Include(a => a.socialMediaProfiles).ToList().Find(u => u.ItemId == id);
    }
    public Theme ReadTheme(int id) {
      return ctx.Themes.ToList().Find(u => u.ItemId == id);
    }

    public IEnumerable<Item> SearchItems(string SearchValue) {
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


    public List<Organization> ReadOrganizations()
    {
      return ctx.Organizations.ToList();
    }

    public List<Person> ReadPersons()
    {
      return ctx.Persons.ToList();
    }

    public Item ReadItem(int id)
    {
      return ctx.Items.Find(id);
    }
  }
}
