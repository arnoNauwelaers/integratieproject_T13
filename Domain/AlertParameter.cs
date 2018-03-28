using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Domain
{
    public enum AlertParameter : byte
    {
        tweets = 0, //aantal tweets veranderd
        mentions, //aantal mentios veranderd
        tweetsNegative, //item tweets worden negatiever
        tweetsPositive, //item tweets worden positiever
        mentionsPositive, //mentions worden positiever
        mentionsNegative // mentions worden negatiever
    }
}
