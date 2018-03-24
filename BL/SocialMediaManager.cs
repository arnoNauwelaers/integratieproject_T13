using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class SocialMediaManager
    {
        private SocialMediaRepository SocialMediaRepository;
        private ItemManager ItemManager;

        public SocialMediaManager()
        {
            SocialMediaRepository = new SocialMediaRepository();
            ItemManager = new ItemManager();
        }
    }
}
