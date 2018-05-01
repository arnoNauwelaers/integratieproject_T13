using System.ComponentModel.DataAnnotations;

namespace BL.Domain
{
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
