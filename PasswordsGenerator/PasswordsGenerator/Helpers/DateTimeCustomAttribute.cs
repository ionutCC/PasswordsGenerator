using System.ComponentModel.DataAnnotations;

namespace PasswordsGenerator.Helpers
{
    public sealed class DateTimeCustomAttribute: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(Convert.ToDateTime(value) < DateTime.Today)
            {
                return new ValidationResult("Past date not allowed!");
            }
            return ValidationResult.Success;
        }
    }
}
