﻿using app.api.Context;
using app.api.Entities;
using app.api.Interfaces.Respositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await context.Users.Include(p => p.Photos).ToListAsync();
            return users;
        }

        public async Task<bool> SaveAll() => await context.SaveChangesAsync() > 0;
    }
}