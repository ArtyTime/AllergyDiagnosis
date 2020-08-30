using Microsoft.EntityFrameworkCore;

namespace Allergy.Models
{
    public class AllergiesDbContext : DbContext
    {
        public DbSet<Allergies> Allergics { get; set; }

        public AllergiesDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Allergies>().HasData(
                new Allergies[]
                {
                new Allergies (1, "Mary Susan", "Peanuts Cats Tomatoes Strawberries"),
                new Allergies (2, "Katty Perry", "Shellfish Cats Tomatoes Eggs Pollen"),
                new Allergies (3, "Don Kihot"),
                new Allergies (4, "Robert Bobert", 22),
                new Allergies (5, "Candy Summers", 4)
                });
        }
    }
}
