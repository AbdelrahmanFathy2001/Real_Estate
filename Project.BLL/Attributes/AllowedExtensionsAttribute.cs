using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Project.BLL.Attributes
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string _allowedExtension;
        public AllowedExtensionsAttribute(string allowedExtension)
        {
            _allowedExtension = allowedExtension;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var File = value as IFormFile;
            if (File != null)
            {
                var extension = Path.GetExtension(File.FileName);
                bool IsAllow = _allowedExtension.Split(',').Contains(extension, StringComparer.OrdinalIgnoreCase);
                if (!IsAllow)
                {
                    return new ValidationResult(ErrorMessage = $"Only {_allowedExtension} allowed !");
                }
            }
            return ValidationResult.Success;
        }
    }
}