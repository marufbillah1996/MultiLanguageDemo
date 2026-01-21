namespace MultiLanguageDemo.Models
{
    public class ArticleDto
    {
        public int Id { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Subtitle { get; set; }
        public string Content { get; set; } = string.Empty;
        public string? Summary { get; set; }
        public string? ImageUrl { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string CategoryIcon { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public DateTime PublishedDate { get; set; }
        public int ViewCount { get; set; }
        public int ReadingTime { get; set; }
        public bool IsFeatured { get; set; }
        public string? Tags { get; set; }
        public string Culture { get; set; } = string.Empty;
    }

    public class CategoryDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? IconClass { get; set; }
        public int ArticleCount { get; set; }
    }
}