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

            // Language indexes
            modelBuilder.Entity<Language>()
                .HasIndex(l => l.Code)
                .IsUnique();

            // Article indexes for performance
            modelBuilder.Entity<Article>()
                .HasIndex(a => a.Slug)
                .IsUnique();

            modelBuilder.Entity<Article>()
                .HasIndex(a => a.IsActive);

            modelBuilder.Entity<Article>()
                .HasIndex(a => new { a.CategoryId, a.IsActive });

            modelBuilder.Entity<Article>()
                .HasIndex(a => new { a.IsFeatured, a.IsActive, a.PublishedDate });

            // Category indexes
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Code)
                .IsUnique();

            modelBuilder.Entity<Category>()
                .HasIndex(c => c.IsActive);

            // CategoryTranslation relationships and indexes
            modelBuilder.Entity<CategoryTranslation>()
                .HasOne(ct => ct.Category)
                .WithMany(c => c.Translations)
                .HasForeignKey(ct => ct.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CategoryTranslation>()
                .HasIndex(ct => new { ct.CategoryId, ct.LanguageId })
                .IsUnique();

            // ArticleTranslation relationships and indexes
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