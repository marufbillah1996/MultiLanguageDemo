using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MultiLanguageDemo.Data;
using MultiLanguageDemo.Models;

namespace MultiLanguageDemo.Services
{
    public class ArticleService : IArticleService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly ILogger<ArticleService> _logger;
        private const int CacheExpirationMinutes = 15;

        public ArticleService(ApplicationDbContext context, IMemoryCache cache, ILogger<ArticleService> logger)
        {
            _context = context;
            _cache = cache;
            _logger = logger;
        }

        public async Task<List<ArticleDto>> GetAllArticlesAsync(string culture)
        {
            var cacheKey = $"articles_all_{culture}";
            
            if (_cache.TryGetValue(cacheKey, out List<ArticleDto>? cachedArticles) && cachedArticles != null)
            {
                _logger.LogDebug("Returning cached articles for culture: {Culture}", culture);
                return cachedArticles;
            }

            var articles = await _context.Articles
                .AsNoTracking()
                .Where(a => a.IsActive)
                .Include(a => a.Translations.Where(t => t.Language!.Code == culture || t.Language!.IsDefault))
                    .ThenInclude(t => t.Language)
                .Include(a => a.Category).ThenInclude(c => c!.Translations.Where(t => t.Language!.Code == culture || t.Language!.IsDefault))
                    .ThenInclude(t => t.Language)
                .OrderByDescending(a => a.PublishedDate)
                .ToListAsync();

            var result = articles.Select(a => MapToDto(a, culture)).ToList();
            
            _cache.Set(cacheKey, result, TimeSpan.FromMinutes(CacheExpirationMinutes));
            _logger.LogDebug("Cached {Count} articles for culture: {Culture}", result.Count, culture);
            
            return result;
        }

        public async Task<ArticleDto?> GetArticleBySlugAsync(string slug, string culture)
        {
            var article = await _context.Articles
                .Where(a => a.Slug == slug && a.IsActive)
                .Include(a => a.Translations.Where(t => t.Language!.Code == culture || t.Language!.IsDefault))
                    .ThenInclude(t => t.Language)
                .Include(a => a.Category).ThenInclude(c => c!.Translations.Where(t => t.Language!.Code == culture || t.Language!.IsDefault))
                    .ThenInclude(t => t.Language)
                .FirstOrDefaultAsync();

            if (article == null) return null;

            // Update view count asynchronously without waiting
            _ = Task.Run(async () =>
            {
                try
                {
                    var articleToUpdate = await _context.Articles.FindAsync(article.ArticleId);
                    if (articleToUpdate != null)
                    {
                        articleToUpdate.ViewCount++;
                        await _context.SaveChangesAsync();
                        
                        // Invalidate cache for this article
                        _cache.Remove($"articles_all_{culture}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to update view count for article: {Slug}", slug);
                }
            });

            return MapToDto(article, culture);
        }

        public async Task<List<ArticleDto>> GetFeaturedArticlesAsync(string culture, int count = 3)
        {
            var cacheKey = $"articles_featured_{culture}_{count}";
            
            if (_cache.TryGetValue(cacheKey, out List<ArticleDto>? cachedArticles) && cachedArticles != null)
            {
                _logger.LogDebug("Returning cached featured articles for culture: {Culture}", culture);
                return cachedArticles;
            }

            var articles = await _context.Articles
                .AsNoTracking()
                .Where(a => a.IsActive && a.IsFeatured)
                .Include(a => a.Translations.Where(t => t.Language!.Code == culture || t.Language!.IsDefault))
                    .ThenInclude(t => t.Language)
                .Include(a => a.Category).ThenInclude(c => c!.Translations.Where(t => t.Language!.Code == culture || t.Language!.IsDefault))
                    .ThenInclude(t => t.Language)
                .OrderByDescending(a => a.PublishedDate)
                .Take(count)
                .ToListAsync();

            var result = articles.Select(a => MapToDto(a, culture)).ToList();
            
            _cache.Set(cacheKey, result, TimeSpan.FromMinutes(CacheExpirationMinutes));
            
            return result;
        }

        public async Task<List<ArticleDto>> GetArticlesByCategoryAsync(int categoryId, string culture)
        {
            var cacheKey = $"articles_category_{categoryId}_{culture}";
            
            if (_cache.TryGetValue(cacheKey, out List<ArticleDto>? cachedArticles) && cachedArticles != null)
            {
                _logger.LogDebug("Returning cached articles for category: {CategoryId}, culture: {Culture}", categoryId, culture);
                return cachedArticles;
            }

            var articles = await _context.Articles
                .AsNoTracking()
                .Where(a => a.CategoryId == categoryId && a.IsActive)
                .Include(a => a.Translations.Where(t => t.Language!.Code == culture || t.Language!.IsDefault))
                    .ThenInclude(t => t.Language)
                .Include(a => a.Category).ThenInclude(c => c!.Translations.Where(t => t.Language!.Code == culture || t.Language!.IsDefault))
                    .ThenInclude(t => t.Language)
                .OrderByDescending(a => a.PublishedDate)
                .ToListAsync();

            var result = articles.Select(a => MapToDto(a, culture)).ToList();
            
            _cache.Set(cacheKey, result, TimeSpan.FromMinutes(CacheExpirationMinutes));
            
            return result;
        }

        public async Task<List<CategoryDto>> GetAllCategoriesAsync(string culture)
        {
            var cacheKey = $"categories_all_{culture}";
            
            if (_cache.TryGetValue(cacheKey, out List<CategoryDto>? cachedCategories) && cachedCategories != null)
            {
                _logger.LogDebug("Returning cached categories for culture: {Culture}", culture);
                return cachedCategories;
            }

            var categories = await _context.Categories
                .AsNoTracking()
                .Where(c => c.IsActive)
                .Include(c => c.Translations.Where(t => t.Language!.Code == culture || t.Language!.IsDefault))
                    .ThenInclude(t => t.Language)
                .Include(c => c.Articles.Where(a => a.IsActive))
                .ToListAsync();

            var result = categories.Select(c => MapCategoryToDto(c, culture)).ToList();
            
            // Categories change less frequently, cache for longer
            _cache.Set(cacheKey, result, TimeSpan.FromMinutes(30));
            
            return result;
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