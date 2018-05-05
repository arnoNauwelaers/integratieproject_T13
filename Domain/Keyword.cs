using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    public class Keyword
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
        public virtual Theme Theme { get; set; }
    }
}
