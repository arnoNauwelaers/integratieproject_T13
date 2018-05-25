using DAL.EF;

namespace DAL
{
    public class UnitOfWork
    {
        public readonly BarometerDbContext Context = new BarometerDbContext();
    }
}
