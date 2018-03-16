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
        private SocialMediaRepository repo;

        public SocialMediaManager()
        {
            this.repo = new SocialMediaRepository();
        }

        private List<int> GetItemIds()
        {
            return null;
        }
    }
}
