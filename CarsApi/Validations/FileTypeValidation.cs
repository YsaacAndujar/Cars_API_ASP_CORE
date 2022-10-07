using System.ComponentModel.DataAnnotations;

namespace CarsApi.Validations
{
    public class FileTypeValidation : ValidationAttribute
    {
        private readonly string[] types;

        public FileTypeValidation(string[] types)
        {
            this.types = types;
        }
        public FileTypeValidation(FileEnumType fileEnumType)
        {
            if(fileEnumType == FileEnumType.Image)
            {
                this.types = new string[] { "image/jpeg", "image/png" };
            }
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
            if (!types.Contains(formFile.ContentType))
            {
                return new ValidationResult($"File type not valid. Send one of the next types: {string.Join(", ", types) }");
            }
            return ValidationResult.Success;
        }
    }
}
