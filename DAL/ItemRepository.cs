using BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ItemRepository : Memory
    {
        List<Item> items; 
        public ItemRepository() : base()
        {
            items = new List<Item>();
            CreateItems();
        }

        public List<Item> ReadItems(SocialMediaPost post) //Lees welke items in post steken
        {
            List<Item> usedItems = new List<Item>();
            foreach (var item in items)
            {
                foreach (var hashtag in post.Hashtags)
                {
                    if (item.Name == hashtag)
                    {
                        usedItems.Add(item);
                    }
                }
                foreach (var word in post.Words)
                {
                    if (item.Name == word)
                    {
                        usedItems.Add(item);
                    }
                }
            }
            return usedItems;
        }

        public void CreateItems()
        {
            items.Add(new Person() { ItemId = 1, FirstName = "Imade", Name = "Annouri", Organization = (new Organization() { ItemId = 1, Name = "N-VA"})});
        }
    }
}
