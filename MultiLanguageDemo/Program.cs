using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using MultiLanguageDemo.Data;
using MultiLanguageDemo.Services;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Configure supported cultures
var supportedCultures = new[]
{
    new CultureInfo("en-US"),
    new CultureInfo("bn-BD"),
    new CultureInfo("fr-FR"),
    new CultureInfo("ar-SA"),
    new CultureInfo("zh-CN")
};

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Add services
builder.Services.AddScoped<IArticleService, ArticleService>();

// Add MVC with localization
builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();

// Configure request localization
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    options.RequestCultureProviders = new List<IRequestCultureProvider>
    {
        new QueryStringRequestCultureProvider { QueryStringKey = "culture" },
        new CookieRequestCultureProvider { CookieName = ".AspNetCore.Culture" },
        new AcceptLanguageHeaderRequestCultureProvider()
    };
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// IMPORTANT: UseRequestLocalization must be here
var localizationOptions = app.Services.GetRequiredService<Microsoft.Extensions.Options.IOptions<RequestLocalizationOptions>>().Value;
app.UseRequestLocalization(localizationOptions);

app.UseRouting();
app.UseSession();
app.UseAuthorization();

// Routes
app.MapControllerRoute(
    name: "setlanguage",
    pattern: "Base/SetLanguage",
    defaults: new { controller = "Base", action = "SetLanguage" });

app.MapControllerRoute(
    name: "article",
    pattern: "{culture}/article/{slug}",
    defaults: new { controller = "Home", action = "Article" });

app.MapControllerRoute(
    name: "category",
    pattern: "{culture}/category/{id}",
    defaults: new { controller = "Home", action = "Category" });

app.MapControllerRoute(
    name: "default",
    pattern: "{culture=en-US}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "api",
    pattern: "api/{controller}/{action}/{id?}");

app.Run();