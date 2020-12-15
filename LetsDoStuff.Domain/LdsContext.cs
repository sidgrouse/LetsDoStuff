using LetsDoStuff.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LetsDoStuff.Domain
{
    public class LdsContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Activity> Activities { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public LdsContext(DbContextOptions<LdsContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OneToManyBetweenCreatorAndActivity(modelBuilder);
        }

        private void OneToManyBetweenCreatorAndActivity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()
                .HasMany(u => u.OwnActivities)
                .WithOne(a => a.Creator)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
