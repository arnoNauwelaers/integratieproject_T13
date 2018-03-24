using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Alert
    {
        [Key]
        public int AlertId { get; set; }
        public AlertType Type { get; set; }
        public AlertParameter Parameter { get; set; }
        public char Condition { get; set; }

        [Required]
        public User User { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        [Required]
        public Item Item { get; set; }
        public Item CompareItem { get; set; }
    }
}
