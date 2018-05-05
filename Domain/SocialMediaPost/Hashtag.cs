using System.ComponentModel.DataAnnotations;

namespace BL.Domain
{
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
}
