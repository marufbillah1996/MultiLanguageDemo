using System.Globalization;
using System.Resources;

namespace MultiLanguageDemo.Helpers
{
    public static class ResourceHelper
    {
        private static ResourceManager _resourceManager =
            new ResourceManager("MultiLanguageDemo.Resources.SharedResource",
                typeof(ResourceHelper).Assembly);

        public static string GetString(string key)
        {
            try
            {
                var culture = CultureInfo.CurrentUICulture;
                var value = _resourceManager.GetString(key, culture);
                return value ?? key;
            }
            catch
            {
                return key;
            }
        }
    }
}