using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using BL;
using BL.Domain;

namespace Application
{
    public class ItemController
    {
        private SocialMediaManager SocialMediaManager;
        private UserManager UserManager;

        public ItemController()
        {
            SocialMediaManager = new SocialMediaManager();
            UserManager = new UserManager();
        }

        public void SynchronizeDatabase()
        {
            List<Item> alteredItems = SocialMediaManager.CreatePosts();
        }
    }
}
