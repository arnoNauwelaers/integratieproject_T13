using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BL.Domain
{
    public class Theme
    {
        [Key]
        public int ThemeId { get; set; }
        public ICollection<string> Keywords { get; set; }
    }
}