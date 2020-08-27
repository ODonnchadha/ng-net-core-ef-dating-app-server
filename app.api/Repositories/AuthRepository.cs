using app.api.Context;
using app.api.Entities;
using app.api.Helpers.Auth;
using app.api.Interfaces.Respositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace app.api.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext context;
        public AuthRepository(DataContext context) => this.context = context;

        /// <summary>
        /// FirstOrDefaultAsync will return NULL if not found. An exception will not be thrown.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<User> Login(string userName, string password)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            if (null == user)
            {
                return null;
            }

            if (!PasswordHash.Verify(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            return user;
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            PasswordHash.Create(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> UserExists(string userName)
        {
            if (await context.Users.AnyAsync(u => u.UserName == userName))
            {
                return true;
            }

            return false;
        }
    }
}
