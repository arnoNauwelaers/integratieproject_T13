using System;

namespace BL.Domain
{
    public enum AlertParameter : byte
    {

        mentions = 0, // aantal tweets over item dalen/stijgen
        compared, // vergelijk aantal mentions met ander item
        sentiment, // worden tweets positiever/negatiever over item
        comparedSentiment // sentiment 2 items vergelijken
        //tweets = 0, //aantal tweets veranderd
        //mentions, //aantal mentios veranderd
        //tweetsNegative, //item tweets worden negatiever
        //tweetsPositive, //item tweets worden positiever
        //mentionsPositive, //mentions worden positiever
        //mentionsNegative // mentions worden negatiever
    }
}
