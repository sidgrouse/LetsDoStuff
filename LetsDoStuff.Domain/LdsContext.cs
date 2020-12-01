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
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
              .Entity<Activity>()
              .HasMany(a => a.Tags)
              .WithMany(t => t.Activities)
              .UsingEntity<ActivityTag>(
                j => j
                  .HasOne(at => at.Tag)
                  .WithMany()
                  .HasForeignKey(at => at.TagId),
                j => j
                  .HasOne(at => at.Activity)
                  .WithMany()
                  .HasForeignKey(at => at.ActivityId),
                j =>
                {
                    j.HasKey(t => new { t.ActivityId, t.TagId });
                });
        }
    }
}
