using Microsoft.AspNetCore.Mvc;
using MultiLanguageDemo.Services;

namespace MultiLanguageDemo.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IArticleService _articleService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IArticleService articleService, ILogger<HomeController> logger)
        {
            _articleService = articleService;
            _logger = logger;
        }

        [ResponseCache(Duration = 300, VaryByQueryKeys = new[] { "culture" }, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> Index()
        {
            var articles = await _articleService.GetAllArticlesAsync(CurrentCulture);
            var categories = await _articleService.GetAllCategoriesAsync(CurrentCulture);

            ViewBag.CurrentCulture = CurrentCulture;
            ViewBag.IsRtl = IsRtl;
            ViewBag.Categories = categories;

            return View(articles);
        }

        [Route("{culture}/article/{slug}")]
        public async Task<IActionResult> Article(string slug)
        {
            var article = await _articleService.GetArticleBySlugAsync(slug, CurrentCulture);

            if (article == null)
            {
                return NotFound();
            }

            ViewBag.CurrentCulture = CurrentCulture;
            ViewBag.IsRtl = IsRtl;

            return View(article);
        }

        [Route("{culture}/category/{id}")]
        [ResponseCache(Duration = 300, VaryByQueryKeys = new[] { "culture" }, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> Category(int id)
        {
            var articles = await _articleService.GetArticlesByCategoryAsync(id, CurrentCulture);
            var categories = await _articleService.GetAllCategoriesAsync(CurrentCulture);
            var currentCategory = categories.FirstOrDefault(c => c.Id == id);

            ViewBag.CurrentCulture = CurrentCulture;
            ViewBag.IsRtl = IsRtl;
            ViewBag.Categories = categories;
            ViewBag.CurrentCategory = currentCategory;

            return View(articles);
        }

        public IActionResult Privacy()
        {
            ViewBag.CurrentCulture = CurrentCulture;
            ViewBag.IsRtl = IsRtl;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}