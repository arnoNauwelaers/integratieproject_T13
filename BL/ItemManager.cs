using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BL
{
    class ItemManager
    {
        private ItemRepository ItemRepository;

        public ItemManager()
        {
            ItemRepository = new ItemRepository();
        }
    }
}
