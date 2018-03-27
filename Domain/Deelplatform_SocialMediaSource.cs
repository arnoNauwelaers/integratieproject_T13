using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    public class Deelplatform_SocialMediaSource
    {
        [Key]
        public int Deelplatform_SocialMediaSource_Id { get; set; }
        public Boolean Geactiveerd { get; set; }
        public DateTime LastRetrieval { get; set; }
        public int? Frequence { get; set; }
        public int? KeepDays { get; set; }

        [Required]
        public SocialMediaSource SocialMediaSource { get; set; }
        /*[Required]
        public Deelplatform Deelplatform { get; set; }*/
    }
}
