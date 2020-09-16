using app.api.Context;
using app.api.Entities;
using app.api.Helpers.Messaging;
using app.api.Helpers.Paging;
using app.api.Helpers.Users;
using app.api.Interfaces.Respositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace app.api.Repositories
{
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext context;
        public DatingRepository(DataContext context) => this.context = context;
        public void Add<T>(T entity) where T : class => context.Add(entity);
        public void Delete<T>(T entity) where T : class => context.Remove(entity);

        public async Task<User> GetUser(int id)
        {
            var user = await context.Users.Include(
                p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var users = context.Users.Include(
                p => p.Photos).OrderByDescending(u => u.LastActive).AsQueryable();

            users = users.Where(u => u.Id != userParams.UserId);
            users = users.Where(u => u.Gender == userParams.Gender);

            if (userParams.Likers)
            {
                var userLikers = await GetUserLikes(userParams.UserId, userParams.Likers);
                users = users.Where(u => userLikers.Contains(u.Id));
            }
            if (userParams.Likees)
            {
                var userLikees = await GetUserLikes(userParams.UserId, userParams.Likers);
                users = users.Where(u => userLikees.Contains(u.Id));
            }

            if (userParams.MinAge != 18 || userParams.MaxAge != 99)
            {
                var minimumDateOfBirth = DateTime.Today.AddYears(-userParams.MaxAge - 1);
                var maximumDateOfBirth = DateTime.Today.AddYears(-userParams.MinAge);

                users = users.Where(
                    u => u.DateOfBirth >= minimumDateOfBirth && u.DateOfBirth <= maximumDateOfBirth);
            }

            if (!string.IsNullOrEmpty(userParams.OrderBy))
            {
                switch(userParams.OrderBy)
                {
                    case "created":
                        users = users.OrderByDescending(u => u.Created);
                        break;
                    default:
                        users = users.OrderByDescending(u => u.LastActive);
                        break;
                }
            }

            return await PagedList<User>.CreateAsync(
                users, userParams.PageNumber, userParams.PageSize);
        }

        private async Task<IEnumerable<int>> GetUserLikes(int id, bool isLiker)
        {
            var user = await context.Users.Include(
                u => u.Likers).Include(u => u.Likees).FirstOrDefaultAsync(u => u.Id == id);

            if (isLiker)
            {
                return user.Likers.Where(u => u.LikeeId == id).Select(u => u.LikerId);
            }
            else
            {
                return user.Likees.Where(u => u.LikerId == id).Select(u => u.LikeeId);
            }
        }

        public async Task<bool> SaveAll() => await context.SaveChangesAsync() > 0;

        public async Task<Like> GetLike(int userId, int recipientId) => await context.Likes.FirstOrDefaultAsync(u => u.LikerId == userId && u.LikeeId == recipientId);

        public async Task<Photo> GetDefaultPhoto(int userId)
        {
            return await context.Photos.Where(
                u => u.UserId == userId).FirstOrDefaultAsync(
                p => p.IsDefault == true);
        }

        public Task<Photo> GetPhoto(int id)
        {
            var photo = context.Photos.FirstOrDefaultAsync(p => p.Id == id);
            return photo;
        }

        public async Task<Message> GetMessage(int id) => await context.Messages.FirstOrDefaultAsync(m => m.Id == id);

        public async Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId)
        {
            // Obtain the entire "conversation."
            var messages = await context.Messages.Include(
                m => m.Sender).ThenInclude(p => p.Photos).Include(
                m => m.Recipient).ThenInclude(p => p.Photos).Where(
                m => m.RecipientId == userId && m.SenderId == recipientId ||
                m.RecipientId == recipientId && m.SenderId == userId)
                .OrderByDescending(m => m.MessageSent).ToListAsync();

            return messages;
        }

        public async Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams)
        {
            var messages = context.Messages.Include(
                m => m.Sender).ThenInclude(p => p.Photos).Include(
                m => m.Recipient).ThenInclude(p => p.Photos).AsQueryable();

            switch (messageParams.MessageContainer)
            {
                case "Inbox":
                    messages = messages.Where(m => m.RecipientId == messageParams.UserId);
                    break;
                case "Outbox":
                    messages = messages.Where(m => m.SenderId == messageParams.UserId);
                    break;
                default:
                    messages = messages.Where(m => m.RecipientId == messageParams.UserId && m.IsRead == false);
                    break;
            }

            messages = messages.OrderByDescending(m => m.MessageSent);

            return await PagedList<Message>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }
    }
}
