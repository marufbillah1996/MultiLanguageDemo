using System.ComponentModel.DataAnnotations;

namespace MultiLanguageDemo.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? IconClass { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<CategoryTranslation> Translations { get; set; } = new List<CategoryTranslation>();
        public ICollection<Article> Articles { get; set; } = new List<Article>();
    }

    public class CategoryTranslation
    {
        [Key]
        public int TranslationId { get; set; }
        public int CategoryId { get; set; }
        public int LanguageId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public Category? Category { get; set; }
        public Language? Language { get; set; }
    }
}