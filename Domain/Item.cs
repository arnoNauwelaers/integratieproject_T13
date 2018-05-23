using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BL.Domain
{
    public class Item
    {
        [Key]
        public int ItemId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Alert> Alerts { get; set; } = new List<Alert>();
        public virtual ICollection<SocialMediaPost> SocialMediaPosts { get; set; } = new List<SocialMediaPost>();
        //Volgorde charts: itemwordsweekly(0), itemhashtagsweekly(1), itempostsmonthly(2)
        public virtual List<Chart> StandardCharts { get; set; } = new List<Chart>();


        public Item()
        {
        }
    }
}
