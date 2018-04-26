using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace BL.Domain
{
    public class SocialMediaPost
    {
        [Key]
        public long PostId { get; set; }
        [JsonProperty("id")]
        public string TweetId { get; set; }
        //TODO geo kan zowel array van double en boolean zijn
        //[JsonProperty("geo")]
        //public double[] Geo { get; set; }
        [JsonProperty("retweet")]
        public Boolean Retweet { get; set; }
        [JsonProperty("date")]
        public DateTime Date { get; set; }
        [JsonProperty("sentiment")]
        [NotMapped]
        public double[] Sentiment { get; set; } = new double[10];
        [JsonProperty("hashtags")]
        [NotMapped]
        public string[] Hashtag { get; set; } = new string[10];
        [JsonProperty("urls")]
        [NotMapped]
        public string[] Verhaal { get; set; } = new string[10];
        [JsonProperty("words")]
        [NotMapped]
        public string[] Word { get; set; } = new string[10];
        [JsonProperty("persons")]
        [NotMapped]
        public string[] Person { get; set; } = new string[10];
        [JsonProperty("themes")]
        [NotMapped]
        public string[] Theme { get; set; } = new string[10];
        [JsonProperty("source")]
        public string Source { get; set; }
        [JsonProperty("profile")]
        public virtual SocialMediaProfile SocialMediaProfile { get; set; }
        public virtual SocialMediaSource SocialMediaSource { get; set; }
        public virtual ICollection<SocialMediaProfile> SocialMediaProfiles { get; set; } = new List<SocialMediaProfile>();
        public virtual ICollection<Sentiment> Sentiments { get; set; } = new List<Sentiment>();
        public virtual ICollection<Hashtag> Hashtags { get; set; } = new List<Hashtag>();
        public virtual ICollection<Verhaal> Verhalen { get; set; } = new List<Verhaal>();
        public virtual ICollection<Woord> Woorden { get; set; } = new List<Woord>();
        public virtual ICollection<Persoon> Personen { get; set; } = new List<Persoon>();
        public virtual ICollection<Thema> Themas { get; set; } = new List<Thema>();

        public void ArraysToLists()
        {
            foreach (var item in Sentiment)
            {
                Sentiments.Add(new Sentiment(item));
            }
            foreach (var item in Hashtag)
            {
                Hashtags.Add(new Hashtag(item));
            }
            foreach (var item in Verhaal)
            {
                Verhalen.Add(new Verhaal(item));
            }
            foreach (var item in Word)
            {
                Woorden.Add(new Woord(item));
            }
            foreach (var item in Person)
            {
                Personen.Add(new Persoon(item));
            }
            foreach (var item in Theme)
            {
                Themas.Add(new Thema(item));
            }
        }

        public void ListsToArrays()
        {
            int i = 0;
            foreach (var item in Sentiments)
            {
                Sentiment[i] = item.Value;
                i++;
            }
            i = 0;
            foreach (var item in Hashtags)
            {
                Hashtag[i] = item.Value;
                i++;
            }
            i = 0;
            foreach (var item in Verhalen)
            {
                Verhaal[i] = item.Value;
                i++;
            }
            i = 0;
            foreach (var item in Woorden)
            {
                Word[i] = item.Value;
                i++;
            }
            i = 0;
            foreach (var item in Personen)
            {
                Person[i] = item.Value;
                i++;
            }
            i = 0;
            foreach (var item in Themas)
            {
                Theme[i] = item.Value;
                i++;
            }
        }
    }

    public class Hashtag
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
        public Hashtag(string val)
        {
            Value = val;
        }

        public Hashtag()
        {
        }
    }

    public class Verhaal
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
        public Verhaal(string val)
        {
            Value = val;
        }

        public Verhaal()
        {
        }
    }


    public class Woord
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
        public Woord(string val)
        {
            Value = val;
        }

        public Woord()
        {
        }
    }

    public class Persoon
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
        public Persoon(string val)
        {
            Value = val;
        }

        public Persoon()
        {
        }
    }

    public class Thema
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
        public Thema(string val)
        {
            Value = val;
        }

        public Thema()
        {
        }
    }

    public class Sentiment
    {
        [Key]
        public int Id { get; set; }
        public double Value { get; set; }
        public Sentiment(double val)
        {
            Value = val;
        }

        public Sentiment()
        {
        }
    }
}
