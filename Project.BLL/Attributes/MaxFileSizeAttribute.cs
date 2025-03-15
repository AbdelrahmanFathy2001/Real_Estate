using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Project.BLL.Attributes
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly double _MaxFileSize;
        public MaxFileSizeAttribute(double MaxFileSize)
        {
            _MaxFileSize = MaxFileSize;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var File = value as IFormFile;
            if (File != null)
            {
                var size = File.Length > _MaxFileSize;
                if (size)
                {
                    return new ValidationResult(ErrorMessage = $"Maximum allowed Size is {_MaxFileSize / 1024} KB allowed !");
                }
            }
            return ValidationResult.Success;
        }
    }
}