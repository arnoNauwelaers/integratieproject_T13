using System;

namespace BL.Domain
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public DateTime DateTime { get; set; }
        public Boolean Read { get; set; } = false;
        public virtual Alert Alert { get; set; }
        public Notification()
        {
            Read = false;
        }
    }
}
