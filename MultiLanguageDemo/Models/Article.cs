using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiLanguageDemo.Models
{
    public class Article
    {
        [Key]//fixes with deepseek
        public int ArticleId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Slug { get; set; } = string.Empty;

        public int CategoryId { get; set; }

        [Required]
        [MaxLength(100)]
        public string AuthorName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        public DateTime PublishedDate { get; set; } = DateTime.UtcNow;

        public int ViewCount { get; set; } = 0;
        public int ReadingTime { get; set; } = 5;
        public bool IsFeatured { get; set; } = false;
        public bool IsActive { get; set; } = true;
        [ForeignKey("CategoryId")]//fixes with deepseek
        public Category? Category { get; set; }
        public ICollection<ArticleTranslation> Translations { get; set; } = new List<ArticleTranslation>();
    }

    public class ArticleTranslation
    {
        [Key] //fixes with deepseek
        public int TranslationId { get; set; }
        public int ArticleId { get; set; }
        public int LanguageId { get; set; }

        [Required]
        [MaxLength(300)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Subtitle { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Summary { get; set; }

        [MaxLength(300)]
        public string? MetaDescription { get; set; }

        [MaxLength(500)]
        public string? Tags { get; set; }

        // Navigation properties
        [ForeignKey("ArticleId")]//fixes with deepseek
        public Article? Article { get; set; }

        [ForeignKey("LanguageId")]//fixes with deepseek
        public Language? Language { get; set; }
    }
}