using Microsoft.AspNetCore.Mvc;
using MultiLanguageDemo.Helpers;
using MultiLanguageDemo.Models;
using MultiLanguageDemo.Services;
using System.Globalization;

namespace MultiLanguageDemo.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesApiController : ControllerBase
    {
        private readonly IArticleService _articleService;

        public ArticlesApiController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        private string GetCultureFromRequest()
        {
            if (Request.Query.ContainsKey("culture"))
            {
                var culture = Request.Query["culture"].FirstOrDefault();
                if (!string.IsNullOrEmpty(culture))
                    return CultureHelper.GetImplementedCulture(culture);
            }

            if (Request.Headers.ContainsKey("X-Language"))
            {
                var culture = Request.Headers["X-Language"].FirstOrDefault();
                if (!string.IsNullOrEmpty(culture))
                    return CultureHelper.GetImplementedCulture(culture);
            }

            var currentCulture = CultureInfo.CurrentCulture.Name;
            return CultureHelper.GetImplementedCulture(currentCulture);
        }

        [HttpGet]
        [ResponseCache(Duration = 300, VaryByQueryKeys = new[] { "culture" })]
        public async Task<IActionResult> GetArticles()
        {
            var culture = GetCultureFromRequest();
            var articles = await _articleService.GetAllArticlesAsync(culture);

            return Ok(new ApiResponse<List<ArticleDto>>
            {
                Success = true,
                Data = articles,
                Culture = culture,
                Message = "Articles retrieved successfully",
                Timestamp = DateTime.UtcNow
            });
        }

        [HttpGet("{slug}")]
        [ResponseCache(Duration = 600, VaryByQueryKeys = new[] { "culture" })]
        public async Task<IActionResult> GetArticle(string slug)
        {
            var culture = GetCultureFromRequest();
            var article = await _articleService.GetArticleBySlugAsync(slug, culture);

            if (article == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Article '{slug}' not found",
                    Culture = culture,
                    Timestamp = DateTime.UtcNow
                });
            }

            return Ok(new ApiResponse<ArticleDto>
            {
                Success = true,
                Data = article,
                Culture = culture,
                Message = "Article retrieved successfully",
                Timestamp = DateTime.UtcNow
            });
        }

        [HttpGet("featured")]
        [ResponseCache(Duration = 600, VaryByQueryKeys = new[] { "culture", "count" })]
        public async Task<IActionResult> GetFeaturedArticles([FromQuery] int count = 3)
        {
            var culture = GetCultureFromRequest();
            var articles = await _articleService.GetFeaturedArticlesAsync(culture, count);

            return Ok(new ApiResponse<List<ArticleDto>>
            {
                Success = true,
                Data = articles,
                Culture = culture,
                Message = "Featured articles retrieved successfully",
                Timestamp = DateTime.UtcNow
            });
        }

        [HttpGet("categories")]
        [ResponseCache(Duration = 1800, VaryByQueryKeys = new[] { "culture" })]
        public async Task<IActionResult> GetCategories()
        {
            var culture = GetCultureFromRequest();
            var categories = await _articleService.GetAllCategoriesAsync(culture);

            return Ok(new ApiResponse<List<CategoryDto>>
            {
                Success = true,
                Data = categories,
                Culture = culture,
                Message = "Categories retrieved successfully",
                Timestamp = DateTime.UtcNow
            });
        }
    }

    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Culture { get; set; }
        public DateTime Timestamp { get; set; }
    }
}