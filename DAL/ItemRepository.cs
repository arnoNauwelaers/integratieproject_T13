using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ItemRepository
    {
        private IList<Item> items { get; set; }

       

        public ItemRepository()
        {
            items = new List<Item>();
            Item item = new Item(1, "Bart Dewever");
            items.Add(item);
        }

        public Item getItem(int itemId)
        {
            foreach (Item item in items)
            {
                if (item.Id == itemId)
                {
                    return item;
                }
            }
            return null;
        }
    }
}
