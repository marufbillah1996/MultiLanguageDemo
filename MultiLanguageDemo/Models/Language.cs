using System.ComponentModel.DataAnnotations;

namespace MultiLanguageDemo.Models
{
    public class Language
    {
        public int LanguageId { get; set; }

        [Required]
        [MaxLength(10)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(50)]
        public string NativeName { get; set; } = string.Empty;

        public bool IsDefault { get; set; }
        public bool IsActive { get; set; } = true;

        [MaxLength(10)]
        public string? FlagIcon { get; set; }

        [MaxLength(3)]
        public string Direction { get; set; } = "ltr"; // ltr or rtl
    }
}
