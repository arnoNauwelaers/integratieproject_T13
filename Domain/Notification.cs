using System;
using System.ComponentModel.DataAnnotations;

namespace BL.Domain
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }
        public DateTime DateTime { get; set; }
        public Boolean Read { get; set; } = false;
        public virtual Alert Alert { get; set; }
        public string Content { get; set; }
        public Notification()
        {
            Read = false;
        }
    }
}
