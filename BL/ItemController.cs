using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BL.Domain;
using Newtonsoft.Json;
using System.Diagnostics;

namespace BL
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

        }
    }
}
