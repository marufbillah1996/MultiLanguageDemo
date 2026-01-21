using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using MultiLanguageDemo.Helpers;
using System.Globalization;

namespace MultiLanguageDemo.Controllers
{
    public class BaseController : Controller
    {
        protected string CurrentCulture => CultureInfo.CurrentCulture.Name;
        protected string CurrentUICulture => CultureInfo.CurrentUICulture.Name;
        protected bool IsRtl => CultureHelper.IsRightToLeft(CurrentCulture);

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            culture = CultureHelper.GetImplementedCulture(culture);

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    IsEssential = true,
                    SameSite = SameSiteMode.Lax,
                    HttpOnly = false
                }
            );

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                var urlParts = returnUrl.Split('/');
                if (urlParts.Length > 1 && CultureHelper.GetImplementedCulture(urlParts[1]) != CultureHelper.GetDefaultCulture())
                {
                    urlParts[1] = culture;
                    returnUrl = string.Join('/', urlParts);
                }
                else
                {
                    //returnUrl = $"/{culture}{returnUrl}";
                    returnUrl = $"{returnUrl}";
                }

                return LocalRedirect(returnUrl);
            }

            return LocalRedirect($"/{culture}/");
        }
    }
}