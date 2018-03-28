using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            List<Item> alteredItems = new List<Item>();
            foreach (var post in data)
            {
                List<Item> items = ItemRepository.ReadItems(post);
                foreach (var item in items)
                {
                    alteredItems.Add(item);
                }
            }
            return alteredItems;
        }
    }
}
