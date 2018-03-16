using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class SocialMediaPost
    {
        [Key]
        public int PostId { get; set; }
        public long Id { get; set; }
        public String[] Hashtags { get; set; }
        public String[] Words { get; set; }
        public DateTime Date { get; set; }
        public String[] Politician { get; set; }
        public String Geo { get; set; }
        public String User_id { get; set; }
        public double[] Sentiment { get; set; }
        public Boolean Retweet { get; set; }
        public String Source { get; set; }
        public String[] Urls { get; set; }
        public String[] Mentions { get; set; }

       
            public override String ToString()
        {
            return Source;
        }
    }
}
