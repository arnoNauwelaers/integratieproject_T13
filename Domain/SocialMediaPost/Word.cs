using System.ComponentModel.DataAnnotations;

namespace BL.Domain
{
    public class Word
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
        public Word(string val)
        {
            Value = val;
        }


        public Word()
        {
        }
    }

}
