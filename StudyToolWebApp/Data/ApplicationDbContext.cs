using Microsoft.EntityFrameworkCore;
using StudyToolWebApp.Models;

namespace StudyToolWebApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Card> cards { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Deck> Decks { get; set; }
        public DbSet<CardCategory> CardCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CardCategory>()
                .HasKey(cc => new { cc.CardId, cc.CategoryId });
            modelBuilder.Entity<CardCategory>()
                .HasOne(c => c.Card)
                .WithMany(cc => cc.Categories)
                .HasForeignKey(c => c.CardId);
            modelBuilder.Entity<CardCategory>()
                .HasOne(c => c.Category)
                .WithMany(cc => cc.Cards)
                .HasForeignKey(c => c.CategoryId);
        }
    }
}
