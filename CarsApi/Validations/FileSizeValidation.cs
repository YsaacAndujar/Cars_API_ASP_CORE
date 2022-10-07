using System.ComponentModel.DataAnnotations;

namespace CarsApi.Validations
{
    public class FileSizeValidation: ValidationAttribute
    {
        private readonly int max;

        public FileSizeValidation(int max)
        {
            this.max = max;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            IFormFile formFile = value as IFormFile;
            if (formFile == null)
            {
                return ValidationResult.Success;
            }
            if(formFile.Length > max * 1024 * 1024)
            {
                return new ValidationResult($"This file size cant be more than {max}mb");
            }
            return ValidationResult.Success;
        }
    }
}
