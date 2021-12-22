using WebApiCors.Models;

namespace WebApiCors.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _context;
        public UserRepository(UserContext context)
        {
            _context = context;
        }

        public User Create(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public User GetByEmail(string email) => _context.Users.FirstOrDefault(x => x.Email == email);

        public User GetById(int id) => _context.Users.FirstOrDefault(x => x.Id == id);
    }
}
