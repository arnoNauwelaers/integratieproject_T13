using BL.Domain;

namespace DAL
{
    public class UserRepository
    {
        public User GetUser()
        {
            return Memory.users[0];
        }
    }
}
