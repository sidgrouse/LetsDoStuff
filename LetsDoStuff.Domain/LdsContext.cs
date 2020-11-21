using LetsDoStuff.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LetsDoStuff.Domain
{
    public class LdsContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public LdsContext(DbContextOptions<LdsContext> options)
            : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=LetsDoStuffDb;Trusted_Connection=True;");
        // }
    }
}
