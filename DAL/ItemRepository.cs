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

        public List<Item> ReadItems(SocialMediaPost post)
        {
            return null;
        }

        public void CreateItems()
        {
            items.Add(new Person() { ItemId = 1, FirstName = "Imade", Name = "Annouri", Organization = (new Organization() { ItemId = 1, Name = "N-VA"})});
        }
    }
}
