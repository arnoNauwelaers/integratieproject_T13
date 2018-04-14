using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BL.Domain
{
    public class Alert
    {
        [Key]
        public int AlertId { get; set; }
        public AlertType Type { get; set; }
        public AlertParameter Parameter { get; set; }
        public char Condition { get; set; }
        public string Content { get; set; }
        [Required]
        public User User { get; set; }
        public List<Notification> Notifications { get; set; }
        [Required]
        public Item Item { get; set; }
        public Item CompareItem { get; set; }

        public Alert(int alertId, AlertType type, AlertParameter parameter, char condition, User user, Item item, Item compareItem = null)
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
                    case '>': Content = $"{Item.Name} is populair aan het worden."; break;
                    case '<': Content = $"{Item.Name} is minder populair aan het worden."; break;
                }
            }
            else
            {
                switch (Condition)
                {
                    case '>': Content = $"{Item.Name} is populair aan het worden dan {CompareItem.Name}"; break;
                    case '<': Content = $"{Item.Name} is minder populair aan het worden dan {CompareItem.Name}."; break;
                }
            }
        }

        public Alert()
        {
            Notifications = new List<Notification>();
        }
    }
}
