using BL.Domain;
using DAL;
using Domain;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class SocialMediaManager
    {
        private SocialMediaRepository SocialMediaRepository;
        private ItemManager ItemManager;
        private Read read;

        public SocialMediaManager()
        {
            SocialMediaRepository = new SocialMediaRepository();
            ItemManager = new ItemManager();
            read = new Read(); 
        }

        public List<Item> CreatePosts()
        {
            List<SocialMediaPost> data = (List<SocialMediaPost>) read.ReadData();
            foreach (var item in data)
            {
                SocialMediaRepository.Add(item);
            }
            SocialMediaRepository.setTrends();
            Debug.WriteLine("LOLOLOL");
            return ItemManager.GetAllItemsFromPosts(data);
        }
    }
}
