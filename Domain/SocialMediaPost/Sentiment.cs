using System.ComponentModel.DataAnnotations;

namespace BL.Domain
{
    public class Sentiment
    {
        [Key]
        public int Id { get; set; }
        public double Polarity { get; set; }
        public double Subjectivity { get; set; }
        public Sentiment(double pol, double sub)
        {
          Polarity = pol;
          Subjectivity = sub;
        }

        public Sentiment()
        {

        }

        public double GetSentiment()
        {
          // not sure how to calculate this correctly, this should be enough for now
          return Polarity * Subjectivity;

        }
    }
}
