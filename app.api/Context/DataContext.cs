using app.api.Entities;
using Microsoft.EntityFrameworkCore;

namespace app.api.Context
{
    public class DataContext : DbContext
    {
        public DbSet<Photo> Photos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Value> Values { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    }
}
