using BL.Domain;
using System.Collections.Generic;

namespace DAL
{
    public class ItemRepository
    {
        List<Item> items; 
        public ItemRepository()
        {
            items = Memory.items;
        }

        public List<Item> ReadItems(SocialMediaPost post) //Lees welke items in post steken
        {
            List<Item> usedItems = new List<Item>();
            foreach (var item in items)
            {
                if(item.Name.ToUpper() == post.Politician[1].ToUpper())
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
