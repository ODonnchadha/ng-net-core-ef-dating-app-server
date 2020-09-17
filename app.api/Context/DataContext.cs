using app.api.Entities;
using Microsoft.EntityFrameworkCore;

namespace app.api.Context
{
    public class DataContext : DbContext
    {
        public DbSet<Photo> Photos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Value> Values { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        /// <summary>
        /// Fluent API:
        /// One user can like many other users. Many other users can like a user. Many-to-many.
        /// e.g.: USER has one LIKER with many LIKEES.
        /// e.g.: USER has one LIKE with many LIKERS.
        /// EF: Two (2) One-to-many relationships.
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Like>().HasKey(k => new
            {
                k.LikerId,
                k.LikeeId
            });
            builder.Entity<Like>().HasOne(l => l.Likee).WithMany(
                l => l.Likers).HasForeignKey(l => l.LikeeId).OnDelete(
                DeleteBehavior.Restrict);
            builder.Entity<Like>().HasOne(l => l.Liker).WithMany(
                l => l.Likees).HasForeignKey(l => l.LikerId).OnDelete(
                DeleteBehavior.Restrict);

            builder.Entity<Message>().HasOne(m => m.Sender)
                .WithMany(m => m.MessagesSent).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Message>().HasOne(m => m.Recipient)
                .WithMany(m => m.MessagesReceived).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
