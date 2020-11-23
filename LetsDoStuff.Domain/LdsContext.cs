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
            Database.EnsureCreated();
        }
    }
}
