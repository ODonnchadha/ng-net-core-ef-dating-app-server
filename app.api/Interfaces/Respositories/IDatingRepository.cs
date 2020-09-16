using app.api.Entities;
using app.api.Helpers.Paging;
using app.api.Helpers.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace app.api.Interfaces.Respositories
{
    public interface IDatingRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<Photo> GetDefaultPhoto(int userId);
        Task<Like> GetLike(int userId, int recipientId);
        Task<Message> GetMessage(int id);
        Task<PagedList<Message>> GetMessagesForUser();
        Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId);
        Task<Photo> GetPhoto(int id);
        Task<User> GetUser(int id);
        Task<PagedList<User>> GetUsers(UserParams userParams);
        Task<bool> SaveAll();
    }
}
