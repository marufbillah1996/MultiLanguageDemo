using Microsoft.EntityFrameworkCore;
using MultiLanguageDemo.Data;
using MultiLanguageDemo.Models;

namespace MultiLanguageDemo.Services
{
    public class ArticleService : IArticleService
    {
        private readonly ApplicationDbContext _context;

        public ArticleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ArticleDto>> GetAllArticlesAsync(string culture)
        {
            var articles = await _context.Articles
                .Where(a => a.IsActive)
                .Include(a => a.Translations).ThenInclude(t => t.Language)
                .Include(a => a.Category).ThenInclude(c => c!.Translations).ThenInclude(t => t.Language)
                .OrderByDescending(a => a.PublishedDate)
                .ToListAsync();

            return articles.Select(a => MapToDto(a, culture)).ToList();
        }

        public async Task<ArticleDto?> GetArticleBySlugAsync(string slug, string culture)
        {
            var article = await _context.Articles
                .Where(a => a.Slug == slug && a.IsActive)
                .Include(a => a.Translations).ThenInclude(t => t.Language)
                .Include(a => a.Category).ThenInclude(c => c!.Translations).ThenInclude(t => t.Language)
                .FirstOrDefaultAsync();

            if (article == null) return null;

            article.ViewCount++;
            await _context.SaveChangesAsync();

            return MapToDto(article, culture);
        }

        public async Task<List<ArticleDto>> GetFeaturedArticlesAsync(string culture, int count = 3)
        {
            var articles = await _context.Articles
                .Where(a => a.IsActive && a.IsFeatured)
                .Include(a => a.Translations).ThenInclude(t => t.Language)
                .Include(a => a.Category).ThenInclude(c => c!.Translations).ThenInclude(t => t.Language)
                .OrderByDescending(a => a.PublishedDate)
                .Take(count)
                .ToListAsync();

            return articles.Select(a => MapToDto(a, culture)).ToList();
        }

        public async Task<List<ArticleDto>> GetArticlesByCategoryAsync(int categoryId, string culture)
        {
            var articles = await _context.Articles
                .Where(a => a.CategoryId == categoryId && a.IsActive)
                .Include(a => a.Translations).ThenInclude(t => t.Language)
                .Include(a => a.Category).ThenInclude(c => c!.Translations).ThenInclude(t => t.Language)
                .OrderByDescending(a => a.PublishedDate)
                .ToListAsync();

            return articles.Select(a => MapToDto(a, culture)).ToList();
        }

        public async Task<List<CategoryDto>> GetAllCategoriesAsync(string culture)
        {
            var categories = await _context.Categories
                .Where(c => c.IsActive)
                .Include(c => c.Translations).ThenInclude(t => t.Language)
                .Include(c => c.Articles)
                .ToListAsync();

            return categories.Select(c => MapCategoryToDto(c, culture)).ToList();
        }

        private ArticleDto MapToDto(Article article, string culture)
        {
            var translation = article.Translations
                .FirstOrDefault(t => t.Language!.Code == culture)
                ?? article.Translations.FirstOrDefault(t => t.Language!.IsDefault);

            var categoryTranslation = article.Category?.Translations
                .FirstOrDefault(t => t.Language!.Code == culture)
                ?? article.Category?.Translations.FirstOrDefault(t => t.Language!.IsDefault);

            return new ArticleDto
            {
                Id = article.ArticleId,
                Slug = article.Slug,
                Title = translation?.Title ?? "N/A",
                Subtitle = translation?.Subtitle,
                Content = translation?.Content ?? string.Empty,
                Summary = translation?.Summary,
                ImageUrl = article.ImageUrl,
                CategoryName = categoryTranslation?.Name ?? "N/A",
                CategoryIcon = article.Category?.IconClass ?? "bi-file-text",
                AuthorName = article.AuthorName,
                PublishedDate = article.PublishedDate,
                ViewCount = article.ViewCount,
                ReadingTime = article.ReadingTime,
                IsFeatured = article.IsFeatured,
                Tags = translation?.Tags,
                Culture = culture
            };
        }

        private CategoryDto MapCategoryToDto(Category category, string culture)
        {
            var translation = category.Translations
                .FirstOrDefault(t => t.Language!.Code == culture)
                ?? category.Translations.FirstOrDefault(t => t.Language!.IsDefault);

            return new CategoryDto
            {
                Id = category.CategoryId,
                Code = category.Code,
                Name = translation?.Name ?? "N/A",
                Description = translation?.Description,
                IconClass = category.IconClass,
                ArticleCount = category.Articles.Count(a => a.IsActive)
            };
        }
    }
}