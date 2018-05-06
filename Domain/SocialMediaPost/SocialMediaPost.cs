﻿using System;
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
        public virtual ICollection<Url> Urls { get; set; } = new List<Url>();
        public virtual ICollection<Word> Words { get; set; } = new List<Word>();
        public virtual ICollection<Person> Persons { get; set; } = new List<Person>();
        public virtual ICollection<Theme> Themes { get; set; } = new List<Theme>();

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
                Urls.Add(new Url(item));
            }
            foreach (var item in Word)
            {
                Words.Add(new Word(item));
            }
            // er horen geen nieuwe personen aangemaakt te worden, ze moeten gelinkt worden aan al bestaande personen. Nu gelinkt in SMManager
            //foreach (var item in Person)
            //{
            //    Persons.Add(new Person(item));
            //}
            foreach (var item in Theme)
            {
                Themes.Add(new Theme(item));
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
            foreach (var item in Urls)
            {
                Verhaal[i] = item.Value;
                i++;
            }
            i = 0;
            foreach (var item in Words)
            {
                Word[i] = item.Value;
                i++;
            }
            i = 0;
            ///*TODO Mathijs vragen
            //foreach (var item in Persons)
            //{
            //    Person[i] = item.Value;
            //    i++;
            //}
            //*/
            i = 0;
            foreach (var item in Themes)
            {
                Theme[i] = item.Name;
                i++;
            }
        }
    }

}
