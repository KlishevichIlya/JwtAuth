using WebApiCors.Models;

namespace WebApiCors.Data
{
    public interface IUserRepository
    {
        User Create(User user);
        User GetByEmail(string email);

        User GetById(int id);
    }
}
