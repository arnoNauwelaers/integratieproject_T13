using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public enum AlertType : byte
    {
        notification = 0,
        mail,
        notificationMail
    }
}
