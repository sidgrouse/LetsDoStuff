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
                .HasForeignKey(u => u.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<User>()
                .HasMany(u => u.ParticipationActivities)
                .WithMany(a => a.Participants)
                .UsingEntity<ParticipantsTicket>(
                j => j
                    .HasOne(au => au.Activity)
                    .WithMany(a => a.ParticipantsTickets)
                    .HasForeignKey(au => au.ActivityId),
                j => j
                    .HasOne(au => au.User)
                    .WithMany(u => u.ParticipantsTickets)
                    .HasForeignKey(au => au.UserId),
                j =>
                {
                    j.Property(au => au.IsParticipant).HasDefaultValue(false);
                    j.HasKey(au => new { au.ActivityId, au.UserId });
                });
        }
    }
}
