using System.Collections.Generic;
using System.Linq;
using DAL;
using BL.Domain;

namespace BL
{
    class ItemManager
    {
        private ItemRepository ItemRepository;

        public ItemManager()
        {
            ItemRepository = new ItemRepository();
        }

        public List<Item> GetAllItemsFromPosts(List<SocialMediaPost> data)
        {
            Dictionary<int, Item> alteredItems = new Dictionary<int, Item>();
            foreach (var post in data)
            {
                List<Item> items = ItemRepository.ReadItems(post);
                foreach (var item in items)
                {
                    //alteredItems.Add(item);
                    if (!alteredItems.ContainsKey(item.ItemId))
                    {
                        alteredItems.Add(item.ItemId, item);
                    }
                        
                }
            }
            return alteredItems.Values.ToList();
        }
    }
}
