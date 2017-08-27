using BryceFamily.Repo.Core.Model;
using Microsoft.EntityFrameworkCore;

namespace BryceFamily.Repo.Core
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            
        }

        public DbSet<EventLocation> EventLocations { get; set; }
        public DbSet<FamilyEvent> FamilyEvents { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Person> People { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventLocation>().ToTable("EventLocation");
            modelBuilder.Entity<FamilyEvent>().ToTable("FamilyEvent");
            modelBuilder.Entity<Image>().ToTable("Image");
            modelBuilder.Entity<Person>().ToTable("Person");
        }
    }
}
