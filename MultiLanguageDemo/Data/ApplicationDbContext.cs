using Microsoft.EntityFrameworkCore;
using MultiLanguageDemo.Models;

namespace MultiLanguageDemo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Language> Languages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryTranslation> CategoryTranslations { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleTranslation> ArticleTranslations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Language>()
                .HasIndex(l => l.Code)
                .IsUnique();

            modelBuilder.Entity<CategoryTranslation>()
                .HasOne(ct => ct.Category)
                .WithMany(c => c.Translations)
                .HasForeignKey(ct => ct.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CategoryTranslation>()
                .HasIndex(ct => new { ct.CategoryId, ct.LanguageId })
                .IsUnique();

            modelBuilder.Entity<ArticleTranslation>()
                .HasOne(at => at.Article)
                .WithMany(a => a.Translations)
                .HasForeignKey(at => at.ArticleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ArticleTranslation>()
                .HasIndex(at => new { at.ArticleId, at.LanguageId })
                .IsUnique();
        }
    }
}