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
        tweetsNegatief, //item tweets worden negatiever
        tweetsPositief, //item tweets worden positiever
        mentionsPositif, //mentions worden positiever
        mentionsNegatif // mentions worden negatiever
    }
}
