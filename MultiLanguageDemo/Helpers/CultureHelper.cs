using System.Globalization;

namespace MultiLanguageDemo.Helpers
{
    public static class CultureHelper
    {
        private static readonly string[] SupportedCultures =
        {
            "en-US", "bn-BD", "fr-FR", "ar-SA", "zh-CN"
        };

        public static string GetImplementedCulture(string name)
        {
            if (string.IsNullOrEmpty(name))
                return GetDefaultCulture();

            if (SupportedCultures.Contains(name, StringComparer.OrdinalIgnoreCase))
                return name;

            var neutralCulture = name.Split('-')[0];
            var match = SupportedCultures.FirstOrDefault(c =>
                c.StartsWith(neutralCulture, StringComparison.OrdinalIgnoreCase));

            return match ?? GetDefaultCulture();
        }

        public static string GetDefaultCulture() => SupportedCultures[0];

        public static bool IsRightToLeft(string culture)
        {
            var rtlCultures = new[] { "ar", "he", "fa", "ur" };
            return rtlCultures.Any(rtl => culture.StartsWith(rtl, StringComparison.OrdinalIgnoreCase));
        }

        public static string GetCurrentCulture() => CultureInfo.CurrentCulture.Name;

        public static List<CultureViewModel> GetSupportedCultureList()
        {
            return new List<CultureViewModel>
            {
                new CultureViewModel { Code = "en-US", Name = "English", NativeName = "English", FlagIcon = "🇺🇸", Direction = "ltr" },
                new CultureViewModel { Code = "bn-BD", Name = "Bengali", NativeName = "বাংলা", FlagIcon = "🇧🇩", Direction = "ltr" },
                new CultureViewModel { Code = "fr-FR", Name = "French", NativeName = "Français", FlagIcon = "🇫🇷", Direction = "ltr" },
                new CultureViewModel { Code = "ar-SA", Name = "Arabic", NativeName = "العربية", FlagIcon = "🇸🇦", Direction = "rtl" },
                new CultureViewModel { Code = "zh-CN", Name = "Chinese", NativeName = "中文", FlagIcon = "🇨🇳", Direction = "ltr" }
            };
        }
    }

    public class CultureViewModel
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string NativeName { get; set; } = string.Empty;
        public string FlagIcon { get; set; } = string.Empty;
        public string Direction { get; set; } = "ltr";
    }
}