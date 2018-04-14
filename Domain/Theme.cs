using System;
using System.Collections.Generic;
<<<<<<< HEAD
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
=======
>>>>>>> ebd51bc8e207e2e67e55de69ea5588a62435f057

namespace BL.Domain
{
    public class Theme
    {
        [Key]
        public int ThemeId { get; set; }
        public ICollection<string> Keywords { get; set; }
    }
}