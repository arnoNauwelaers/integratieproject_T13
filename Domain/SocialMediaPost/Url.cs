using System.ComponentModel.DataAnnotations;

namespace BL.Domain
{
    public class Url
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
        public Url(string val)
        {
            Value = val;
        }

        public Url()
        {
        }
    }
}
