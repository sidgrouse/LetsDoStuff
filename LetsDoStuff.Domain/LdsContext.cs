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
            ManyToManyBetweenActivitiesAndTegs(modelBuilder);
            ManyToManyBetweenActivitiesAndSubscribers(modelBuilder);
            OneToManyBetweenCreatorAndActivity(modelBuilder);
        }

        private void ManyToManyBetweenActivitiesAndTegs(ModelBuilder modelBuilder)
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
        
        private void ManyToManyBetweenActivitiesAndSubscribers(ModelBuilder modelBuilder)
        {
            modelBuilder
               .Entity<Activity>()
               .HasMany(a => a.Participants)
               .WithMany(u => u.ActivitiesForParticipation)
               .UsingEntity<ActivityParticipant>(
                 j => j
                   .HasOne(au => au.Participant)
                   .WithMany()
                   .HasPrincipalKey(u => u.Id),
                 j => j
                   .HasOne(au => au.Activity)
                   .WithMany()
                   .HasForeignKey(au => au.ActivityId),
                 j =>
                 {
                     j.HasKey(u => new { u.ActivityId, u.ParticipantId });
                 });
        }

        private void OneToManyBetweenCreatorAndActivity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()
                .HasMany(u => u.CreatedActivities)
                .WithOne(a => a.Creator);
        }
    }
}
