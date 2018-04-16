using BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EF
{
    public class ItemRepository
    {
        private BarometerDbContext ctx;

        public ItemRepository()
        {
            ctx = BarometerDbContext.Create();
            ctx.Database.Initialize(false);
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
            ctx.Entry(item).State = System.Data.Entity.EntityState.Modified;
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
                if (item.Name.ToUpper() == post.Politician[1].ToUpper())
                {
                    usedItems.Add(item);
                }
                foreach (var hashtag in post.Hashtags)
                {
                    if (item.Name.ToUpper() == hashtag.ToUpper())
                    {
                        usedItems.Add(item);
                    }
                }
                foreach (var word in post.Words)
                {
                    if (item.Name.ToUpper() == word.ToUpper())
                    {
                        usedItems.Add(item);
                    }
                }
            }
            return usedItems;
        }
    }
}
