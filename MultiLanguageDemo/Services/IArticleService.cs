using MultiLanguageDemo.Models;

namespace MultiLanguageDemo.Services
{
    public interface IArticleService
    {
        Task<List<ArticleDto>> GetAllArticlesAsync(string culture);
        Task<ArticleDto?> GetArticleBySlugAsync(string slug, string culture);
        Task<List<ArticleDto>> GetFeaturedArticlesAsync(string culture, int count = 3);
        Task<List<ArticleDto>> GetArticlesByCategoryAsync(int categoryId, string culture);
        Task<List<CategoryDto>> GetAllCategoriesAsync(string culture);
    }
}