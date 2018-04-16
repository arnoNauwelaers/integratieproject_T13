using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BL.Domain
{
    public class Alert
    {
        [Key]
        public int AlertId { get; set; }
        public virtual AlertType Type { get; set; }
        public virtual AlertParameter Parameter { get; set; }
        //TODO max length: 1
        public string Condition { get; set; }
        public string Content { get; set; }
        //[Required] geeft voorlopig error in database
        public virtual User User { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        [Required]
        public virtual Item Item { get; set; }
        public virtual Item CompareItem { get; set; }

        public Alert(int alertId, AlertType type, AlertParameter parameter, string condition, User user, Item item, Item compareItem = null)
        {
            Notifications = new List<Notification>();
            AlertId = alertId;
            Type = type;
            Parameter = parameter;
            Condition = condition;
            User = user;
            Item = item;
            if (CompareItem == null)
            {
                switch (Condition)
                {
                    case ">": Content = $"{Item.Name} is populair aan het worden."; break;
                    case "<": Content = $"{Item.Name} is minder populair aan het worden."; break;
                }
            }
            else
            {
                switch (Condition)
                {
                    case ">": Content = $"{Item.Name} is populair aan het worden dan {CompareItem.Name}"; break;
                    case "<": Content = $"{Item.Name} is minder populair aan het worden dan {CompareItem.Name}."; break;
                }
            }
        }

        public Alert()
        {
        }
    }
}
