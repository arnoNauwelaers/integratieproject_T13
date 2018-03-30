using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public DateTime DateTime { get; set; }
        public Boolean Read { get; set; }
        public Alert Alert { get; set; }
        public Notification()
        {
            Read = false;
        }
    }
}
