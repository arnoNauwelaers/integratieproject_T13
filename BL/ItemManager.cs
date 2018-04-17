using System.Collections.Generic;
using System.Linq;
using DAL;
using BL.Domain;
using DAL.EF;

namespace BL
{
    class ItemManager
    {
        private ItemRepository itemRepository;

        public ItemManager()
        {
            itemRepository = new ItemRepository();
        }

        public List<Item> GetAllItemsFromPosts(List<SocialMediaPost> data)
        {
            Dictionary<int, Item> alteredItems = new Dictionary<int, Item>();
            foreach (var post in data)
            {
                List<Item> items = itemRepository.ReadItems(post);
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
