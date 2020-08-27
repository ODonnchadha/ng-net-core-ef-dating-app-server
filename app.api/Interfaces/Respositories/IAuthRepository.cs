using app.api.Entities;
using System.Threading.Tasks;

namespace app.api.Interfaces.Respositories
{
    public interface IAuthRepository
    {
        Task<bool> UserExists(string userName);
        Task<User> Login(string userName, string password);
        Task<User> Register(User user, string password);
    }
}
